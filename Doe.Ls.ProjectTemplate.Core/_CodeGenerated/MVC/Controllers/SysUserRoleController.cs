  



using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Web.Controllers.Domain;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    
        
    public partial class SysUserRoleController : AppControllerBase
    {
     private SysUserRoleRepository _repository=null;
     public SysUserRoleRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.SysUserRoleRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var sysUserRoles = Repository.List();
            return View(sysUserRoles.ToList());
        }

        //
        // GET: /SysUserRole/Details/5
        public ActionResult Details(string userId = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var sysUserRole = Repository.GetEntityByKey(userId);
            if (sysUserRole == null)
            {
                throw new HttpException("sysUserRole not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", sysUserRole);
              } else {
            return View(sysUserRole);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var sysUserRoles = Repository.List();
                IQueryable<SysUserRole> displayedSysUserRoles;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedSysUserRoles = Repository.FilterSysUserRoles(sysUserRoles, searchArgs);
                }
                else
                {
                    displayedSysUserRoles = CustomOrderBy.CustomSort(sysUserRoles, arg);
                }

                var totalRecord =  displayedSysUserRoles.Count();
                var totalDisplayRecord =  displayedSysUserRoles.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedSysUserRoles =  displayedSysUserRoles.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedSysUserRoles.AsEnumerable().ToArray().Select(ent => ent.To(new SysUserRoleLight()));

                dataTableResult.sEcho = arg.sEcho;
                dataTableResult.iTotalRecords = totalRecord;
                dataTableResult.iTotalDisplayRecords = totalDisplayRecord;
                dataTableResult.aaData = result;
                dataTableResult.Status= Status.Success;
                dataTableResult.Message = "Success";               
            } 
            catch (Exception exception) 
            {
                LogException(exception);

                dataTableResult.Status = Status.Error;
                dataTableResult.Message = "Errors";
                dataTableResult.AddError(new DbValidationError("DB error",
                    "Oops! something wrong happened " + exception.Message));
            }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
        }

         // GET: /SysUserRole/Details/5
        public ActionResult DetailsJson(string userId = "")
        {
            var ajaxResult = new Result();
            
            try {
                var sysUserRoleLight = Repository.GetEntityByKey(userId).To(new SysUserRoleLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = sysUserRoleLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            } catch (Exception exception) {
                LogException(exception);
                ajaxResult.Status = Status.Error;
                ajaxResult.Message = "Errors";
                ajaxResult.AddError(new DbValidationError("DB error",
                    "Oops! something wrong happened " + exception.Message));
                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            }
            
        }



        // GET: /SysUserRole/Delete/5
        public ActionResult Delete(string userId = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var sysUserRole = Repository.GetEntityByKey(userId);
            if (sysUserRole == null)
            {
                throw new HttpException("sysUserRole not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", sysUserRole);
              } else {
            return View(sysUserRole);
              }

            
        }
        //
        // GET: /SysUserRole/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new SysUserRole());
            } else {
            return View(new SysUserRole());
            }
        }

        //
        // POST: /SysUserRole/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SysUserRole sysUserRole)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(sysUserRole);
                } catch (Exception exception) {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    errors.Add(new DbValidationError("DB error",  "Oops! something wrong happened "+exception.Message));

                    if ((Request.IsAjaxRequest())) {
                        ajaxResult.Status = Status.Error;
                        ajaxResult.Message = "Errors";
                        ajaxResult.AddErrors(errors);
                        return Json(ajaxResult);
                    } else {
                        
                        ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                    }
                   
                }
                CreateLookups();
                if (Request.IsAjaxRequest()) {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";
                    return Json(ajaxResult);
                } 
                return RedirectToAction("Index");               
             
  }
            else
            {
                if (Request.IsAjaxRequest())
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
                    return View(sysUserRole);
                }
                
            }
            
        }

              
        //
        // GET: /SysUserRole/Edit/5
        public ActionResult Edit(string  userId = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var sysUserRole = Repository.GetEntityByKey(userId);
            if (sysUserRole == null)
            {
                throw new HttpException("sysUserRole not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", sysUserRole);
              } else {
                  return View(sysUserRole);
              }
        }

        //
        // POST: /SysUserRole/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SysUserRole sysUserRole)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldSysUserRole = Repository.GetEntityByEntityKey(sysUserRole);
                
                Repository.SetPropertyValuesFrom(ref oldSysUserRole,sysUserRole);

                try
                {
                    Repository.Update(oldSysUserRole);     
                } 
                catch (Exception exception) 
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    errors.Add(new DbValidationError("DB error",  "Oops! something wrong happened "+exception.Message));

                    if ((Request.IsAjaxRequest())) 
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

                if (Request.IsAjaxRequest()) 
                {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";

                    return Json(ajaxResult);
                } 
                return RedirectToAction("Index");
            }
            else
            {
                if (Request.IsAjaxRequest())
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
                    return View(sysUserRole);
                }                
            }          
        }

        //
        // POST: /SysUserRole/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(SysUserRole sysUserRole)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldSysUserRole = Repository.GetEntityByEntityKey(sysUserRole);
            try
            {
                Repository.Delete(oldSysUserRole);     
            }
            catch (Exception exception)
            {
                LogException(exception);
            
                var errors = Repository.GetBackendValidationErrors().ToList();
                errors.Add(new DbValidationError("DB error",  "Oops! something wrong happened "+exception.Message));

                if ((Request.IsAjaxRequest())) 
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
        
            if (Request.IsAjaxRequest()) 
            {
                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
            
                return Json(ajaxResult);
            } 
        
            return RedirectToAction("Index");
        }
        
        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("SysUserRole", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var sysUserItems=ServiceRepository.SysUserRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.UserId.ToString(), Text =pe.FirstName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("sysUserItems",sysUserItems,ViewData);
                

          var sysRoleItems=ServiceRepository.SysRoleRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.RoleId.ToString(), Text =pe.RoleApiName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("sysRoleItems",sysRoleItems,ViewData);
                

          var orgLevelItems=ServiceRepository.OrgLevelRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.OrgLevelId.ToString(), Text =pe.OrgLevelName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("orgLevelItems",orgLevelItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}