using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Workflow
    {
    public class WorkflowEnginePosition : WorkflowEngineBase
        {
        public WorkflowEnginePosition(IWorkflowObject workflowObject, UserInfoExtension userInfo, IRepositoryFactory factory) : base(workflowObject, userInfo, factory)
            {
            }

        public WorkflowEnginePosition(IWorkflowObject workflowObject, IUserTask task) : base(workflowObject, task)
            {
            }
        public override Enums.Privilege GetWorkflowObjectPrivilege(PrivilegeOptions options = null)
            {
            var priv = Enums.Privilege.ReadOnlyPrivilege;
            if(WorkflowObject.WorkflowObjectType == WorkflowObjectType.Position)
                {
                var position = WorkflowObject as Position;
                if(WorkflowObject.IsDeleted())
                    {
                    priv = Enums.Privilege.AccessDeniedPrivilege;
                    priv.CanRead = true;
                    return priv;
                    }


                if(options != null && options.GetDownloadPrivilege)
                    {
                    if(WorkflowObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Approved ||
                        WorkflowObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Imported)
                        {
                        priv.CanDownload = true;
                        }
                    else
                        {

                        if(!WorkflowObject.IsDeleted() && PosRdPdFactory.Create(WorkflowObject).IsValid())
                            {
                            priv.CanDownload = true;
                            }
                        }
                    }
                if(Task.User.HasAdminOrPowerRole())
                    {

                    if(!WorkflowObject.IsLive()) { priv.CanEdit = true; }
                    priv.CanRead = true;

                    priv.CanPerformActions = true;

                    return priv;
                    }

                var actions = GetWorkflowObjectActions();
                if(actions.Any()) priv.CanPerformActions = true;
                switch(position.StatusValue.GetEnum())
                    {
                    case Enums.StatusValue.Approved:
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionApprover, Task.User))
                            {
                            priv.CanRead = true;
                            }
                        break;
                    case Enums.StatusValue.Imported:
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionApprover, Task.User))
                            {
                            priv.CanRead = true;
                            }
                        break;
                    case Enums.StatusValue.Draft:
                        if(position.UnitId==Enums.Cnt.Na||
                        position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionApprover, Task.User)
                            ||
                            position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionEditor, Task.User)
                        ||
                            position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DirectorateEndorser, Task.User)
                            ||
                            position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DirectorateDataEntry, Task.User)
                            ||
                            position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.BusinessUnitDataEntry, Task.User)
                            ||
                            position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.BusinessUnitAuthor, Task.User)
                            )
                            {
                            priv.CanEdit = true;
                            priv.CanDelete= true;
                            priv.CanPerformActions = true;

                            }
                            if (Task.User.HasDoERoleOnly())
                            {
                                priv.CanRead = false;
                            }
                            
                        break;
                    case Enums.StatusValue.Submitted:
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionApprover, Task.User)
                            || position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DirectorateEndorser, Task.User)
                            )
                            {
                            priv.CanEdit = true;
                            priv.CanRead = true;
                            }
                        if(Task.User.HasDoERoleOnly())
                            {
                            priv.CanRead = false;
                            }
                        break;
                    case Enums.StatusValue.Endorsed:
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionApprover, Task.User))
                            {
                            priv.CanEdit = true;
                            priv.CanRead = true;
                            }
                        break;

                    case Enums.StatusValue.Deleted:
                        if(Task.User.HasAdminOrPowerRole())
                            {
                            priv = Enums.Privilege.AccessDeniedPrivilege;
                            priv.CanRead = true;
                            }
                        if(Task.User.HasDoERoleOnly())
                            {
                            priv.CanRead = false;
                            }
                        break;
                    }
                }

            return priv;
            }
        public override List<WorkflowAction> GetWorkflowObjectActions()
            {
            var position = WorkflowObject as Position;
            if(position==null)throw new InvalidCastException("Workflow object is not of the type Position");

            var list = new List<WorkflowAction>();
            var status = position.StatusValue.GetEnum();


            switch(status)
                {

                #region case draft
                case Enums.StatusValue.Draft:
                    var srv=new ServiceRepository(this.Factory);       
                    srv.PositionRepository().LoadNavigationProperty(position,po=>po.Positions);

                            if (Task.User.HasAdminOrPowerRole())
                            {
                        if(this.PositionRdPdTasks.IsValid()) { list.Add(WorkflowAction.MarkAsImported);}
                               if (position.Positions.All(p => p.StatusId == (int) Enums.StatusValue.Deleted)) // if positions=0 All will return true
                                {
                                    list.Add(WorkflowAction.Delete);
                                }
                            }

                            if (Task.User.IsBusinessUnitAuthor)
                                if (
                                    position.HasTheSameOrganisationalStructureForThisRole(
                                        Enums.UserRole.BusinessUnitAuthor, Task.User))
                                {

                            if(this.PositionRdPdTasks.IsValid()) { list.Add(WorkflowAction.Submit);}

                            if(position.Positions.All(p => p.StatusId == (int)Enums.StatusValue.Deleted)) // if positions=0 All will return true
                                {
                                list.Add(WorkflowAction.Delete);
                                }
                            }

                            if (Task.User.IsBusinessUnitDataEntry)
                                if (
                                    position.HasTheSameOrganisationalStructureForThisRole(
                                        Enums.UserRole.BusinessUnitDataEntry, Task.User))
                                {
                            if(position.Positions.All(p => p.StatusId == (int)Enums.StatusValue.Deleted)) // if positions=0 All will return true
                                {
                                list.Add(WorkflowAction.Delete);
                                }

                            }

                            if (Task.User.IsDirectorateEndorser)
                                if (
                                    position.HasTheSameOrganisationalStructureForThisRole(
                                        Enums.UserRole.DirectorateEndorser, Task.User))
                                {
                            if(this.PositionRdPdTasks.IsValid()) { list.Add(WorkflowAction.Endorse);}
                            if(position.Positions.All(p => p.StatusId == (int)Enums.StatusValue.Deleted)) // if positions=0 All will return true
                                {
                                list.Add(WorkflowAction.Delete);
                                }
                            }
                            if (Task.User.IsDirectorateDataEntry)
                                if (
                                    position.HasTheSameOrganisationalStructureForThisRole(
                                        Enums.UserRole.DirectorateDataEntry, Task.User))
                                {
                            if(this.PositionRdPdTasks.IsValid()) { list.Add(WorkflowAction.Submit);}
                            if(position.Positions.All(p => p.StatusId == (int)Enums.StatusValue.Deleted)) // if positions=0 All will return true
                                {
                                list.Add(WorkflowAction.Delete);
                                }
                            }
                            if (Task.User.IsDivisionApprover)
                                if (
                                    position.HasTheSameOrganisationalStructureForThisRole(
                                        Enums.UserRole.DivisionApprover, Task.User))
                                {
                            if(this.PositionRdPdTasks.IsValid()) { list.Add(WorkflowAction.Approve);}
                            if(!position.Positions.Any())
                                {
                                list.Add(WorkflowAction.Delete);
                                }
                            }
                            if (Task.User.IsDivisionEditor)
                                if (position.HasTheSameOrganisationalStructureForThisRole(
                                    Enums.UserRole.DivisionEditor, Task.User))
                                {
                            if(this.PositionRdPdTasks.IsValid()) { list.Add(WorkflowAction.Endorse);}
                            if(position.Positions.All(p => p.StatusId == (int)Enums.StatusValue.Deleted)) // if positions=0 All will return true
                                {
                                list.Add(WorkflowAction.Delete);
                                }
                            }
                        
                        
                    break;
                #endregion

                #region case submitted
                case Enums.StatusValue.Submitted:
                    if(Task.User.HasAdminOrPowerRole())
                        {
                        if(this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Approve);
                        if (this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Reject);
                        }

                    if(Task.User.IsDirectorateEndorser)
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DirectorateEndorser, Task.User))
                            {
                            if (this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Endorse);
                            if (this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Reject);
                            }

                    if(Task.User.IsDivisionApprover)
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionApprover, Task.User))
                            {
                            if (this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Approve);
                            if (this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Reject);
                            }

                    break;
                #endregion

                #region case endorse
                case Enums.StatusValue.Endorsed:
                    if(Task.User.HasAdminOrPowerRole())
                        {
                        if (this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Approve);
                        if (this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Reject);
                        }

                    if(Task.User.IsDivisionApprover)
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionApprover, Task.User))
                            {
                            if (this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Approve);
                            if (this.PositionRdPdTasks.IsValid()) list.Add(WorkflowAction.Reject);
                            }

                    break;
                #endregion

                #region case approved, Imported

                case Enums.StatusValue.Approved:
                case Enums.StatusValue.Imported:
                        if (Task.User.HasDoERoleOnly()|| Task.User.IsGuest)
                        {
                            break;
                        }
                    if(Task.User.HasAdminOrPowerRole())
                        {
                        list.Add(WorkflowAction.BringToDraft);

                        list.Add(WorkflowAction.UpdatePositionNumber);
                        
                        list.Add(WorkflowAction.Clone);
                        }

                    if(Task.User.IsBusinessUnitAuthor || Task.User.IsBusinessUnitDataEntry)
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.BusinessUnitAuthor, Task.User)
                            || position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.BusinessUnitDataEntry, Task.User)
                            )

                            {
                            
                            list.Add(WorkflowAction.Clone);
                            }


                    if(Task.User.IsDirectorateDataEntry || Task.User.IsDirectorateEndorser)
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DirectorateEndorser, Task.User)
                                || position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DirectorateDataEntry, Task.User)
                                )
                            {
                            
                            list.Add(WorkflowAction.Clone);
                            }


                    if(Task.User.IsDivisionApprover || Task.User.IsDivisionEditor)
                        if(position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionEditor, Task.User)
                                || position.HasTheSameOrganisationalStructureForThisRole(Enums.UserRole.DivisionApprover, Task.User)
                                )
                            {
                            list.Add(WorkflowAction.BringToDraft);
                            
                            list.Add(WorkflowAction.Clone);
                            }

                    break;
                #endregion

                #region case deleted
                case Enums.StatusValue.Deleted:
                    break;
                    #endregion


                }

            var uniqueList = list.Distinct().ToList();
            WorkflowAction.Populate(this.Factory, uniqueList);
            return uniqueList;
            }
        
        public override Result ApplyAction(WorkflowActionModel model, bool relatedWorkflowObjects = false, NameValueCollection formCollection = null)
            {
            var srv = new ServiceRepository(Factory);
            var posRep = srv.PositionRepository();

            var position = posRep.GetPositionById(model.WfObjectId);
            var oldStatus = (Enums.StatusValue)position.StatusId;
            var action = new WorkflowAction { ActionId = model.ActionId };
            WorkflowAction.Populate(Factory, action);
            switch(model.GetWorkflowAction())
                {

                case Enums.WorkflowActions.BringToDraft:
                case Enums.WorkflowActions.Submit:
                case Enums.WorkflowActions.Endorse:

                case Enums.WorkflowActions.MarkAsImported:
                case Enums.WorkflowActions.Approve:                                                                    
                case Enums.WorkflowActions.Archive:   
                                     
                case Enums.WorkflowActions.Delete:

                    position.StatusId = (int)action.GetStatus();
                    
                    break;                
                case Enums.WorkflowActions.CreateNewVersion:
                    break;
                case Enums.WorkflowActions.Clone:
                    break;
                case Enums.WorkflowActions.Reject:
                    switch(position.StatusValue.GetEnum())
                        {

                        case Enums.StatusValue.Submitted:
                            position.StatusId = (int)Enums.StatusValue.Draft;

                          
                            break;
                        case Enums.StatusValue.Endorsed:
                            var nextStatus = model.NextStatus ?? Enums.StatusValue.Submitted;
                            position.StatusId = (int)nextStatus;                          
                            break;

                        }

                    break;

                    case Enums.WorkflowActions.UpdatePositionNumber:
                        if (Task.User.HasAdminOrPowerRole())
                        {
                            var newPosNumber = formCollection["NewPositionNumber"].Trim();
                        if(string.IsNullOrWhiteSpace(newPosNumber))throw new FormatException(MessageService.GetMessageByCode("WF-ACTION-INVALID-POS-NO").MessageFormat);
                        if(posRep.Exists(newPosNumber)) throw new FormatException(MessageService.GetMessageByCode("WF-ACTION-POS-NO-EXISTS").MessageFormat);

                        position.PositionNumber = newPosNumber;
                        }

                    break;
                }
            posRep.CreateOrUpdatePosition(position);

            position = posRep.GetPositionById(model.WfObjectId);
            this.WorkflowObject = position;
            posRep.PositionHistoryRepository.Insert(new PositionHistory
                {
                Action = action.ActionName,
                PositionId = model.WfObjectId,
                StatusFrom = oldStatus.ToString(),
                StatusTo = position.StatusValue.GetEnum().ToString(),
                CreatedBy = Task.User.UserName,
                CreatedDate = DateTime.Now,
                AdditionalInfo = $"User comment: {model.Comment}<br/> Additional info: position {position.PositionNumber}-{position.PositionTitle} has been {action.ActionStatus} by {Task.User.UserName} "
                });

            var validForRelated = new List<Enums.WorkflowActions>
            {
                Enums.WorkflowActions.Endorse,Enums.WorkflowActions.Approve,Enums.WorkflowActions.BringToDraft,Enums.WorkflowActions.MarkAsImported,Enums.WorkflowActions.Submit,Enums.WorkflowActions.Reject

            };

            if(relatedWorkflowObjects&& validForRelated.Contains(model.GetWorkflowAction()))
            {
                IWorkflowObject relatedWorkflowObject = null;
                WorkflowActionModel relatedModel=new WorkflowActionModel();
                model.To(relatedModel);
                if (position.RolePositionDescription.IsPositionDescription)
                {
                    relatedWorkflowObject = srv.PositionDescriptionRepository().LoadPositionDescById(position.RolePositionDescriptionId);
                    relatedModel.ObjectType=WorkflowObjectType.PositionDescription;
                    relatedModel.WfObjectId = relatedWorkflowObject.WorkflowObjectId;
                }
                else
                {
                    relatedWorkflowObject = srv.RoleDescriptionRepository().GetRoleDescriptionById(position.RolePositionDescriptionId);
                    relatedModel.ObjectType = WorkflowObjectType.RoleDescription;
                    relatedModel.WfObjectId = relatedWorkflowObject.WorkflowObjectId;
                    }

                var relatedWfEngine = WorkflowEngineFactory.CreatEngine(relatedWorkflowObject, this.Task);

                relatedWfEngine.ApplyAction(relatedModel);
            }

            
            return SuccessMessage(position, action);
        }
        }
    }