  



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
    
        
    public partial class OccupationTypeController : AppControllerBase
    {
     private OccupationTypeRepository _repository=null;
     public OccupationTypeRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.OccupationTypeRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var occupationTypes = Repository.List();
            return View(occupationTypes.ToList());
        }

        //
        // GET: /OccupationType/Details/5
        public ActionResult Details(string occupationTypeCode = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var occupationType = Repository.GetEntityByKey(occupationTypeCode);
            if (occupationType == null)
            {
                throw new HttpException("occupationType not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", occupationType);
              } else {
            return View(occupationType);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var occupationTypes = Repository.List();
                IQueryable<OccupationType> displayedOccupationTypes;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedOccupationTypes = Repository.FilterOccupationTypes(occupationTypes, searchArgs);
                }
                else
                {
                    displayedOccupationTypes = CustomOrderBy.CustomSort(occupationTypes, arg);
                }

                var totalRecord =  displayedOccupationTypes.Count();
                var totalDisplayRecord =  displayedOccupationTypes.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedOccupationTypes =  displayedOccupationTypes.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedOccupationTypes.AsEnumerable().ToArray().Select(ent => ent.To(new OccupationTypeLight()));

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

         // GET: /OccupationType/Details/5
        public ActionResult DetailsJson(string occupationTypeCode = "")
        {
            var ajaxResult = new Result();
            
            try {
                var occupationTypeLight = Repository.GetEntityByKey(occupationTypeCode).To(new OccupationTypeLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = occupationTypeLight;

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



        // GET: /OccupationType/Delete/5
        public ActionResult Delete(string occupationTypeCode = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var occupationType = Repository.GetEntityByKey(occupationTypeCode);
            if (occupationType == null)
            {
                throw new HttpException("occupationType not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", occupationType);
              } else {
            return View(occupationType);
              }

            
        }
        //
        // GET: /OccupationType/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new OccupationType());
            } else {
            return View(new OccupationType());
            }
        }

        //
        // POST: /OccupationType/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OccupationType occupationType)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(occupationType);
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
                    return View(occupationType);
                }
                
            }
            
        }

              
        //
        // GET: /OccupationType/Edit/5
        public ActionResult Edit(string  occupationTypeCode = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var occupationType = Repository.GetEntityByKey(occupationTypeCode);
            if (occupationType == null)
            {
                throw new HttpException("occupationType not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", occupationType);
              } else {
                  return View(occupationType);
              }
        }

        //
        // POST: /OccupationType/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OccupationType occupationType)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldOccupationType = Repository.GetEntityByEntityKey(occupationType);
                
                Repository.SetPropertyValuesFrom(ref oldOccupationType,occupationType);

                try
                {
                    Repository.Update(oldOccupationType);     
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
                    return View(occupationType);
                }                
            }          
        }

        //
        // POST: /OccupationType/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(OccupationType occupationType)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldOccupationType = Repository.GetEntityByEntityKey(occupationType);
            try
            {
                Repository.Delete(oldOccupationType);     
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
        
        var enity = T4Helper.GetEntityType("OccupationType", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}