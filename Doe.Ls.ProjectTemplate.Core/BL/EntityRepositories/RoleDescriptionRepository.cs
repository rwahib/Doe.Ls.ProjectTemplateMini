using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Data;


namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories
    {
    public partial class RoleDescriptionRepository : BaseRepository<RoleDescription>
        {
        public RoleDescriptionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
            {
            }

        public ServiceRepository ServiceRepository
            {
            get
                {
                return new ServiceRepository(this.RepositoryFactory);
                }
            }
        public override IQueryable<RoleDescription> List()
            {
            return base.List()
                    .Include(ent => ent.KeyRelationships)
                    .Include(ent => ent.RoleCapabilities)
                    .Include(ent => ent.RolePositionDescription)
                    .Include(ent => ent.RolePositionDescription.Grade)
                    .Include(ent => ent.RolePositionDescription.StatusValue)
                    .OrderBy(ent => ent.RoleDescriptionId);
            }
        public IQueryable<RoleDescription> ActiveList()
            {
            return this.List().Where(rd => rd.RolePositionDescription.StatusId != (int)Enums.StatusValue.Deleted);

            }
        public IQueryable<RoleDescription> LiveList()
            {
            var stsList = new int[] { (int)Enums.StatusValue.Imported, (int)Enums.StatusValue.Approved };
            return this.List().Where(rd => stsList.Contains(rd.RolePositionDescription.StatusId));

            }
        public RoleDescription GetRoleDescriptionById(int roleDescriptionId)
            {
            return List().SingleOrDefault(r => r.RoleDescriptionId == roleDescriptionId);
            }
        public IQueryable<RoleDescription> ListForCapabilitiesOnly()
            {
            return this.List()
                    .Include(ent => ent.RoleCapabilities)
                    .OrderBy(ent => ent.RoleDescriptionId);
            }

        public RoleDescription LoadRoleDescWithRoleCapabilitiesOnly(int roleDescId)
            {
            return ListForCapabilitiesOnly().FirstOrDefault(r => r.RoleDescriptionId == roleDescId);
            }

        public IQueryable<RoleDescription> ListForCapabilityFramework()
            {
            return base.List()
                .Include(ent => ent.KeyRelationships)
                .Include(ent => ent.RoleCapabilities)
                .Include(ent => ent.RolePositionDescription)
                .Include(ent => ent.RolePositionDescription.Grade)
                .Include(ent => ent.RolePositionDescription.StatusValue)
                .Include(ent => ent.RoleCapabilities.Select(rc => rc.CapabilityLevel))
                .Include(ent => ent.RoleCapabilities.Select(rc => rc.CapabilityName))
                .Include(ent => ent.RoleCapabilities.Select(rc => rc.CapabilityName.CapabilityBehaviourIndicators))
                .Include(ent => ent.RoleCapabilities.Select(rc => rc.CapabilityName.CapabilityGroup));

            }

        public RoleDescription LoadRoleDescWithCapabilityFramework(int roleDescId)
            {
            return ListForCapabilityFramework().SingleOrDefault(l => l.RoleDescriptionId == roleDescId);
            }

        public IQueryable<RoleDescription> ListForkeyRelationships()
            {
            return base.List()
                .Include(ent => ent.KeyRelationships)
                .Include(ent => ent.KeyRelationships.Select(r => r.RelationshipScope))
                .Include(ent => ent.RolePositionDescription)
                .Include(ent => ent.RolePositionDescription.Grade)
                .Include(ent => ent.RolePositionDescription.StatusValue);
            }

        public RoleDescription LoadRoleDescWithKeyRelationships(int roleDescId)
            {
            return ListForkeyRelationships().SingleOrDefault(r => r.RoleDescriptionId == roleDescId);
            }

        public IQueryable<RoleDescription> ListForCaption()
            {
            return base.List()
                    .Include(ent => ent.RolePositionDescription.Grade)
                    .Include(ent => ent.RolePositionDescription)
                    .Include(ent => ent.RolePositionDescription.StatusValue)
                    .OrderBy(ent => ent.RoleDescriptionId);
            }

        public IQueryable<RoleDescription> ListForPrimitiveItems()
            {
            return base.List()
                    .Include(ent => ent.RolePositionDescription.Grade)
                    .Include(ent => ent.RolePositionDescription)
                    .Include(ent => ent.RolePositionDescription.StatusValue)
                    .OrderBy(ent => ent.RoleDescriptionId);
            }

        public RoleDescription GetRoleDescPrimitiveItemsById(int roleDescId)
            {
            return ListForPrimitiveItems().SingleOrDefault(r => r.RoleDescriptionId == roleDescId);
            }

        public override void Insert(RoleDescription rd)
            {
            rd.CreatedDate = DateTime.Now;
            rd.LastModifiedDate = DateTime.Now;
            if(string.IsNullOrWhiteSpace(rd.CreatedBy))
                {
                rd.CreatedBy = SessionService.GetCurrentUser().UserName;
                }
            if(string.IsNullOrWhiteSpace(rd.LastModifiedBy))
                {
                rd.LastModifiedBy = SessionService.GetCurrentUser().UserName;
                }

            if(ValidateEntity(rd).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(rd) };
                }

            base.Insert(rd);
            }

        public override void Update(RoleDescription entity, bool refresh = true)
            {

            if(ValidateEntity(entity).Count > 0)
                {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Update(entity, refresh);
            }

        public IQueryable<RoleDescription> FilterRoleDescriptions(IQueryable<RoleDescription> roleDescriptions, SearchArg searchArg)
            {
            var searchWord = searchArg.Search.ToLower();
            var filteredRoleDescription = roleDescriptions.Where(ent =>
                    (!string.IsNullOrEmpty(ent.RolePositionDescription.Title) && ent.RolePositionDescription.Title.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RolePositionDescription.DocNumber) && ent.RolePositionDescription.DocNumber.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.RolePositionDescription.DateOfApproval.ToString()) && ent.RolePositionDescription.DateOfApproval.ToString().ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.LastModifiedBy.ToString()) && ent.LastModifiedBy.ToString().ToLower().Contains(searchWord))
);

            return filteredRoleDescription.OrderBy(e => e.RoleDescriptionId);
            }



        public void CreateRoleDescription(RoleDescription roleDescription, RolePositionDescription rolepositionDesc)
            {
            var guid = UnitOfWork.StratTransction();

            try
                {
                var rolePosRep = ServiceRepository.RolePositionDescriptionRepository();
                rolepositionDesc.RolePositionDescId = rolePosRep.GetDbNewId("RolePositionDescription");
                rolepositionDesc.IsPositionDescription = false;
                    rolepositionDesc.Version = 1;
                rolePosRep.Insert(rolepositionDesc);

                //RolePositionDescription
                roleDescription.RoleDescriptionId = rolepositionDesc.RolePositionDescId;
                roleDescription.RolePositionDescription = null;
                var itemSrv = ServiceRepository.GlobalItemRepository();
                var itemsList = itemSrv.List().ToArray();

                var agencyOverview = itemSrv.GetGlobalItemByCode(itemsList, Enums.GlobalItem.AgencyOverview);
                roleDescription.Agency = itemSrv.GetGlobalItemByCode(itemsList, Enums.GlobalItem.Agency).ItemContent;
                roleDescription.AgencyOverview = agencyOverview.ItemContent;
                roleDescription.Cluster = itemSrv.GetGlobalItemByCode(itemsList, Enums.GlobalItem.Cluster).ItemContent;
                roleDescription.AgencyWebsite = itemSrv.GetGlobalItemByCode(itemsList, Enums.GlobalItem.AgencyWebsite).ItemContent;
                

                roleDescription.EssentialRequirements =
                 itemSrv.GetGlobalItemByCode(itemsList, Enums.GlobalItem.GeneralEssentialRequirement).ItemContent;
                roleDescription.RoleCapabilityItems =
                    itemSrv.GetGlobalItemByCode(itemsList, Enums.GlobalItem.CapabilitiesForTheRole).ItemContent;
                roleDescription.CapabilitySummary =
                itemSrv.GetGlobalItemByCode(itemsList, Enums.GlobalItem.CapabilitySummary).ItemContent;
                roleDescription.FocusCapabilities =
                itemSrv.GetGlobalItemByCode(itemsList, Enums.GlobalItem.FocusCapabilitiesForTheRole).ItemContent;


                this.Insert(roleDescription);
                UnitOfWork.CommitTransaction(guid);

                }

            catch(Exception ex) //error occurred
                {
                UnitOfWork.RollbackTransaction(guid);
                throw new Exception(ex.Message);
                //Handel error
                }

            }

        public IQueryable<RoleDescription> FilterDisplayedRoleDescriptions(JQueryDataTableRolePositionDesc arg, IQueryable<RoleDescription> roleDescriptions)
            {
            var displayedRoleDescription = roleDescriptions;
            SearchArg searchArgs;
            if(arg.StatusCode != null || arg.GradeCode != null)
                {


                if(arg.StatusCode != null)
                    {
                    var intArr = arg.StatusCode.ToParamList().CastToIntegerList();
                    if(intArr.Any())
                        {
                        displayedRoleDescription =
                            displayedRoleDescription.Where(
                                ent => intArr.Contains(ent.RolePositionDescription.StatusId));
                        }
                    }
                if(arg.GradeCode != null)
                    {
                    var gradeList = arg.GradeCode.ToParamList();
                    if(gradeList.Any())
                        {
                        displayedRoleDescription =
                            displayedRoleDescription.Where(
                                ent => gradeList.Contains(ent.RolePositionDescription.GradeCode));
                        }
                    }

                if(!string.IsNullOrWhiteSpace(arg.sSearch))
                    {
                    searchArgs = new SearchArg { Search = arg.sSearch, };
                    displayedRoleDescription = this.FilterRoleDescriptions(displayedRoleDescription, searchArgs);
                    }

                displayedRoleDescription = CustomOrderBy.CustomSort(displayedRoleDescription, arg);
                }
            else if(!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                searchArgs = new SearchArg { Search = arg.sSearch };
                displayedRoleDescription = this.FilterRoleDescriptions(displayedRoleDescription, searchArgs);
                }
            else
                {
                displayedRoleDescription = CustomOrderBy.CustomSort(displayedRoleDescription, arg);
                }
            return displayedRoleDescription;
            }

        public RoleDescription GetEmptyModel()
            {
            var model = new RoleDescription()
                {
                RoleDescriptionId = 0,
                RolePositionDescription = new RolePositionDescription
                    {
                    RolePositionDescId = 0,
                    StatusValue = new StatusValue
                        {
                        StatusId = (int)Enums.StatusValue.Draft,
                        StatusName = Enums.StatusValue.Draft.ToString()

                        },
                    IsPositionDescription = false,
                    },
                DecisionMaking = ServiceRepository.GlobalItemRepository().GetGlobalItemByCode(Enums.GlobalItem.DecisionMaking).ItemContent
                };
            return model;
            }

       
        [Unity.Attributes.Dependency]
        public RolePositionDescriptionRepository RolePositionDescriptionRepository { get; set; }

        }
    }



