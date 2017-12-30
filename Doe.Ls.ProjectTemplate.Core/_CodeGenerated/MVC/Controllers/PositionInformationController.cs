  



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
    
        
    public partial class PositionInformationController : AppControllerBase
    {
     private PositionInformationRepository _repository=null;
     public PositionInformationRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.PositionInformationRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var positionInformations = Repository.List();
            return View(positionInformations.ToList());
        }

        //
        // GET: /PositionInformation/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var positionInformation = Repository.GetEntityByKey(id);
            if (positionInformation == null)
            {
                throw new HttpException("positionInformation not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", positionInformation);
              } else {
            return View(positionInformation);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var positionInformations = Repository.List();
                IQueryable<PositionInformation> displayedPositionInformations;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedPositionInformations = Repository.FilterPositionInformations(positionInformations, searchArgs);
                }
                else
                {
                    displayedPositionInformations = CustomOrderBy.CustomSort(positionInformations, arg);
                }

                var totalRecord =  displayedPositionInformations.Count();
                var totalDisplayRecord =  displayedPositionInformations.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedPositionInformations =  displayedPositionInformations.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedPositionInformations.AsEnumerable().ToArray().Select(ent => ent.To(new PositionInformationLight()));

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

         // GET: /PositionInformation/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var positionInformationLight = Repository.GetEntityByKey(id).To(new PositionInformationLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = positionInformationLight;

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



        // GET: /PositionInformation/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var positionInformation = Repository.GetEntityByKey(id);
            if (positionInformation == null)
            {
                throw new HttpException("positionInformation not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", positionInformation);
              } else {
            return View(positionInformation);
              }

            
        }
        //
        // GET: /PositionInformation/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new PositionInformation());
            } else {
            return View(new PositionInformation());
            }
        }

        //
        // POST: /PositionInformation/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PositionInformation positionInformation)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(positionInformation);
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
                    return View(positionInformation);
                }
                
            }
            
        }

              
        //
        // GET: /PositionInformation/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var positionInformation = Repository.GetEntityByKey(id);
            if (positionInformation == null)
            {
                throw new HttpException("positionInformation not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", positionInformation);
              } else {
                  return View(positionInformation);
              }
        }

        //
        // POST: /PositionInformation/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PositionInformation positionInformation)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldPositionInformation = Repository.GetEntityByEntityKey(positionInformation);
                
                Repository.SetPropertyValuesFrom(ref oldPositionInformation,positionInformation);

                try
                {
                    Repository.Update(oldPositionInformation);     
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
                    return View(positionInformation);
                }                
            }          
        }

        //
        // POST: /PositionInformation/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(PositionInformation positionInformation)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldPositionInformation = Repository.GetEntityByEntityKey(positionInformation);
            try
            {
                Repository.Delete(oldPositionInformation);     
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
        
        var enity = T4Helper.GetEntityType("PositionInformation", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var positionItems=ServiceRepository.PositionRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.PositionId.ToString(), Text =pe.PositionTitle})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("positionItems",positionItems,ViewData);
                

          var positionTypeItems=ServiceRepository.PositionTypeRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.PositionTypeCode.ToString(), Text =pe.PositionTypeName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("positionTypeItems",positionTypeItems,ViewData);
                

          var employeeTypeItems=ServiceRepository.EmployeeTypeRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.EmployeeTypeCode.ToString(), Text =pe.EmployeeTypeName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("employeeTypeItems",employeeTypeItems,ViewData);
                

          var positionStatusValueItems=ServiceRepository.PositionStatusValueRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.PosStatusCode.ToString(), Text =pe.PosStatusTitle})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("positionStatusValueItems",positionStatusValueItems,ViewData);
                

          var occupationTypeItems=ServiceRepository.OccupationTypeRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.OccupationTypeCode.ToString(), Text =pe.OccupationTypeName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("occupationTypeItems",occupationTypeItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}