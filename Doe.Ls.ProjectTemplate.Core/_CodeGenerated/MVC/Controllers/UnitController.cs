  



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
    
        
    public partial class UnitController : AppControllerBase
    {
     private UnitRepository _repository=null;
     public UnitRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.UnitRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var units = Repository.List();
            return View(units.ToList());
        }

        //
        // GET: /Unit/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var unit = Repository.GetEntityByKey(id);
            if (unit == null)
            {
                throw new HttpException("unit not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", unit);
              } else {
            return View(unit);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var units = Repository.List();
                IQueryable<Unit> displayedUnits;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedUnits = Repository.FilterUnits(units, searchArgs);
                }
                else
                {
                    displayedUnits = CustomOrderBy.CustomSort(units, arg);
                }

                var totalRecord =  displayedUnits.Count();
                var totalDisplayRecord =  displayedUnits.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedUnits =  displayedUnits.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedUnits.AsEnumerable().ToArray().Select(ent => ent.To(new UnitLight()));

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

         // GET: /Unit/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var unitLight = Repository.GetEntityByKey(id).To(new UnitLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = unitLight;

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



        // GET: /Unit/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var unit = Repository.GetEntityByKey(id);
            if (unit == null)
            {
                throw new HttpException("unit not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", unit);
              } else {
            return View(unit);
              }

            
        }
        //
        // GET: /Unit/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new Unit());
            } else {
            return View(new Unit());
            }
        }

        //
        // POST: /Unit/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Unit unit)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(unit);
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
                    return View(unit);
                }
                
            }
            
        }

              
        //
        // GET: /Unit/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var unit = Repository.GetEntityByKey(id);
            if (unit == null)
            {
                throw new HttpException("unit not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", unit);
              } else {
                  return View(unit);
              }
        }

        //
        // POST: /Unit/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Unit unit)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldUnit = Repository.GetEntityByEntityKey(unit);
                
                Repository.SetPropertyValuesFrom(ref oldUnit,unit);

                try
                {
                    Repository.Update(oldUnit);     
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
                    return View(unit);
                }                
            }          
        }

        //
        // POST: /Unit/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Unit unit)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldUnit = Repository.GetEntityByEntityKey(unit);
            try
            {
                Repository.Delete(oldUnit);     
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
        
        var enity = T4Helper.GetEntityType("Unit", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var businessUnitItems=ServiceRepository.BusinessUnitRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.BUnitId.ToString(), Text =pe.BUnitName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("businessUnitItems",businessUnitItems,ViewData);
                

          var functionalAreaItems=ServiceRepository.FunctionalAreaRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.FuncationalAreaId.ToString(), Text =pe.AreanName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("functionalAreaItems",functionalAreaItems,ViewData);
                

          var unitItems=ServiceRepository.UnitRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.UnitId.ToString(), Text =pe.UnitName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("unitItems",unitItems,ViewData);
                

          var hierarchyLevelItems=ServiceRepository.HierarchyLevelRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.HierarchyId.ToString(), Text =pe.HierarchyName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("hierarchyLevelItems",hierarchyLevelItems,ViewData);
                

          var teamTypeItems=ServiceRepository.TeamTypeRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.TeamTypeId.ToString(), Text =pe.TeamTypeName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("teamTypeItems",teamTypeItems,ViewData);
                

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