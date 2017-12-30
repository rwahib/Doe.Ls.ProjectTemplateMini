  



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
    
        
    public partial class AppEntityTypeController : AppControllerBase
    {
     private AppEntityTypeRepository _repository=null;
     public AppEntityTypeRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.AppEntityTypeRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var appEntityTypes = Repository.List();
            return View(appEntityTypes.ToList());
        }

        //
        // GET: /AppEntityType/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var appEntityType = Repository.GetEntityByKey(id);
            if (appEntityType == null)
            {
                throw new HttpException("appEntityType not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", appEntityType);
              } else {
            return View(appEntityType);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var appEntityTypes = Repository.List();
                IQueryable<AppEntityType> displayedAppEntityTypes;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedAppEntityTypes = Repository.FilterAppEntityTypes(appEntityTypes, searchArgs);
                }
                else
                {
                    displayedAppEntityTypes = CustomOrderBy.CustomSort(appEntityTypes, arg);
                }

                var totalRecord =  displayedAppEntityTypes.Count();
                var totalDisplayRecord =  displayedAppEntityTypes.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedAppEntityTypes =  displayedAppEntityTypes.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedAppEntityTypes.AsEnumerable().ToArray().Select(ent => ent.To(new AppEntityTypeLight()));

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

         // GET: /AppEntityType/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var appEntityTypeLight = Repository.GetEntityByKey(id).To(new AppEntityTypeLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = appEntityTypeLight;

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



        // GET: /AppEntityType/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var appEntityType = Repository.GetEntityByKey(id);
            if (appEntityType == null)
            {
                throw new HttpException("appEntityType not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", appEntityType);
              } else {
            return View(appEntityType);
              }

            
        }
        //
        // GET: /AppEntityType/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new AppEntityType());
            } else {
            return View(new AppEntityType());
            }
        }

        //
        // POST: /AppEntityType/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppEntityType appEntityType)
        {
            this.ModelState.Remove("SysAdminDashboard"); 
appEntityType.SysAdminDashboard = Request["SysAdminDashboard"].IsOn();this.ModelState.Remove("PowerUserDashboard"); 
appEntityType.PowerUserDashboard = Request["PowerUserDashboard"].IsOn();this.ModelState.Remove("HighPriority"); 
appEntityType.HighPriority = Request["HighPriority"].IsOn();
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(appEntityType);
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
                    return View(appEntityType);
                }
                
            }
            
        }

              
        //
        // GET: /AppEntityType/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var appEntityType = Repository.GetEntityByKey(id);
            if (appEntityType == null)
            {
                throw new HttpException("appEntityType not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", appEntityType);
              } else {
                  return View(appEntityType);
              }
        }

        //
        // POST: /AppEntityType/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppEntityType appEntityType)
        {
            this.ModelState.Remove("SysAdminDashboard"); 
appEntityType.SysAdminDashboard = Request["SysAdminDashboard"].IsOn();this.ModelState.Remove("PowerUserDashboard"); 
appEntityType.PowerUserDashboard = Request["PowerUserDashboard"].IsOn();this.ModelState.Remove("HighPriority"); 
appEntityType.HighPriority = Request["HighPriority"].IsOn();
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldAppEntityType = Repository.GetEntityByEntityKey(appEntityType);
                
                Repository.SetPropertyValuesFrom(ref oldAppEntityType,appEntityType);

                try
                {
                    Repository.Update(oldAppEntityType);     
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
                    return View(appEntityType);
                }                
            }          
        }

        //
        // POST: /AppEntityType/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(AppEntityType appEntityType)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldAppEntityType = Repository.GetEntityByEntityKey(appEntityType);
            try
            {
                Repository.Delete(oldAppEntityType);     
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
        
        var enity = T4Helper.GetEntityType("AppEntityType", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}