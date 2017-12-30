using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;


namespace Doe.Ls.ProjectTemplate.Core.BL.Workflow {
    public abstract class WorkflowEngineBase : IWorkflowEngine {
        protected IRepositoryFactory Factory { get; }

        private MessageService _messageService;
        protected MessageService MessageService => _messageService ?? (_messageService = MessageFactory.GetMessageService(Factory));

        private TrimRecordRepository _trimRecordRepository;
        protected TrimRecordRepository TrimRecordRepository => _trimRecordRepository ?? (_trimRecordRepository = new ServiceRepository(Factory).TrimRecordRepository());

        private bool? _trimEnabled; // for unit test
        public bool TrimEnabled {
            get {
                if(_trimEnabled == null) {
                    _trimEnabled = Settings.ProjectTemplateSettings.Site.TrimIntegration;

                    }
                return _trimEnabled.Value;
                }
            set { _trimEnabled = value; }
            }

        protected WorkflowEngineBase(IWorkflowObject workflowObject, UserInfoExtension userInfo, IRepositoryFactory factory) {
            ValidateType(workflowObject);
            Factory = factory;
            WorkflowObject = workflowObject;
            PositionRdPdTasks = PosRdPdFactory.Create(workflowObject);
            Task = UserTaskFactory.GetTask(userInfo, Factory);
            }

        protected WorkflowEngineBase(IWorkflowObject workflowObject, IUserTask task) {
            Factory = task.RepositoryFactory;
            WorkflowObject = workflowObject;
            PositionRdPdTasks = PosRdPdFactory.Create(workflowObject);
            Task = task;
            }

        public IWorkflowObject WorkflowObject { get; protected set; }

        public T GetWorkflowObject<T>() where T : class, IWorkflowObject {
            return WorkflowObject as T;
            }

        public IPositionRdPdTasks PositionRdPdTasks { get; }
        public IUserTask Task { get; }
        public abstract List<WorkflowAction> GetWorkflowObjectActions();

        public abstract Result ApplyAction(WorkflowActionModel model, bool relatedWorkflowObjects = false,
            NameValueCollection formCollection = null);
        public abstract Enums.Privilege GetWorkflowObjectPrivilege(PrivilegeOptions options = null);

        protected Result SuccessMessage(IWorkflowObject workflowObject, WorkflowAction action) {

            var actionStatus = workflowObject.WorkflowObjectStatus.StatusName;
            if(action.ActionId == (int)Enums.WorkflowActions.Rename) {
                return new Result {
                    Status = Status.Success,
                    Message = string.Format(MessageService.GetMessageByCode("WF-ACTION-RENAME-PD-RD-ACTION-CONFIRM-CONTENT").MessageFormat, action.ActionName, workflowObject.WorkflowObjectTitle, actionStatus),

                    HeaderText = string.Format(MessageService.GetMessageByCode("WF-ACTION-RENAME-PD-RD-ACTION-CONFIRM-HEADER").MessageFormat, workflowObject.WorkflowObjectType.ToString().Wordify()),

                    };
                }
            if(action.ActionId == (int)Enums.WorkflowActions.UpdatePositionNumber) {
                return new Result {
                    Status = Status.Success,
                    Message = string.Format(MessageService.GetMessageByCode("WF-ACTION-RENAME-POS-NO-ACTION-CONFIRM-CONTENT").MessageFormat, action.ActionName, workflowObject.WorkflowObjectTitle, actionStatus),

                    HeaderText = string.Format(MessageService.GetMessageByCode("WF-ACTION-RENAME-POS-NO-ACTION-CONFIRM-HEADER").MessageFormat, workflowObject.WorkflowObjectType.ToString().Wordify()),

                    };
                }


            return new Result {
                Status = Status.Success,
                Message = string.Format(MessageService.GetMessageByCode("WF-ACTION-CONFIRM-CONTENT").MessageFormat, action.ActionName, workflowObject.WorkflowObjectTitle, actionStatus),

                HeaderText = string.Format(MessageService.GetMessageByCode("WF-ACTION-CONFIRM-HEADER").MessageFormat, workflowObject.WorkflowObjectType.ToString().Wordify()),

                };

            }

