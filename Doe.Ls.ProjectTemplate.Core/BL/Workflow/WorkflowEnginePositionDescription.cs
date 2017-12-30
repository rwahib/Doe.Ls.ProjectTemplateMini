using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Workflow
    {
    public class WorkflowEnginePositionDescription : WorkflowEngineBase
        {
        public WorkflowEnginePositionDescription(PositionDescription workflowObject, UserInfoExtension userInfo, IRepositoryFactory factory) : base(workflowObject, userInfo, factory)
            {
            }

        public WorkflowEnginePositionDescription(PositionDescription workflowObject, IUserTask task) : base(workflowObject, task)
            {
            }

        public override List<WorkflowAction> GetWorkflowObjectActions()
            {
            var positionDescription = WorkflowObject as PositionDescription;

            var list = new List<WorkflowAction>();
            var status = positionDescription.StatusValue.GetEnum();


            switch(status)
                {

                #region case draft
                case Enums.StatusValue.Draft:

                    if(Task.User.HasAnyAdminRoleExceptHr())
                        {
                        if(IsAllPositionsDeleted(positionDescription.RolePositionDescription))
                            {
                            list.Add(WorkflowAction.Delete);
                            }
                        }

                    break;
                #endregion

                #region case submitted
                case Enums.StatusValue.Submitted:
                    break;
                #endregion

                #region case endorse
                case Enums.StatusValue.Endorsed:
                    break;
                #endregion

                #region case approved, Imported

                case Enums.StatusValue.Approved:
                case Enums.StatusValue.Imported:
                    if(Task.User.HasDoERoleOnly() || Task.User.IsGuest)
                        {
                        break;
                        }
                    if(Task.User.HasAnyAdminRoleExceptHr())
                        {
                        list.Add(WorkflowAction.CreateNewVersion);
                        list.Add(WorkflowAction.Clone);


                        }

                    break;
                #endregion

                #region case deleted
                case Enums.StatusValue.Deleted:
                    break;
                    #endregion


                }
            if(status != Enums.StatusValue.Deleted)
                {
                if(this.Task.User.HasAdminOrPowerRole())
                    {
                    list.Add(WorkflowAction.Rename);
                    list.Add(WorkflowAction.BulkBringToDraft);
                    list.Add(WorkflowAction.BulkMarkAsImported);
                    list.Add(WorkflowAction.MovePositions);

                    }
                }

            var uniqueList = list.Distinct().ToList();
            WorkflowAction.Populate(this.Factory, uniqueList);
            return uniqueList;
            }

        public override Result ApplyAction(WorkflowActionModel model, bool relatedWorkflowObjects = false, NameValueCollection formCollection = null)
            {
            var srv = new ServiceRepository(Factory);
            var rep = srv.PositionDescriptionRepository();

            var positionDescription = rep.GetPositionDescriptionById(model.WfObjectId);
            var rpdRep = srv.RolePositionDescriptionRepository();
            var histRep = srv.RolePositionDescriptionHistoryRepository();

            rpdRep.LoadNavigationProperty(positionDescription.RolePositionDescription, rpd => rpd.Positions);

            var positions = positionDescription.RolePositionDescription.Positions.ToArray();

            var oldStatus = (Enums.StatusValue)positionDescription.StatusValue.StatusId;
            var oldTitle = positionDescription.RolePositionDescription.Title;
            var action = new WorkflowAction { ActionId = model.ActionId };

            WorkflowAction.Populate(Factory, action);
            
            switch(model.GetWorkflowAction())
                {

                case Enums.WorkflowActions.BringToDraft:

                    #region position description status
                    switch(oldStatus)
                        {
                        case Enums.StatusValue.Deleted:
                        case Enums.StatusValue.Archived:
                            return new Result
                                {
                                Status = Status.Error,
                                Message =
                                    "Cannot bring to draft due to the old status is Deleted, Archived or inactive"
                                };

                        case Enums.StatusValue.Approved:
                        case Enums.StatusValue.Imported:
                            if(positions.HasAnyIncludeLive(Enums.StatusValue.Submitted, Enums.StatusValue.Endorsed))
                                {
                                return new Result
                                    {
                                    Status = Status.Error,
                                    Message =
                                        "Cannot bring to draft due to the old status is Live, Endorsed or Submitted"
                                    };
                                }
                            else
                                {
                                positionDescription.RolePositionDescription.StatusId = (int)Enums.StatusValue.Draft;
                                }
                            break;
                        case Enums.StatusValue.Draft:
                        case Enums.StatusValue.Endorsed:
                        case Enums.StatusValue.Submitted:

                            return new Result
                                {
                                Status = Status.Error,
                                Message = "Cannor bring to draft because status is not live"
                                };
                        }

                    #endregion

                    break;

                case Enums.WorkflowActions.Submit:

                    #region position description status

                    switch(oldStatus)
                        {
                        case Enums.StatusValue.Deleted:
                        case Enums.StatusValue.Archived:
                            return new Result
                                {
                                Status = Status.Error,
                                Message =
                                    "Cannot submit becaue the status is not active"
                                };

                        case Enums.StatusValue.Draft:
                            positionDescription.RolePositionDescription.StatusId = (int)Enums.StatusValue.Submitted;
                            break;
                        case Enums.StatusValue.Approved:

                        case Enums.StatusValue.Imported:
                        case Enums.StatusValue.Endorsed:
                        case Enums.StatusValue.Submitted:

                            return new Result
                                {
                                Status = Status.Error,
                                Message = "Invalid status"
                                };

                        }

                    #endregion

                    break;

                case Enums.WorkflowActions.Endorse:

                    #region position description status
                    switch(oldStatus)
                        {
                        case Enums.StatusValue.Deleted:
                        case Enums.StatusValue.Archived:
                            return new Result
                                {
                                Status = Status.Error,
                                Message =
                                    "Cannot endorse becaue the status is not active"
                                };

                        case Enums.StatusValue.Submitted:
                        case Enums.StatusValue.Draft:
                            positionDescription.RolePositionDescription.StatusId = (int)Enums.StatusValue.Endorsed;
                            break;

                        case Enums.StatusValue.Approved:
                        case Enums.StatusValue.Imported:
                        case Enums.StatusValue.Endorsed:

                            return new Result
                                {
                                Status = Status.Error,
                                Message = "Cannot endorse becaue invalid status"
                                };
                        }

                    #endregion

                    break;
                case Enums.WorkflowActions.MarkAsImported:

                    #region position description status
                    switch(oldStatus)
                        {
                        case Enums.StatusValue.Deleted:
                        case Enums.StatusValue.Archived:
                            return new Result
                                {
                                Status = Status.Error,
                                Message =
                                    "Cannot endorse becaue the status is not active"
                                };

                        case Enums.StatusValue.Draft:
                        case Enums.StatusValue.Submitted:
                        case Enums.StatusValue.Endorsed:

                            positionDescription.RolePositionDescription.StatusId = (int)Enums.StatusValue.Imported;
                            break;

                        case Enums.StatusValue.Approved:
                        case Enums.StatusValue.Imported:

                            return new Result
                                {
                                Status = Status.Error,
                                Message = "Cannot endorse becaue invalid status"
                                };
                        }

                    #endregion

                    break;
                case Enums.WorkflowActions.Approve:

                    #region position description status
                    switch(oldStatus)
                        {
                        case Enums.StatusValue.Deleted:
                        case Enums.StatusValue.Archived:
                            return new Result
                                {
                                Status = Status.Error,
                                Message =
                                    "Cannot endorse becaue the status is not active"
                                };

                        case Enums.StatusValue.Draft:
                        case Enums.StatusValue.Submitted:
                        case Enums.StatusValue.Endorsed:

                            positionDescription.RolePositionDescription.StatusId = (int)Enums.StatusValue.Approved;
                            break;

                        case Enums.StatusValue.Approved:
                        case Enums.StatusValue.Imported:

                            return new Result
                                {
                                Status = Status.Error,
                                Message = "Cannot endorse becaue invalid status"
                                };
                        }

                    #endregion

                    break;

                case Enums.WorkflowActions.Reject:

                    #region position description status
                    switch(oldStatus)
                        {
                        case Enums.StatusValue.Deleted:
                        case Enums.StatusValue.Archived:
                            return new Result
                                {
                                Status = Status.Error,
                                Message =
                                    "Cannot endorse becaue the status is not active"
                                };

                        case Enums.StatusValue.Submitted:

                            positionDescription.RolePositionDescription.StatusId = (int)Enums.StatusValue.Draft;

                            break;
                        case Enums.StatusValue.Endorsed:
                            if(!model.NextStatus.HasValue)
                                {
                                positionDescription.RolePositionDescription.StatusId =
                                    (int)Enums.StatusValue.Submitted;
                                }
                            else
                                {
                                positionDescription.RolePositionDescription.StatusId =
                                    (int)model.NextStatus;
                                }

                            break;

                        case Enums.StatusValue.Approved:
                        case Enums.StatusValue.Imported:
                        case Enums.StatusValue.Draft:
                            return new Result
                                {
                                Status = Status.Error,
                                Message = "Cannot endorse becaue invalid status"
                                };
                        }

                    #endregion

                    break;

                case Enums.WorkflowActions.CreateNewVersion:

                    break;

                case Enums.WorkflowActions.Clone:

                    break;
                case Enums.WorkflowActions.Delete:

                    #region position description status
                    switch(oldStatus)
                        {
                        case Enums.StatusValue.Deleted:
                        case Enums.StatusValue.Archived:
                        case Enums.StatusValue.Submitted:
                        case Enums.StatusValue.Endorsed:

                            return new Result
                                {
                                Status = Status.Error,
                                Message =
                                    "Cannot delete as it is not Draft"
                                };

                        case Enums.StatusValue.Draft:
                            if(positions.HasAnyIncludeLive(Enums.StatusValue.Draft, Enums.StatusValue.Endorsed, Enums.StatusValue.Submitted))
                                {
                                return new Result
                                    {
                                    Status = Status.Error,
                                    Message =
                                        "Cannot delete due to the attached positions"
                                    };
                                }
                            else
                                {
                                positionDescription.RolePositionDescription.StatusId = (int)Enums.StatusValue.Deleted;
                                }
                            break;
                        }

                    #endregion

                    break;

                case Enums.WorkflowActions.Rename:
                    if(formCollection != null && formCollection["NewTitle"] != null)
                        {
                        
                        var newTitle = formCollection["NewTitle"];
                        rpdRep.UpdateRolePosDescriptionTitleCascade(positionDescription.RolePositionDescription.DocNumber, newTitle);
                        rpdRep.Refresh(positionDescription.RolePositionDescription);
                           
                        SyncTrim(model,true);
                        }

                    break;

                case Enums.WorkflowActions.BulkBringToDraft:
                  
                    rpdRep.BulkBringToDraft(positionDescription.RolePositionDescription.DocNumber);
                        rpdRep.Refresh(positionDescription.RolePositionDescription);
                    

                        SyncTrim(model,true);

                    break;

                case Enums.WorkflowActions.BulkMarkAsImported:
                   
                    rpdRep.BulkMarkAsImported(positionDescription.RolePositionDescription.DocNumber);
                    rpdRep.Refresh(positionDescription.RolePositionDescription);

                        SyncTrim(model,true);


                    break;

                case Enums.WorkflowActions.MovePositions:
                    
                    rpdRep.MovePositions(formCollection);
                    rpdRep.Refresh(positionDescription.RolePositionDescription);
                        SyncTrim(model,true);

                    break;

                }

            positionDescription = rep.GetPositionDescriptionById(model.WfObjectId);
            this.WorkflowObject = positionDescription;


            if(model.GetWorkflowAction() == Enums.WorkflowActions.Rename)
                {
                var hisItem = new RolePositionDescriptionHistory
                    {
                    Action = action.ActionName,
                    RolePositionDescId = model.WfObjectId,
                    StatusFrom = oldStatus.ToString(),
                    StatusTo = positionDescription.RolePositionDescription.StatusValue.GetEnum().ToString(),
                    CreatedBy = Task.User.UserName,
                    CreatedDate = DateTime.Now,
                    AdditionalInfo =
                 $"User comment: {model.Comment}<br/> Additional info: position title updated from {oldTitle} to {positionDescription.RolePositionDescription.Title} by {Task.User.UserName} "
                    };

                histRep.Insert(hisItem);
                return SuccessMessage(positionDescription, action);

                }

            #region exclude list
            var excludeActionList = new List<Enums.WorkflowActions>()
            {
             Enums.WorkflowActions.Rename,Enums.WorkflowActions.Clone,Enums.WorkflowActions.CreateNewVersion
            ,Enums.WorkflowActions.UpdatePositionNumber,Enums.WorkflowActions.BulkBringToDraft,Enums.WorkflowActions.BulkMarkAsImported,Enums.WorkflowActions.MovePositions
            };
            #endregion

            if (!excludeActionList.Contains(model.GetWorkflowAction()) )
            {
                rpdRep.UpdateStatus(positionDescription.PositionDescriptionId,
                    (Enums.StatusValue) positionDescription.RolePositionDescription.StatusId);

                        SyncTrim(model,true);

                }

            srv.RolePositionDescriptionHistoryRepository().Insert(new RolePositionDescriptionHistory
                {
                Action = action.ActionName,
                RolePositionDescId = model.WfObjectId,
                StatusFrom = oldStatus.ToString(),
                StatusTo = positionDescription.RolePositionDescription.StatusValue.GetEnum().ToString(),
                CreatedBy = Task.User.UserName,
                CreatedDate = DateTime.Now,
                AdditionalInfo = $"User comment: {model.Comment}<br/> Additional info: position {positionDescription.RolePositionDescription.DocNumber}-{positionDescription.RolePositionDescription.Title} has been {action.ActionStatus} by {Task.User.UserName} "
                });

           
            return SuccessMessage(positionDescription, action);

            }

       

        public override Enums.Privilege GetWorkflowObjectPrivilege(PrivilegeOptions options = null)
            {
            var priv = Enums.Privilege.ReadOnlyPrivilege;
            if(Task.User.HasDoERoleOnly() || Task.User.IsGuest)
                {
                return priv;
                }
            switch(WorkflowObject.WorkflowObjectStatus.GetEnum())
                {
                case Enums.StatusValue.Deleted:
                    break;

                case Enums.StatusValue.Draft:
                    priv.CanPerformActions = true;
                    priv.CanEdit = true;
                    break;
                case Enums.StatusValue.Submitted:
                    priv.CanPerformActions = true;
                    if(Task.User.HasAdminOrPowerRole() || Task.User.IsBusinessUnitAuthor || Task.User.IsDirectorateEndorser || Task.User.IsDirectorateDataEntry
                        || Task.User.IsDivisionApprover || Task.User.IsDivisionEditor)
                        {
                        priv.CanEdit = true;
                        }
                    break;
                case Enums.StatusValue.Endorsed:
                    priv.CanPerformActions = true;
                    if(Task.User.HasAdminOrPowerRole() || Task.User.IsDirectorateEndorser || Task.User.IsDirectorateDataEntry
                       || Task.User.IsDivisionApprover || Task.User.IsDivisionEditor)
                        {
                        priv.CanEdit = true;
                        }
                    break;
                case Enums.StatusValue.Approved:
                case Enums.StatusValue.Imported:
                    priv.CanPerformActions = true;

                    break;
                }

            return priv;
            }

        private bool IsAllPositionsDeleted(RolePositionDescription rpd)
            {
            LoadPositions(rpd);

            return !rpd.Positions.Any() || rpd.Positions.ToList().All(p => p.StatusId == (int)Enums.StatusValue.Deleted);
            }

        private void LoadPositions(RolePositionDescription rpd)
            {
            var srv = new ServiceRepository(this.Factory);
            srv.RolePositionDescriptionRepository().LoadNavigationProperty(rpd, r => r.Positions);
            }
        }
    }