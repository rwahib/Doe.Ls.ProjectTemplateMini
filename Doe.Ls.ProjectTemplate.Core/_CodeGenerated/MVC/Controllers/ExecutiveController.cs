  



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
 [ValidateInput(false)]    
         
     public partial class ExecutiveController : AppControllerBase
     {
      private ExecutiveRepository _repository=null;
      public ExecutiveRepository Repository
          {
          get
          {
 
              return _repository=_repository ?? ServiceRepository.ExecutiveRepository();
 
              }
  
          }
                 
 
         public ActionResult Index()
         {
             var executives = Repository.List();
             return View(executives.ToList());
         }
 
         //
         // GET: /Executive/Details/5
         public ActionResult Details(string executiveCod = "")
         {
             ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
             var executive = Repository.GetEntityByKey(executiveCod);
             if (executive == null)
             {
                 throw new HttpException("executive not found");
             }
              if (Request.IsAjaxRequest()) {
                   return View("Details-modal", executive);
               } else {
             return View(executive);
               }
 
             
         }
 
          public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
          {
             InitialiseArgument(arg);
             var dataTableResult = new DataTableResult();
          
             try
             {
                 var executives = Repository.List();
                 IQueryable<Executive> displayedExecutives;
             
                 if (!string.IsNullOrWhiteSpace(arg.sSearch))
                 {
                     var searchArgs = new SearchArg { Search = arg.sSearch };
                     displayedExecutives = Repository.FilterExecutives(executives, searchArgs);
                 }
                 else
                 {
                     displayedExecutives = CustomOrderBy.CustomSort(executives, arg);
                 }
 
                 var totalRecord =  displayedExecutives.Count();
                 var totalDisplayRecord =  displayedExecutives.Count();
 
                 if (arg.iDisplayLength == -1)
                     arg.iDisplayLength = totalRecord;
 
                 displayedExecutives =  displayedExecutives.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);
 
                 var result = displayedExecutives.AsEnumerable().ToArray().Select(ent => ent.To(new ExecutiveLight()));
 
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
 
          // GET: /Executive/Details/5
         public ActionResult DetailsJson(string executiveCod = "")
         {
             var ajaxResult = new Result();
             
             try {
                 var executiveLight = Repository.GetEntityByKey(executiveCod).To(new ExecutiveLight());
                 
 
                 ajaxResult.Status = Status.Success;
                 ajaxResult.Message = "Success";
                 ajaxResult.Data = executiveLight;
 
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
 
 
 
         // GET: /Executive/Delete/5
         public ActionResult Delete(string executiveCod = "")
         {
             ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
 
             var executive = Repository.GetEntityByKey(executiveCod);
             if (executive == null)
             {
                 throw new HttpException("executive not found");
             }
              if (Request.IsAjaxRequest()) {
                   return View("Delete-modal", executive);
               } else {
             return View(executive);
               }
 
             
         }
         //
         // GET: /Executive/Create
         public ActionResult Create()
         {
             ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
             ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
             CreateLookups();
             if (Request.IsAjaxRequest()) {
                 return View("Create-modal", new Executive());
             } else {
             return View(new Executive());
             }
         }
 
         //
         // POST: /Executive/Create
 
         [System.Web.Mvc.HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create(Executive executive)
         {
             
             
             ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
             var ajaxResult = new Result();
             var errors = Repository.GetValidationErrors(ModelState).ToList();
             if (ModelState.IsValid) {
                 try {
                     Repository.Insert(executive);
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
                     return View(executive);
                 }
                 
             }
             
         }
 
               
         //
         // GET: /Executive/Edit/5
         public ActionResult Edit(string  executiveCod = "")
         {
             ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
             var executive = Repository.GetEntityByKey(executiveCod);
             if (executive == null)
             {
                 throw new HttpException("executive not found");
             }
             CreateLookups();
             
               if (Request.IsAjaxRequest()) {
                   return View("Edit-modal", executive);
               } else {
                   return View(executive);
               }
         }
 
         //
         // POST: /Executive/Edit/5
         [System.Web.Mvc.HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Edit(Executive executive)
         {
             
             ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
             var ajaxResult = new Result();
             var errors = Repository.GetValidationErrors(ModelState).ToList();
             
             if (ModelState.IsValid)
             {
                 var oldExecutive = Repository.GetEntityByEntityKey(executive);
                 
                 Repository.SetPropertyValuesFrom(ref oldExecutive,executive);
 
                 try
                 {
                     Repository.Update(oldExecutive);     
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
                     return View(executive);
                 }                
             }          
         }
 
         //
         // POST: /Executive/Delete/5
 
         [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
         [ValidateAntiForgeryToken]
         public ActionResult DeleteConfirmed(Executive executive)
         {
             ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
             var ajaxResult = new Result();            
             var oldExecutive = Repository.GetEntityByEntityKey(executive);
             try
             {
                 Repository.Delete(oldExecutive);     
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
         
         var enity = T4Helper.GetEntityType("Executive", this.ServiceRepository.GetUnitOfWork().DbContext);
         ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
               
         var statusValueItems=ServiceRepository.StatusValueRepository()
                     .List().ToArray()
                     .Select(pe => new SelectListItemExtension { Value = pe.StatusId.ToString(), Text =pe.StatusName})
                     .ToArray();
         
         ViewBagWrapper.ListBag.SetList("statusValueItems",statusValueItems,ViewData);
                 
 
           }
 
                
         
         protected override void Dispose(bool disposing)
         {
 
             base.Dispose(disposing);
         }
     }
 }