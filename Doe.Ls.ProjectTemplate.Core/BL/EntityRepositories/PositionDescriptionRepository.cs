using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

using Unity.Attributes;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories
    {
    public partial class PositionDescriptionRepository : BaseRepository<PositionDescription>
        {
        public ServiceRepository ServiceRepository
            {
            get
                {
                return new ServiceRepository(this.RepositoryFactory);
                }
            }
        public PositionDescriptionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
            {
            }

        public IQueryable<PositionDescription> ActiveList()
            {
            return List().Where(pd => pd.RolePositionDescription.StatusId != (int)Enums.StatusValue.Deleted);

            }
        public IQueryable<PositionDescription> LiveList()
            {
            var stsList = new int[] { (int)Enums.StatusValue.Imported, (int)Enums.StatusValue.Approved };
            return List().Where(pd => stsList.Contains(pd.RolePositionDescription.StatusId));

            }
        public override IQueryable<PositionDescription> List()
            {
            return base.List()
                    .Include(ent => ent.RolePositionDescription)
                    .Include(ent => ent.RolePositionDescription.Grade)
                    .Include(ent => ent.RolePositionDescription.StatusValue)
                    .Include(ent => ent.PositionFocusCriterias)
                    .OrderBy(ent => ent.PositionDescriptionId);
            }
        public IQueryable<PositionDescription> ListWithSelectionCriteria()
            {
            return this.List()
                    .Include(ent => ent.RolePositionDescription)
                    .Include("PositionFocusCriterias.LookupFocusGradeCriteria.Focus")
                    .Include("PositionFocusCriterias.LookupFocusGradeCriteria.SelectionCriteria")
                    .OrderBy(ent => ent.PositionDescriptionId);
            }

        public PositionDescription LoadPositionDescById(int id)
            {
            return ListWithSelectionCriteria()
                   .FirstOrDefault(p => p.PositionDescriptionId == id);
            }
        public override void Insert(PositionDescription entity)
            {
            entity.BriefRoleStatement = entity.BriefRoleStatement?.Trim() ?? "";
            entity.StatementOfDuties = entity.StatementOfDuties?.Trim() ?? "";
            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Insert(entity);
            }

        public override void Update(PositionDescription entity, bool refresh = true)
            {
            entity.BriefRoleStatement = entity.BriefRoleStatement?.Trim() ?? "";
            entity.StatementOfDuties = entity.StatementOfDuties?.Trim() ?? "";
            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Update(entity, refresh);
            }
     

        public IQueryable<PositionDescription> FilterDisplayedPositions(JQueryDataTableRolePositionDesc arg, IQueryable<PositionDescription> positionDescriptions
            )
            {
            var displayedPositionDescription = positionDescriptions;
            SearchArg searchArgs;
            if(arg.StatusCode != null || arg.GradeCode != null)
                {


                if(arg.StatusCode != null)
                    {
                    var intArr = arg.StatusCode.ToParamList().CastToIntegerList();
                    if(intArr.Any())
                        {
                        displayedPositionDescription =
                            displayedPositionDescription.Where(
                                ent => intArr.Contains(ent.RolePositionDescription.StatusId));
                        }
                    }
                if(arg.GradeCode != null)
                    {
                    var gradeList = arg.GradeCode.ToParamList();
                    if(gradeList.Any())
                        {
                        displayedPositionDescription =
                            displayedPositionDescription.Where(
                                ent => gradeList.Contains(ent.RolePositionDescription.GradeCode));
                        }
                    }

                if(!string.IsNullOrWhiteSpace(arg.sSearch))
                    {
                    searchArgs = new SearchArg { Search = arg.sSearch, };
                    displayedPositionDescription = this.FilterPositionDescriptions(displayedPositionDescription, searchArgs);
                    }

                displayedPositionDescription = CustomOrderBy.CustomSort(displayedPositionDescription, arg);
                }
            else if(!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                searchArgs = new SearchArg { Search = arg.sSearch };
                displayedPositionDescription = this.FilterPositionDescriptions(displayedPositionDescription, searchArgs);
                }
            else
                {
                displayedPositionDescription = CustomOrderBy.CustomSort(displayedPositionDescription, arg);
                }
            return displayedPositionDescription;
            }

        public IQueryable<PositionDescription> FilterPositionDescriptions(IQueryable<PositionDescription> positionDescriptions, SearchArg searchArg)
            {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionDescription = positionDescriptions.Where(ent =>
                    (!string.IsNullOrEmpty(ent.RolePositionDescription.Title) && ent.RolePositionDescription.Title.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RolePositionDescription.DocNumber) && ent.RolePositionDescription.DocNumber.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RolePositionDescription.LastModifiedBy.ToString()) && ent.RolePositionDescription.LastModifiedBy.ToString().ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.BriefRoleStatement) && ent.BriefRoleStatement.ToLower().Contains(searchWord))
);

            return filteredPositionDescription.OrderBy(e => e.PositionDescriptionId);
            }


        public void CreatePositionDescription(PositionDescription positionDescription, RolePositionDescription rolepositionDesc)
            {
            var guid = UnitOfWork.StratTransction();

            try
                {
                var rolePosRep = ServiceRepository.RolePositionDescriptionRepository();
                rolepositionDesc.Version = 1;
                rolepositionDesc.IsPositionDescription = true;
                rolepositionDesc.StatusId = (int)Enums.StatusValue.Draft;
                rolePosRep.Insert(rolepositionDesc);
                positionDescription.PositionDescriptionId = rolepositionDesc.RolePositionDescId;
                positionDescription.RolePositionDescription = null;
                this.Insert(positionDescription);
                UnitOfWork.CommitTransaction(guid);
                }

            catch(Exception ex) //error occurred
                {
                UnitOfWork.RollbackTransaction(guid);
                throw new Exception(ex.Message);
                //Handel error
                }

            }

        public void UpdatePositionDescription(RolePositionDescription rolePosdesc)
            {

            var guid = UnitOfWork.StratTransction();

            try
                {
                var rolePosRep = ServiceRepository.RolePositionDescriptionRepository();
                rolePosRep.Update(rolePosdesc);
                UnitOfWork.CommitTransaction(guid);
                }

            catch(Exception ex) //error occurred
                {
                UnitOfWork.RollbackTransaction(guid);
                throw new Exception(ex.Message);
                //Handel error
                }
            }


        public PositionDescription GetPositionDescriptionById(int positionDescriptionId)
            {
            return List().SingleOrDefault(p => p.PositionDescriptionId == positionDescriptionId);
            }

        public PositionDescription GetEmptyModel()
            {
            var model = new PositionDescription()
                {
                RolePositionDescription = new RolePositionDescription
                    {
                    RolePositionDescId = 0,
                    StatusValue = new StatusValue
                        {
                        StatusId = (int)Enums.StatusValue.Draft,
                        StatusName = Enums.StatusValue.Draft.ToString()

                        },
                    IsPositionDescription = true,
                    },
                PositionDescriptionId = 0,


                };

            return model;
            }

        public void ClonePositionDescrition(PositionDescription newPositionDesc, 
            RolePositionDescription newRolePositionDesc,string sourceDocNum, 
            int sourcePositionDescId, string userName)
        {
            //1. RolePositionDes, PositionDesc
            CreatePositionDescription(newPositionDesc, newRolePositionDesc);
            //2. load source selection criteria list
            var sourceSelectionCriteriaItems = ServiceRepository.PositionFocusCriteriaRepository()
               .List().Where(l => l.PositionDescriptionId == sourcePositionDescId)
               .ToList();
            
            var newSelFocusList = new List<PositionFocusCriteria>();
            foreach (var criteria in sourceSelectionCriteriaItems)
            {
                //replace the positionDescId with the new PositionDesc
                var positionFocusCriteria = new PositionFocusCriteria
                {
                    LookupId = criteria.LookupId,
                    PositionDescriptionId = newPositionDesc.PositionDescriptionId,
                    LookupCustomContent = criteria.LookupCustomContent,
                    LastModifiedDate = DateTime.Now,
                    LastModifiedBy = userName
                };
                newSelFocusList.Add(positionFocusCriteria);

            }
            ServiceRepository.PositionFocusCriteriaRepository()
               .UpdatePositionFocusCriteria(newRolePositionDesc.RolePositionDescId, newSelFocusList);

            //3.History
            ServiceRepository.RolePositionDescriptionHistoryRepository()
                .LogHistoryOnClone(newRolePositionDesc, sourceDocNum, userName);
        }

        
        [Unity.Attributes.Dependency]
        public RolePositionDescriptionRepository RolePositionDescriptionRepository { get; set; }
        }
    }



