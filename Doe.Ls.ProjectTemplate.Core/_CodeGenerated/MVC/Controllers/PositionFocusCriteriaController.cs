  



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
    
        
    public partial class PositionFocusCriteriaController : AppControllerBase
    {
     private PositionFocusCriteriaRepository _repository=null;
     public PositionFocusCriteriaRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.PositionFocusCriteriaRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var positionFocusCriterias = Repository.List();
            return View(positionFocusCriterias.ToList());
        }

        //
        // GET: /PositionFocusCriteria/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var positionFocusCriteria = Repository.GetEntityByKey(id);
            if (positionFocusCriteria == null)
            {
                throw new HttpException("positionFocusCriteria not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", positionFocusCriteria);
              } else {
            return View(positionFocusCriteria);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var positionFocusCriterias = Repository.List();
                IQueryable<PositionFocusCriteria> displayedPositionFocusCriterias;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedPositionFocusCriterias = Repository.FilterPositionFocusCriterias(positionFocusCriterias, searchArgs);
                }
                else
                {
                    displayedPositionFocusCriterias = CustomOrderBy.CustomSort(positionFocusCriterias, arg);
                }

                var totalRecord =  displayedPositionFocusCriterias.Count();
                var totalDisplayRecord =  displayedPositionFocusCriterias.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedPositionFocusCriterias =  displayedPositionFocusCriterias.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedPositionFocusCriterias.AsEnumerable().ToArray().Select(ent => ent.To(new PositionFocusCriteriaLight()));

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

         // GET: /PositionFocusCriteria/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var positionFocusCriteriaLight = Repository.GetEntityByKey(id).To(new PositionFocusCriteriaLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = positionFocusCriteriaLight;

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



        // GET: /PositionFocusCriteria/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var positionFocusCriteria = Repository.GetEntityByKey(id);
            if (positionFocusCriteria == null)
            {
                throw new HttpException("positionFocusCriteria not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", positionFocusCriteria);
              } else {
            return View(positionFocusCriteria);
              }

            
        }
        //
        // GET: /PositionFocusCriteria/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new PositionFocusCriteria());
            } else {
            return View(new PositionFocusCriteria());
            }
        }

        //
        // POST: /PositionFocusCriteria/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PositionFocusCriteria positionFocusCriteria)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(positionFocusCriteria);
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
                    return View(positionFocusCriteria);
                }
                
            }
            
        }

              
        //
        // GET: /PositionFocusCriteria/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var positionFocusCriteria = Repository.GetEntityByKey(id);
            if (positionFocusCriteria == null)
            {
                throw new HttpException("positionFocusCriteria not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", positionFocusCriteria);
              } else {
                  return View(positionFocusCriteria);
              }
        }

        //
        // POST: /PositionFocusCriteria/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PositionFocusCriteria positionFocusCriteria)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldPositionFocusCriteria = Repository.GetEntityByEntityKey(positionFocusCriteria);
                
                Repository.SetPropertyValuesFrom(ref oldPositionFocusCriteria,positionFocusCriteria);

                try
                {
                    Repository.Update(oldPositionFocusCriteria);     
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
                    return View(positionFocusCriteria);
                }                
            }          
        }

        //
        // POST: /PositionFocusCriteria/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(PositionFocusCriteria positionFocusCriteria)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldPositionFocusCriteria = Repository.GetEntityByEntityKey(positionFocusCriteria);
            try
            {
                Repository.Delete(oldPositionFocusCriteria);     
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
        
        var enity = T4Helper.GetEntityType("PositionFocusCriteria", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var positionDescriptionItems=ServiceRepository.PositionDescriptionRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.PositionDescriptionId.ToString(), Text =pe.BriefRoleStatement})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("positionDescriptionItems",positionDescriptionItems,ViewData);
                

          var lookupFocusGradeCriteriaItems=ServiceRepository.LookupFocusGradeCriteriaRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.LookupId.ToString(), Text =pe.GradeCode})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("lookupFocusGradeCriteriaItems",lookupFocusGradeCriteriaItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}