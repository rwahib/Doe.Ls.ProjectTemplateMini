using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class RolePositionDescriptionRepository : BaseRepository<RolePositionDescription> 
    {
        public RolePositionDescriptionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public IQueryable<RolePositionDescription> BaseList()
        {
            var na = Enums.Cnt.Na;
            return base.List().
                Include(rp=>rp.StatusValue).
                Where(ent => ent.RolePositionDescId != na);

        }

        public IQueryable<RolePositionDescription> BaseListAll()
        {
            return base.List();

        }
        public RolePositionDescription GetPositionDescriptionById(int rolePositionDescId)
        {
            var rolePositionDesc = List()
             .FirstOrDefault(l => l.RolePositionDescId == rolePositionDescId);
            if(rolePositionDesc == null)
                {
                throw new InvalidOperationException("Invalid Role/Position description");
                }
            return rolePositionDesc;
        }

        public IQueryable<RolePositionDescription> BaseListWithGrade()
        {
           return BaseList().Include(ent => ent.Grade);

        }
        public override IQueryable<RolePositionDescription> List()
        {
            
            return BaseListWithGrade()                    
                    .Include(ent=>ent.PositionDescription) 
                    .Include(ent=>ent.RoleDescription)                    
                    .OrderBy(ent=>ent.RolePositionDescId);
        }

       
        public RolePositionDescription GetRolePositionDescById(int rolePositionDescId)
        {
            return List().FirstOrDefault(r => r.RolePositionDescId == rolePositionDescId);
        }
        public IQueryable<RolePositionDescription> GetRolePositionDescByDocumentNumber(string docNumber) {
            return List().Where(r => r.DocNumber == docNumber);
        }
        public IQueryable<RolePositionDescription> ListHasRdOrPd()
        {
            return this.BaseList().Where(ent => ent.IsPositionDescription&&ent.PositionDescription != null || !ent.IsPositionDescription &&ent.RoleDescription != null);
        }
        public IQueryable<RolePositionDescription> ListForPositionDescriptions()
        {
            return BaseListWithGrade()
                .Include(ent => ent.PositionDescription)
                .Include(ent => ent.PositionDescription.PositionFocusCriterias.Select(rc => rc.LookupFocusGradeCriteria.SelectionCriteria))
                .Include(ent => ent.PositionDescription.PositionFocusCriterias.Select(rc => rc.LookupFocusGradeCriteria.Focus))
                //.Include("PositionDescription.PositionFocusCriterias.LookupFocusGradeCriteria.SelectionCriteria")
                //.Include("PositionDescription.PositionFocusCriterias.LookupFocusGradeCriteria.Focus")
                .Where(ent=>ent.IsPositionDescription)
                .OrderBy(ent => ent.RolePositionDescId);
        }
        public IQueryable<RolePositionDescription> FilterForLiveRolePositionDescriptions(IQueryable<RolePositionDescription> list)
        {
            var listLiveStatus = new [] {(int)Enums.StatusValue.Approved, (int)Enums.StatusValue.Imported};


            return list.Where(ent =>listLiveStatus.Contains(ent.StatusId));

        }

        public IQueryable<RolePositionDescription> ListForRoleDescriptions()
        {
            return BaseListWithGrade()                    
                .Include(ent => ent.RoleDescription)
                .Include(ent => ent.RoleDescription.RoleCapabilities)
                .Include(ent => ent.RoleDescription.RoleCapabilities.Select(rc =>rc.CapabilityName))
                .Include(ent => ent.RoleDescription.RoleCapabilities.Select(rc => rc.CapabilityLevel))
                .Include(ent => ent.RoleDescription.RoleCapabilities.Select(rc => rc.CapabilityName.CapabilityGroup))
                .Include(ent => ent.RoleDescription.RoleCapabilities.Select(rc => rc.CapabilityName.CapabilityBehaviourIndicators))                
                .Include(ent => ent.RoleDescription.KeyRelationships)
                .Where(ent => ent.IsPositionDescription == false)
                .OrderBy(ent => ent.RolePositionDescId);
        }
       
        public override void Insert(RolePositionDescription entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.LastModifiedDate = DateTime.Now;
            if (string.IsNullOrWhiteSpace(entity.CreatedBy))
            {
                entity.CreatedBy = SessionService.GetCurrentUser().UserName;
            }
            if (string.IsNullOrWhiteSpace(entity.LastModifiedBy))
            {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
            }
            entity.RolePositionDescId = this.GetDbNewId("RolePositionDescription");
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(RolePositionDescription entity, bool refresh = true) 
        {
            if (SessionService.GetCurrentUser() != null)
            {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
            }
            entity.LastModifiedDate = DateTime.Now;
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        public bool Exists(string docNumber)
            {
            return this.List().Any(rpd => rpd.DocNumber == docNumber);

            }
        

        public IQueryable<RolePositionDescription> FilterRolePositionDescriptions(IQueryable<RolePositionDescription> rolePositionDescriptions, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredRolePositionDescription = rolePositionDescriptions.Where(ent => 
                    ent.RolePositionDescId.ToString().Contains(searchWord)
                    || ent.Version.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.Title) && ent.Title.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.DocNumber) && ent.DocNumber.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Grade.GradeTitle) && ent.Grade.GradeTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredRolePositionDescription.OrderBy(e => e.RolePositionDescId);
        }

        public void UpdateStatus(int id, Enums.StatusValue status)
        {
            var roleposdesc = this.List().FirstOrDefault(l => l.RolePositionDescId == id);
           
            roleposdesc.StatusId = (int)status;

            if (roleposdesc.StatusId == (int) Enums.StatusValue.Approved || roleposdesc.StatusId == (int)Enums.StatusValue.Imported) {
                roleposdesc.DateOfApproval = DateTime.Now;
            }

            this.Update(roleposdesc);
        }


        public RolePositionDescription CreateRolePositionDescription(RolePositionDescription rolePos)
        {
            rolePos.Version = 1;
            rolePos.StatusId = (int) Enums.StatusValue.Draft;
            var teachingBased = GradeRepository.IsTeachingBased(rolePos.GradeCode);
            if (teachingBased && !rolePos.IsPositionDescription)
            {
                var msg = MessageHelper.SelectPublicServiceGrade();
                throw new InvalidOperationException(msg);
                //throw new InvalidOperationException("There is a mismatch in the selection, selected grade is not teaching based");
            }
            if (!teachingBased && rolePos.IsPositionDescription)
            {
                var msg = MessageHelper.SelectTeachingServiceGrade();
                throw new InvalidOperationException(msg);
                //throw new InvalidOperationException("There is a mismatch in the selection, selected grade is teaching based");
            }
            if (rolePos.IsPositionDescription)
            {
                var positionDesc = new PositionDescription();
                
                rolePos.PositionDescription = positionDesc;
            }
            else
            {
                var roleDesc = new RoleDescription();
               
                rolePos.RoleDescription = roleDesc;
            }
            this.Insert(rolePos);
            return rolePos;
        }


        public bool IsPositionDesc(int rolePositionDescId)
        {
            var rolePositionDesc = List().SingleOrDefault(rp => rp.RolePositionDescId == rolePositionDescId);

            if (rolePositionDesc != null)
            {
                return rolePositionDesc.IsPositionDescription;
            }

            return false;
        }
        public bool HasDocNumber(string docNum, int versionNum)
            {
            var rpd = BaseList().SingleOrDefault(r => r.DocNumber == docNum && r.Version == versionNum);
            if(rpd != null)
                {
                return true;
                }
            return false;
            }
        public bool HasDocNumber(string docNum)
            {
           return BaseList().Any(r => r.DocNumber == docNum);
           
            }
        public bool HasOtherDocumentNumber(string docNum, int rolePositionDescriptionId)
            {
            return BaseList().Any(r => r.DocNumber == docNum && r.RolePositionDescId != rolePositionDescriptionId);

            }

        public void UpdateRolePosDescriptionTitleCascade(string docNumber, string newTitle, bool newDbInstance = false)
            {
            if(!newDbInstance)
                {
                var ctx = (this.UnitOfWork.DbContext as SampleProjectTemplateEntities);
                ctx.UpdateRolePosDescriptionTitleCascade(docNumber,newTitle);

                return;
                }

                var newUnitOfWork = this.RepositoryFactory.GetService<IUnitOfWork>("new-instance");
                using (var ctx = newUnitOfWork.DbContext as SampleProjectTemplateEntities)
                {
                ctx.UpdateRolePosDescriptionTitleCascade(docNumber,
                    newTitle);

            }
        }

        public void BulkBringToDraft(string docNumber, bool newDbInstance = false)
        {
            {
                if (newDbInstance)
                {
                    var ctx = (this.UnitOfWork.DbContext as SampleProjectTemplateEntities);
                    ctx.BulkBringToDraftPositions(docNumber);

                    return;
                }


                using (var ctx = new SampleProjectTemplateEntities())
                {
                    ctx.BulkBringToDraftPositions(docNumber);
                }
            }
        }

        public void BulkMarkAsImported(string docNumber, bool newDbInstance = false)
            {
               
                if(!newDbInstance)
                    {
                    var ctx = (this.UnitOfWork.DbContext as SampleProjectTemplateEntities);
                    ctx.BulkMarkAsImportPositions(docNumber);

                    return;
                    }
                
            var newUnitOfWork = this.RepositoryFactory.GetService<IUnitOfWork>("new-instance");
                using (var ctx = newUnitOfWork.DbContext as SampleProjectTemplateEntities)
                {
                ctx.BulkMarkAsImportPositions(docNumber);
                    }
               
            }


        [Unity.Attributes.Dependency]
        public GradeRepository GradeRepository { get; set; }


        public void MovePositions(NameValueCollection formCollection)
        {
            var model = RpdPositionsModel.ParsModel(formCollection);
            MovePositions(model);
        }
        public void MovePositions(RpdPositionsModel model)
            {
            var targetRdp = this.GetRolePositionDescById(model.RolePositionDescId);
            var repo = new ServiceRepository(this.RepositoryFactory);
            var posRepo = repo.PositionRepository();

            foreach(var id in model.PositionIds)
                {
                var position = posRepo.GetPositionById(id);
                position.PositionTitle = targetRdp.Title;
                position.RolePositionDescriptionId = targetRdp.RolePositionDescId;
                posRepo.Update(position);

                }


            }



        }
}



