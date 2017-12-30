using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {
    [HasDoeRole]
    public partial class UserController : AppControllerBase
        {
        private SysUserRepository _repository;

        public UserController(ServiceRepository service)
            {
            _serviceRepository = service;
            _repository = service.SysUserRepository();
            }

        public UserController()
            {

            }
        public SysUserRepository Repository
            {
            get
                {

                return _repository = _repository ?? ServiceRepository.SysUserRepository();

                }

            }

        [HasAnyAdminRole]
        public ActionResult Index()
            {
            var sysUsers = Repository.List();
            var users = sysUsers.Take(10).ToArray().Select(su => UserInfoExtension.MapSysUser(su, this.ServiceRepository.RepositoryFactory));
            return View(users);
            }

        //
        public ActionResult Details(string userId = "", int roleId = (int)Enums.UserRole.Guest)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var sysUser = Repository.GetSysUserByUserName(userId);
            if(sysUser == null)
                {
                var msg = MessageHelper.NotFoundMessage("system user");
                throw new HttpException(msg);

                }
            var userInfo = UserInfoExtension.MapSysUser(sysUser, ServiceRepository.RepositoryFactory);
            ViewBagWrapper.InfoBag.SetTitle(userId == CurrentUser.UserName ? "My Profile" : ((Enums.UserRole)roleId).GetDescription() + " Details", ViewData);

            return Request.IsAjaxRequest() ? View("Details-modal", userInfo) : View(userInfo);
            }

        public ActionResult ListJson([FromUri] JQueryDataTableSysUser arg)
            {
            InitialiseArgument(arg);
            InitialiseTask();
            var dataTableResult = new DataTableResult();

            try
                {
                var sysUsers = Repository.List().Where(su => su.Active); ;
                
                var displayedSysUsers= sysUsers;

                    if (!string.IsNullOrWhiteSpace(arg.sSearch))
                    {
                        displayedSysUsers = Repository.FilterSysUsers(sysUsers, arg);                        
                    }
                displayedSysUsers = CustomOrderBy.CustomSort(displayedSysUsers, arg);

                var totalRecord = displayedSysUsers.Count();
                var totalDisplayRecord = displayedSysUsers.Count();

                if(arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedSysUsers = displayedSysUsers.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                    var result =
                        displayedSysUsers.AsEnumerable()
                            .ToArray()
                            .Select(ent =>
                            {
                                var u = UserInfoExtension.MapSysUser(ent, ServiceRepository.RepositoryFactory);
                                return UserInfoExtensionLight.MapFrom(u);

                            });
                    
                dataTableResult.sEcho = arg.sEcho;
                dataTableResult.iTotalRecords = totalRecord;
                dataTableResult.iTotalDisplayRecords = totalDisplayRecord;
                dataTableResult.aaData = result;
                dataTableResult.Status = Status.Success;
                dataTableResult.Message = "Success";
                }
            catch(Exception exception)
                {
                LogException(exception);

                dataTableResult.Status = Status.Error;
                dataTableResult.Message = "Errors";
                dataTableResult.AddError(new DbValidationError("Database updates ",
                    "Oops! something went wrong  " + exception.Message));
                }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
            }

        // GET: /SysUser/Details/5
        public ActionResult DetailsJson(string userId = "")
            {
            var ajaxResult = new Result();

            try
                {
                var sysUserLight = Repository.GetEntityByKey(userId).To(new SysUserLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = sysUserLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
                }
            catch(Exception exception)
                {
                LogException(exception);
                ajaxResult.Status = Status.Error;
                ajaxResult.Message = "Errors";
                ajaxResult.AddError(new DbValidationError("Database updates ",
                    "Oops! something went wrong  " + exception.Message));
                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
                }

            }

        
        private void CreateLookups()
            {

            }


        public ActionResult MyProfile()
            {
            if(!CurrentUser.IsGuest)
                {
                ViewBagWrapper.InfoBag.SetTitle("My profile", ViewData);
                return Request.IsAjaxRequest() ? View("MyProfile-modal", CurrentUser) : View(CurrentUser);
                }

            return RedirectToAction("Details", new { userId = CurrentUser.UserName });
            }

        [System.Web.Http.Authorize()]
        public ActionResult Dashboard()
            {
            var task = UserTaskFactory.GetTask(this.CurrentUser, this.Repository.RepositoryFactory);

            return View(CurrentUser);


            }
        }
    }