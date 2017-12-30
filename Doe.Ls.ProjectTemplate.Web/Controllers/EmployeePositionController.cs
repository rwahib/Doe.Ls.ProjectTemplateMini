


using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class EmployeePositionController : AppControllerBase
    {
     private EmployeePositionRepository _repository=null;
     public EmployeePositionRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.EmployeePositionRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var employeePositions = Repository.List();
            return View(employeePositions.ToList());
        }

        //
        // GET: /EmployeePosition/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var employeePosition = Repository.GetEntityByKey(id);
            if (employeePosition == null)
            {
                throw new HttpException($"The employee position ({id}) not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", employeePosition);
              } else {
            return View(employeePosition);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var employeePositions = Repository.List();
                IQueryable<EmployeePosition> displayedEmployeePositions;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedEmployeePositions = Repository.FilterEmployeePositions(employeePositions, searchArgs);
                }
                else
                {
                    displayedEmployeePositions = CustomOrderBy.CustomSort(employeePositions, arg);
                }

                var totalRecord =  displayedEmployeePositions.Count();
                var totalDisplayRecord =  displayedEmployeePositions.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedEmployeePositions =  displayedEmployeePositions.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedEmployeePositions.AsEnumerable().ToArray().Select(ent => ent.To(new EmployeePositionLight()));

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
                var msg = MessageHelper.ErrorOccured();
                dataTableResult.AddError(new DbValidationError("Error", msg + exception.Message));
                //dataTableResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
            }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
        }

         // GET: /EmployeePosition/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var employeePositionLight = Repository.GetEntityByKey(id).To(new EmployeePositionLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = employeePositionLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            } catch (Exception exception) {
                LogException(exception);
                ajaxResult.Status = Status.Error;
                ajaxResult.Message = "Errors";
                var msg = MessageHelper.ErrorOccured();
                ajaxResult.AddError(new DbValidationError("Error", msg + exception.Message));
                //ajaxResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            }
            
        }



        // GET: /EmployeePosition/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var employeePosition = Repository.GetEntityByKey(id);
            if (employeePosition == null)
            {
                throw new HttpException($"The employee position ({id}) not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", employeePosition);
              } else {
            return View(employeePosition);
              }

            
        }
        //
        // GET: /EmployeePosition/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new EmployeePosition());
            } else {
            return View(new EmployeePosition());
            }
        }

        //
        // POST: /EmployeePosition/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeePosition employeePosition)
        {
            this.ModelState.Remove("DisplayInOrgChart"); 

employeePosition.DisplayInOrgChart = Request["DisplayInOrgChart"].IsOn();
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(employeePosition);
                } catch (Exception exception) {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error",  "Oops! something went wrong. "+exception.Message));

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
                    return View(employeePosition);
                }
                
            }
            
        }

              
        //
        // GET: /EmployeePosition/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var employeePosition = Repository.GetEntityByKey(id);
            if (employeePosition == null)
            {
                throw new HttpException($"The employee position ({id}) not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", employeePosition);
              } else {
                  return View(employeePosition);
              }
        }

        //
        // POST: /EmployeePosition/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeePosition employeePosition)
        {
            this.ModelState.Remove("DisplayInOrgChart"); 

employeePosition.DisplayInOrgChart = Request["DisplayInOrgChart"].IsOn();
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldEmployeePosition = Repository.GetEntityByEntityKey(employeePosition);
                
                Repository.SetPropertyValuesFrom(ref oldEmployeePosition,employeePosition);

                try
                {
                    Repository.Update(oldEmployeePosition);     
                } 
                catch (Exception exception) 
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error",  "Oops! something went wrong. "+exception.Message));

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
                    return View(employeePosition);
                }                
            }          
        }

        //
        // POST: /EmployeePosition/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(EmployeePosition employeePosition)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldEmployeePosition = Repository.GetEntityByEntityKey(employeePosition);
            try
            {
                Repository.Delete(oldEmployeePosition);     
            }
            catch (Exception exception)
            {
                LogException(exception);

                var errors = Repository.GetBackendValidationErrors().ToList();

                errors.Add(new DbValidationError("", string.Format(GetMessage("ERR-GENERAL").MessageFormat, exception.Message)));
                errors.Add(new DbValidationError("", GetMessage("ERR-GENERAL-DELETE-HAS-CHILDS").MessageFormat));

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
        
        var enity = T4Helper.GetEntityType("EmployeePosition", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var employeeItems=ServiceRepository.EmployeeRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.EmployeeId.ToString(), Text =pe.FirstName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("employeeItems",employeeItems,ViewData);
                

          var positionItems=ServiceRepository.PositionRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.PositionId.ToString(), Text =pe.PositionTitle})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("positionItems",positionItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}