using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.Dashboards;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {
    public class UserRoleController : AppControllerBase
        {
        private SysUserRoleRepository _repository = null;
        public SysUserRoleRepository Repository
            {
            get
                {
                return _repository = _repository ?? ServiceRepository.SysUserRoleRepository();
                }

            }

        public UserRoleController(ServiceRepository service)
            {
            _serviceRepository = service;
            _repository = service.SysUserRoleRepository();
            }
        public UserRoleController()
            {

            }

        [HasAdminOrPowerRole]
        public ActionResult ListSystemAdministratorUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.SystemAdministrator, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.SystemAdministrator);

            return View("Index", sysUsers.ToList());
            }
        [HasAnyAdminRole]
        public ActionResult ListPowerUserUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.PowerUser, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.PowerUser);

            return View("Index", sysUsers.ToList());
            }
        [HasAnyAdminRole]
        public ActionResult ListAdministratorUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.Administrator, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.Administrator);

            return View("Index", sysUsers.ToList());
            }

        [HasAnyAdminRole]        
        public ActionResult ListHRDataEntryUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.HRDataEntry, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.HRDataEntry);

            return View("Index", sysUsers.ToList());
            }

        [HasAnyAdminRole]
        public ActionResult ListDivisionApproverUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.DivisionApprover, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.DivisionApprover);

            return View("Index", sysUsers.ToList());
            }

        [HasAnyAdminRole]
        public ActionResult ListDivisionEditorUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.DivisionEditor, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.DivisionEditor);

            return View("Index", sysUsers.ToList());
            }
        [HasAnyAdminRole]
        public ActionResult ListDirectorateEndorserUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.DirectorateEndorser, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.DirectorateEndorser);

            return View("Index", sysUsers.ToList());
            }
        [HasAnyAdminRole]
        public ActionResult ListDirectorateDataEntryUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.DirectorateDataEntry, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.DirectorateDataEntry);

            return View("Index", sysUsers.ToList());
            }
        [HasAnyAdminRole]
        public ActionResult ListBusinessUnitAuthorUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.BusinessUnitAuthor, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.BusinessUnitAuthor);

            return View("Index", sysUsers.ToList());
            }
        [HasAnyAdminRole]
        public ActionResult ListBusinessUnitDataEntryUsers()
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Search, ViewData);
            ViewBagWrapper.SetGeneralObject("RoleId", (int)Enums.UserRole.BusinessUnitDataEntry, ViewData);

            var sysUsers = Repository.GetSysUserRoleList(Enums.UserRole.BusinessUnitDataEntry);

            return View("Index", sysUsers.ToList());
            }
        
        [HasAnyAdminRole]
        public ActionResult Create(int roleId)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var role = (Enums.UserRole)roleId;

            var model = new UserRoleModel
                {
                RoleId = roleId,
                ActiveFrom = DateTime.Now
                };
            model.SetDefaults(ServiceRepository);

            CreateLookups();
            if(Request.IsAjaxRequest())
                {
                return View("Create-modal", model);
                }
            else
                {
                return View("Create", model);
                }
            }
        [System.Web.Mvc.HttpPost]
        [HasAnyAdminRole]
        public ActionResult Create(UserRoleModel userRoleModel)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);

            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if(ModelState.IsValid)
                {
                try
                    {
                    var sysUser = ServiceRepository.SysUserRepository().GetSysUserByEmailAndSaveItToDb(userRoleModel.Email);
                    userRoleModel.SetDefaults(sysUser, ServiceRepository);

                        var orgLvl = Enums.OrgLevel.NA;
                        var structureId = "-1";
                        if (userRoleModel != null)
                        {
                            orgLvl = userRoleModel.OrgLevel;
                            structureId = userRoleModel.StructureId;
                        }

                        var userRole = Repository.GetSysUserRoleById(sysUser.UserId, userRoleModel.UserRole, orgLvl, structureId);
                    if(userRole != null)
                        {
                        throw new InvalidOperationException($"User {userRole.SysUser.DisplayedName()}: {userRole.SysUser.Email} is already {userRoleModel.UserRole.GetDescription()}");
                        }
                    userRoleModel.SetDefaults(sysUser, ServiceRepository);
                    Repository.Grant(userRoleModel);
                    Repository.LogGrantSecurityActivity(userRoleModel,CurrentUser.UserName,CurrentUser.CurrentRoleEnum,$" {userRoleModel.OrgLevelName}-{userRoleModel.StructureId}-{userRoleModel.OrgObjetcName}");
                    }
                catch(Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    errors.Add(new DbValidationError("Error", exception.Message));

                    if((Request.IsAjaxRequest()))
                        {
                        ajaxResult.Status = Status.Error;
                        ajaxResult.Message = "Errors";
                        ajaxResult.AddErrors(errors);
                        return Json(ajaxResult);
                        }
                    else
                        {

                        ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                        }

                    }
                CreateLookups();
                if(Request.IsAjaxRequest())
                    {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";
                    return Json(ajaxResult);
                    }
                return RedirectToAction(GetListActionName(userRoleModel.UserRole));

                }
            else
                {
                if(Request.IsAjaxRequest())
                    {
                    ajaxResult.Status = Status.Error;
                    ajaxResult.Message = "Errors";
                    ajaxResult.AddErrors(errors);
                    return Json(ajaxResult);
                    }
                else
                    {
                    ViewBagWrapper.ErrorBag.SetErrors(Repository.GetValidationErrors(ModelState), ViewData);
                    CreateLookups();
                    return View("Create", userRoleModel);
                    }

                }

            }

        public ActionResult Edit(string userName, int roleId, int orgLevelId, string structureId = "-1")
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var userRoleModel = Repository.GetSysUserRoleById(userName, roleId, orgLevelId, structureId).ToUserOrgLevelObject(ServiceRepository);

            if(userRoleModel == null)
                {
                var msg = MessageHelper.NotFoundMessage($"user role {userName}");
                throw new HttpException(msg);
                }
            
            if(Request.IsAjaxRequest())
                {
                return View("Edit-modal", userRoleModel);
                }
            else
                {
                return View("Edit", userRoleModel);
                }

            }
        [System.Web.Mvc.HttpPost]
        [HasAnyAdminRole]
        public ActionResult Edit(UserRoleModel userRoleModel)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            userRoleModel.SetDefaults(ServiceRepository);

            if(ModelState.IsValid)
                {
                try
                    {
                    var sysUser = Repository.GetSysUserRoleByModel(userRoleModel);
                        if (sysUser == null)
                        {

                        Repository.UpdateWithNewStructure(userRoleModel);

                        }
                        else
                        {

                            sysUser.UpdateSignature(this.Repository.RepositoryFactory);
                            sysUser.ActiveFrom = userRoleModel.ActiveFrom;
                            sysUser.ActiveTo = userRoleModel.ActiveTo;
                            sysUser.Note = userRoleModel.Note;

                            Repository.Update(sysUser);
                        }
                        Repository.LogUpdateSecurityActivity(userRoleModel, CurrentUser.UserName, CurrentUser.CurrentRoleEnum, $" {userRoleModel.OrgLevelName}-{userRoleModel.StructureId}-{userRoleModel.OrgObjetcName}");

                    }
                catch(Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    errors.Add(new DbValidationError("Error", exception.Message));

                    if((Request.IsAjaxRequest()))
                        {
                        ajaxResult.Status = Status.Error;
                        ajaxResult.Message = "Errors";
                        ajaxResult.AddErrors(errors);
                        return Json(ajaxResult);
                        }
                    else
                        {

                        ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                        }

                    }
                CreateLookups();
                if(Request.IsAjaxRequest())
                    {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";
                    return Json(ajaxResult);
                    }
                return RedirectToAction("Edit", new { UserName = userRoleModel.UserId, userRoleModel.RoleId, userRoleModel.OrgLevelId, userRoleModel.StructureId });

                }
            else
                {
                if(Request.IsAjaxRequest())
                    {
                    ajaxResult.Status = Status.Error;
                    ajaxResult.Message = "Errors";
                    ajaxResult.AddErrors(errors);
                    return Json(ajaxResult);
                    }
                else
                    {
                    ViewBagWrapper.ErrorBag.SetErrors(Repository.GetValidationErrors(ModelState), ViewData);
                    CreateLookups();
                    return View("Edit", userRoleModel);
                    }

                }

            }
        [HasAnyAdminRole]

        public ActionResult Details(string userName, int roleId, int orgLevelId, string structureId = "-1")
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var userRoleModel = Repository.GetSysUserRoleById(userName, roleId, orgLevelId, structureId).ToUserOrgLevelObject(ServiceRepository);
            if(userRoleModel == null)
                {
                throw new HttpException($"The user role {userName} is not found.");
                }

            if(Request.IsAjaxRequest())
                {
                return View("Details-modal", userRoleModel);
                }
            else
                {
                return View(userRoleModel);
                }

            }

        [HasAnyAdminRole]
        public ActionResult Delete(string userName, int roleId, int orgLevelId, string structureId = "-1")
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);


            var userRoleModel = Repository.GetSysUserRoleById(userName, roleId, orgLevelId, structureId).ToUserOrgLevelObject(ServiceRepository);
            if(userRoleModel == null)
                {
                throw new HttpException($"The user role {userName} is not found.");
                }
            if(Request.IsAjaxRequest())
                {
                return View("Delete-modal", userRoleModel);
                }
            else
                {
                return View(userRoleModel);
                }


            }

        [HasAnyAdminRole]
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [HasAnyAdminRole]
        public ActionResult DeleteConfirmed(UserRoleModel userRoleModel)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();
            userRoleModel.SetDefaults(ServiceRepository);
            var syUserRole = Repository.GetSysUserRoleById(userRoleModel.UserId, userRoleModel.RoleId,
                userRoleModel.OrgLevelId, userRoleModel.StructureId);

            try
                {
                Repository.Delete(syUserRole);
                Repository.LogDeleteSecurityActivity(userRoleModel, CurrentUser.UserName, CurrentUser.CurrentRoleEnum, $" {userRoleModel.OrgLevelName}-{userRoleModel.StructureId}-{userRoleModel.OrgObjetcName}");
                }
            catch(Exception exception)
                {
                LogException(exception);

                var errors = Repository.GetBackendValidationErrors().ToList();

                errors.Add(new DbValidationError("", string.Format(GetMessage("ERR-GENERAL").MessageFormat, exception.Message)));
                errors.Add(new DbValidationError("", GetMessage("ERR-GENERAL-DELETE-HAS-CHILDS").MessageFormat));

                if((Request.IsAjaxRequest()))
                    {
                    ajaxResult.Status = Status.Error;
                    ajaxResult.Message = "Errors";
                    ajaxResult.AddErrors(errors);

                    return Json(ajaxResult);
                    }
                else
                    {
                    ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                    }

                }

            CreateLookups();

            if(Request.IsAjaxRequest())
                {
                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";

                return Json(ajaxResult);
                }

            return RedirectToAction(GetListActionName(userRoleModel.RoleId));
            }

        public static string GetListActionName(int roleId)
            {
            var role = (Enums.UserRole)roleId;
            return DashboardItem.GetDashboardRoleItemLink(role);
            
            }
        public static string GetListActionName(Enums.UserRole role)
            {
            return GetListActionName((int)role);
            }
        
        /// <summary>
        /// TODO implement the method
        /// </summary>
        private void CreateLookups()
            {           
            }
        }
    }