        protected void SyncTrim(WorkflowActionModel model, bool force = false) {
            if(!TrimEnabled) return;

            var service = new ServiceRepository(Factory);
            RefreshWorkflowObject(service);
            try {
                TrimRecordRepository.SynchRolePosDescription(WorkflowObject.WorkflowObjectId, model.Comment, force);

                var recordInfoModel = TrimRecordRepository.GetRecordInfoModel(WorkflowObject.WorkflowObjectId);
                service.RolePositionDescriptionHistoryRepository().Insert(new RolePositionDescriptionHistory {
                    Action = Enums.WorkflowActions.SyncPdOrRdAttachmentToTrim.ToString().Wordify(),
                    RolePositionDescId = model.WfObjectId,
                    StatusFrom = WorkflowObject.WorkflowObjectStatus.ToString(),
                    StatusTo = model.NextStatus?.ToString() ?? WorkflowObject.WorkflowObjectStatus.ToString(),
                    CreatedBy = Task.User.UserName,
                    CreatedDate = DateTime.Now,
                    AdditionalInfo = $"'HP RM record' is successfully Synced - for 'HP RM record' {recordInfoModel}"
                    });
                }
            catch(Exception exception) {
                service.RolePositionDescriptionHistoryRepository().Insert(new RolePositionDescriptionHistory {
                    Action = Enums.WorkflowActions.SyncPdOrRdAttachmentToTrim.ToString().Wordify(),
                    RolePositionDescId = model.WfObjectId,
                    StatusFrom = WorkflowObject.WorkflowObjectStatus.ToString(),
                    StatusTo = model.NextStatus?.ToString() ?? WorkflowObject.WorkflowObjectStatus.ToString(),
                    CreatedBy = Task.User.UserName,
                    CreatedDate = DateTime.Now,
                    AdditionalInfo = $"'HP RM record' is failed to be synced :<br/>Error - {exception.Message} "
                    });
                service.LoggerService().SendMail(exception);
                service.LoggerService().Log(exception);
                }
            }

        protected void ExpireTrim(WorkflowActionModel model) {
            if(!TrimEnabled) return;

            var service = new ServiceRepository(Factory);
            RefreshWorkflowObject(service);

            try {
                TrimRecordRepository.ExpireToken(WorkflowObject.WorkflowObjectId);
                }
            catch(Exception exception) {
                service.RolePositionDescriptionHistoryRepository().Insert(new RolePositionDescriptionHistory {
                    Action = Enums.WorkflowActions.SyncPdOrRdAttachmentToTrim.ToString().Wordify(),
                    RolePositionDescId = model.WfObjectId,
                    StatusFrom = WorkflowObject.WorkflowObjectStatus.ToString(),
                    StatusTo = model.NextStatus?.ToString() ?? WorkflowObject.WorkflowObjectStatus.ToString(),
                    CreatedBy = Task.User.UserName,
                    CreatedDate = DateTime.Now,
                    AdditionalInfo = $"'HP RM record' is failed to Expire record : Error - {exception}"
                    });
                }
            }

        private void RefreshWorkflowObject(ServiceRepository service) {
            if(WorkflowObject is PositionDescription) {
                WorkflowObject =
                    service.PositionDescriptionRepository()
                        .GetPositionDescriptionById(WorkflowObject.WorkflowObjectId);
                }
            if(WorkflowObject is RoleDescription) {
                WorkflowObject =
                    service.RoleDescriptionRepository()
                        .GetRoleDescriptionById(WorkflowObject.WorkflowObjectId);
                }
            }

        private void ValidateType(object workflowObject) {
            if(workflowObject is Position || workflowObject is PositionDescription || workflowObject is RoleDescription) return;

            throw new InvalidOperationException($"{workflowObject.GetType().Name} is not recognised as valid type");

            }
        }
    }