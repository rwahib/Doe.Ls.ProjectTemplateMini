  



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
    
        
    public partial class PositionController : AppControllerBase
    {
     private PositionRepository _repository=null;
     public PositionRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.PositionRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var positions = Repository.List();
            return View(positions.ToList());
        }

        //
        // GET: /Position/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var position = Repository.GetEntityByKey(id);
            if (position == null)
            {
                throw new HttpException("position not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", position);
              } else {
            return View(position);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var positions = Repository.List();
                IQueryable<Position> displayedPositions;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedPositions = Repository.FilterPositions(positions, searchArgs);
                }
                else
                {
                    displayedPositions = CustomOrderBy.CustomSort(positions, arg);
                }

                var totalRecord =  displayedPositions.Count();
                var totalDisplayRecord =  displayedPositions.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedPositions =  displayedPositions.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedPositions.AsEnumerable().ToArray().Select(ent => ent.To(new PositionLight()));

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

         // GET: /Position/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var positionLight = Repository.GetEntityByKey(id).To(new PositionLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = positionLight;

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



        // GET: /Position/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var position = Repository.GetEntityByKey(id);
            if (position == null)
            {
                throw new HttpException("position not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", position);
              } else {
            return View(position);
              }

            
        }
        //
        // GET: /Position/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new Position());
            } else {
            return View(new Position());
            }
        }

        //
        // POST: /Position/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Position position)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(position);
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
                    return View(position);
                }
                
            }
            
        }

              
        //
        // GET: /Position/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var position = Repository.GetEntityByKey(id);
            if (position == null)
            {
                throw new HttpException("position not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", position);
              } else {
                  return View(position);
              }
        }

        //
        // POST: /Position/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Position position)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldPosition = Repository.GetEntityByEntityKey(position);
                
                Repository.SetPropertyValuesFrom(ref oldPosition,position);

                try
                {
                    Repository.Update(oldPosition);     
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
                    return View(position);
                }                
            }          
        }

        //
        // POST: /Position/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Position position)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldPosition = Repository.GetEntityByEntityKey(position);
            try
            {
                Repository.Delete(oldPosition);     
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
        
        var enity = T4Helper.GetEntityType("Position", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var positionItems=ServiceRepository.PositionRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.PositionId.ToString(), Text =pe.PositionTitle})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("positionItems",positionItems,ViewData);
                

          var rolePositionDescriptionItems=ServiceRepository.RolePositionDescriptionRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.RolePositionDescId.ToString(), Text =pe.Title})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("rolePositionDescriptionItems",rolePositionDescriptionItems,ViewData);
                

          var unitItems=ServiceRepository.UnitRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.UnitId.ToString(), Text =pe.UnitName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("unitItems",unitItems,ViewData);
                

          var positionLevelItems=ServiceRepository.PositionLevelRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.PositionLevelId.ToString(), Text =pe.PositionLevelName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("positionLevelItems",positionLevelItems,ViewData);
                

          var statusValueItems=ServiceRepository.StatusValueRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.StatusId.ToString(), Text =pe.StatusName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("statusValueItems",statusValueItems,ViewData);
                

          var locationItems=ServiceRepository.LocationRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.LocationId.ToString(), Text =pe.Name})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("locationItems",locationItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}