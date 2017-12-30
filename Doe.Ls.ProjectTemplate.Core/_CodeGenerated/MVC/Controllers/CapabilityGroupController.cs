  



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
    
        
    public partial class CapabilityGroupController : AppControllerBase
    {
     private CapabilityGroupRepository _repository=null;
     public CapabilityGroupRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.CapabilityGroupRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var capabilityGroups = Repository.List();
            return View(capabilityGroups.ToList());
        }

        //
        // GET: /CapabilityGroup/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var capabilityGroup = Repository.GetEntityByKey(id);
            if (capabilityGroup == null)
            {
                throw new HttpException("capabilityGroup not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", capabilityGroup);
              } else {
            return View(capabilityGroup);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var capabilityGroups = Repository.List();
                IQueryable<CapabilityGroup> displayedCapabilityGroups;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedCapabilityGroups = Repository.FilterCapabilityGroups(capabilityGroups, searchArgs);
                }
                else
                {
                    displayedCapabilityGroups = CustomOrderBy.CustomSort(capabilityGroups, arg);
                }

                var totalRecord =  displayedCapabilityGroups.Count();
                var totalDisplayRecord =  displayedCapabilityGroups.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedCapabilityGroups =  displayedCapabilityGroups.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedCapabilityGroups.AsEnumerable().ToArray().Select(ent => ent.To(new CapabilityGroupLight()));

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

         // GET: /CapabilityGroup/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var capabilityGroupLight = Repository.GetEntityByKey(id).To(new CapabilityGroupLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = capabilityGroupLight;

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



        // GET: /CapabilityGroup/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var capabilityGroup = Repository.GetEntityByKey(id);
            if (capabilityGroup == null)
            {
                throw new HttpException("capabilityGroup not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", capabilityGroup);
              } else {
            return View(capabilityGroup);
              }

            
        }
        //
        // GET: /CapabilityGroup/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new CapabilityGroup());
            } else {
            return View(new CapabilityGroup());
            }
        }

        //
        // POST: /CapabilityGroup/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CapabilityGroup capabilityGroup)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(capabilityGroup);
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
                    return View(capabilityGroup);
                }
                
            }
            
        }

              
        //
        // GET: /CapabilityGroup/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var capabilityGroup = Repository.GetEntityByKey(id);
            if (capabilityGroup == null)
            {
                throw new HttpException("capabilityGroup not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", capabilityGroup);
              } else {
                  return View(capabilityGroup);
              }
        }

        //
        // POST: /CapabilityGroup/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CapabilityGroup capabilityGroup)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldCapabilityGroup = Repository.GetEntityByEntityKey(capabilityGroup);
                
                Repository.SetPropertyValuesFrom(ref oldCapabilityGroup,capabilityGroup);

                try
                {
                    Repository.Update(oldCapabilityGroup);     
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
                    return View(capabilityGroup);
                }                
            }          
        }

        //
        // POST: /CapabilityGroup/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(CapabilityGroup capabilityGroup)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldCapabilityGroup = Repository.GetEntityByEntityKey(capabilityGroup);
            try
            {
                Repository.Delete(oldCapabilityGroup);     
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
        
        var enity = T4Helper.GetEntityType("CapabilityGroup", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}