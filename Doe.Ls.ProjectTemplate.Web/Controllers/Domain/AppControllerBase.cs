#region

using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web.Mvc;
using System.Web.Security;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Data;

#endregion

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {



    [ValidateInput(false)]
    public abstract class AppControllerBase : Controller
        {
        protected const string AllAdministrators = "SystemAdministrator, IsAdministrator, PowerUser";

        protected ServiceRepository _serviceRepository;

        protected ServiceRepository ServiceRepository => _serviceRepository ?? (_serviceRepository = InitialiseRepository());

        protected ILoginService LoginService => ServiceRepository.LoginService();

        protected ISessionService SessionService => ServiceRepository.SessionService();

        protected ILoggerService LoggerService => ServiceRepository.LoggerService();


        public UserInfoExtension CurrentUser
            {
            get
                {
                var curUser = ServiceRepository.SessionService().GetCurrentUser() as UserInfoExtension;
                if(curUser == null && User.Identity.IsAuthenticated)
                    {
                    curUser=LoginService.GetUserAndCacheIt(User.Identity.Name);
                    }
                else
                if(curUser == null)
                    {
                    curUser = UserInfoExtension.GuestUser;

                    }
                return curUser;
                }
            }

        private ServiceRepository InitialiseRepository()
            {
            var factory = new HttpRepositoryFactory();
            factory.RegisterAllDependencies();
            return new ServiceRepository(factory);
            }

        protected override void OnException(ExceptionContext filterContext)
            {
            if(filterContext.RouteData.Values.ContainsValue("Error"))
                {
                return; // otherwise close loop
                }
            LogException(filterContext.Exception);

            filterContext.ExceptionHandled = true;

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var info = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
            var newController = factory.CreateController(filterContext.RequestContext, "Error") as ErrorController;
            filterContext.RouteData.Values["controller"] = "Error";
            filterContext.Controller = newController;
            if(filterContext.Controller != null)
                {
                filterContext.Controller.ViewData = new ViewDataDictionary(info);
                ViewBagWrapper.UserInfoExtensionBag.SetCurrentUser(CurrentUser, filterContext.Controller.ViewData);
                }


            string actionToCall = "Index";
            if(filterContext.HttpContext.Request.IsAjaxRequest())
                {
                actionToCall = "IndexAjax";
                }


            filterContext.RouteData.Values["action"] = actionToCall;


            if(newController != null)
                {
                IController cnt = newController;
                filterContext.ExceptionHandled = true;
                cnt.Execute(filterContext.RequestContext);
                }
           

            base.OnException(filterContext);
            }

        protected string GetCurrentLoggedUser()
            {
            return HttpContext.User.Identity.Name;
            }

       

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            T4Helper.SetAssemblyClassNameFormatFromContext(ServiceRepository.GetUnitOfWork().DbContext);
            var exclusions = new string[] { "account", "appapi", "error", "public", "Notific" };
            var exclueded = false;
            var controllerName = filterContext.Controller.GetType().Name.ToLower();
            foreach(var exclusion in exclusions)
                {
                if(controllerName.Contains(exclusion))
                    {
                    exclueded = true;
                    break;
                    }
                }
            if(!exclueded)
                if(CurrentUser == null ||Session.IsNewSession)
                {

                FormsAuthentication.SignOut();
                SessionService.Abandon();
                   
               }
            else
                {
                ViewBagWrapper.UserInfoExtensionBag.SetCurrentUser(CurrentUser, ViewData);                
                ViewBagWrapper.DbContextBag.SetDbContext(ServiceRepository.GetUnitOfWork().DbContext, ViewData);
                ViewBagWrapper.GlobalServiceRepository.SetRepository(this.ServiceRepository, ViewData);
                var task = InitialiseTask();
                ViewBagWrapper.TaskBag.SetTask(task, ViewData);
                InitMessagesList();
                }
            

            /*
            if(PositionEstablishmentSettings.Notification.PerformanceMonitorMode)
                {
                this.ServiceRepository.GeneralLogRepository().LogMonitorInfo("Http Request", $"{filterContext.ActionDescriptor.ActionName}-{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}", filterContext.RequestContext.HttpContext.Request.RawUrl);
                }
                */

            }


        protected IUserTask InitialiseTask()
            {
            if(CurrentUser == null)
                {

                FormsAuthentication.SignOut();
                SessionService.Abandon();
                return null;
                }

            var task = UserTaskFactory.GetTask(CurrentUser, ServiceRepository.RepositoryFactory);
            ViewBagWrapper.TaskBag.SetTask(task, ViewData);

            return task;
            }

        protected void ClearModelState()
            {
            ModelState.Remove("LastModifiedDate");
            ModelState.Remove("LastModifiedBy");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("CreatedDate");
            }

        #region Common Protected Methods

        protected string GetBaseUrl()
            {
            return EntityBase.Http.HttpHelper.GetAppUrl();
            }

        protected void LogException(Exception ex)
            {
            LoggerService.SendMail(ex);
            LoggerService.Log(ex);

            }

        protected void LogUserActivity(Enums.LogActions action, string context = "", string note = "")
            {
            try
                {
                this.ServiceRepository.GeneralLogRepository().Log(action, context, note);

                }
            catch(Exception exception)
                {
                LogException(exception);
                }
            }

        protected bool GetDefaultAction(UserInfoExtension userExtension, out ActionResult actionResult)
            {
            actionResult = RedirectToAction("Index", "Home");
            return true;
            }


        protected void InitialiseArgument(JQueryDataTableParamModel arg)
            {
            if(arg.iDisplayStart < 0) arg.iDisplayStart = 0;
            if(arg.iDisplayLength <= 0) arg.iDisplayLength = 10000;
            if(arg.sSearch != null)
                {
                arg.sSearch = arg.sSearch.ToLower().Replace("\"", "").Replace("\'", "");
                }
            if(string.IsNullOrWhiteSpace(arg.SortColumnName))
                {
                arg.SortColumnName = !string.IsNullOrEmpty(arg.sColumns)
                    ? arg.sColumns.Split(',')[arg.iSortCol_0]
                    : string.Empty;
                }

            arg.SortColumnDesc = arg.sSortDir_0 != "asc";            
            }

        protected void SetErrorsInTempData(object errors)
            {
            TempData["errors"] = errors;
            }
        protected void GetErrorsFromTempData()
            {
            if(TempData["errors"] != null)
                {
                var errors = TempData["errors"] as IEnumerable<DbValidationError>;

                ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                }
            }

        #endregion


        #region Implementation of IDisposable

        protected bool _disposed;

        public new void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }
        protected void SetWorkflowEngine(IWorkflowObject position, IUserTask task)
            {
            IWorkflowEngine workflowEngine = null;
            workflowEngine = WorkflowEngineFactory.CreatEngine(position, task);
            ViewBagWrapper.WorkflowBag.SetWorkflowEngine(workflowEngine, ViewData);

            }

        protected IWorkflowEngine SetWorkflowEngine(IWorkflowObject position)
        {
            IWorkflowEngine workflowEngine = null;
            var task = UserTaskFactory.GetTask(this.CurrentUser, this.ServiceRepository.RepositoryFactory);
            if (task != null)
            {

                workflowEngine = WorkflowEngineFactory.CreatEngine(position, task);

                ViewBagWrapper.WorkflowBag.SetWorkflowEngine(workflowEngine, ViewData);
            }
            return workflowEngine;
            }

        protected MessageService InitMessagesList()
        {
            var srv = MessageFactory.GetMessageService(this.ServiceRepository.RepositoryFactory);
            ViewBagWrapper.MessageListBag.SetMessageService(srv,ViewData);
            return srv;
        }

        protected SysMessage GetMessage(string code)
            {
            var srv = ViewBagWrapper.MessageListBag.GetMessageService(ViewData);
            var sysMessage=srv.GetMessageByCode(code) ?? new SysMessage
            {
                MessageFormat = "Error/Warning message missing",
                MessageHint = "Missing message",
                MsgCategoryId = (int) MessageCategory.Error
            };
            return sysMessage;
            }

        protected override void Dispose(bool disposing)
            {
            if(!_disposed)
                {
                if(disposing)
                    {
                    this.ServiceRepository.GetUnitOfWork().Dispose();
                    this.ServiceRepository.GetUnitOfWork().DbContext.Dispose();
                    }
                }
            _disposed = true;
            }

        #endregion
        }
    }