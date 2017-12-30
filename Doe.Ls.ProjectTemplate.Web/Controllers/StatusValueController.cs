


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
    public partial class StatusValueController : AppControllerBase
    {
     private StatusValueRepository _repository=null;
     public StatusValueRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.StatusValueRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var statusValues = Repository.List();
            return View(statusValues.ToList());
        }

        //
        // GET: /StatusValue/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var statusValue = Repository.GetEntityByKey(id);
            if (statusValue == null)
            {
                var msg = MessageHelper.NotFoundMessage("status value");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", statusValue);
              } else {
            return View(statusValue);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var statusValues = Repository.List();
                IQueryable<StatusValue> displayedStatusValues;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedStatusValues = Repository.FilterStatusValues(statusValues, searchArgs);
                }
                else
                {
                    displayedStatusValues = CustomOrderBy.CustomSort(statusValues, arg);
                }

                var totalRecord =  displayedStatusValues.Count();
                var totalDisplayRecord =  displayedStatusValues.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedStatusValues =  displayedStatusValues.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedStatusValues.AsEnumerable().ToArray().Select(ent => ent.To(new StatusValueLight()));

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

         // GET: /StatusValue/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var statusValueLight = Repository.GetEntityByKey(id).To(new StatusValueLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = statusValueLight;

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



        // GET: /StatusValue/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var statusValue = Repository.GetEntityByKey(id);
            if (statusValue == null)
            {
                var msg = MessageHelper.NotFoundMessage("status value");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", statusValue);
              } else {
            return View(statusValue);
              }

            
        }
        //
        // GET: /StatusValue/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new StatusValue());
            } else {
            return View(new StatusValue());
            }
        }

        //
        // POST: /StatusValue/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StatusValue statusValue)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(statusValue);
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
                    return View(statusValue);
                }
                
            }
            
        }

              
        //
        // GET: /StatusValue/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var statusValue = Repository.GetEntityByKey(id);
            if (statusValue == null)
            {
                var msg = MessageHelper.NotFoundMessage("status value");
                throw new HttpException(msg);
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", statusValue);
              } else {
                  return View(statusValue);
              }
        }

        //
        // POST: /StatusValue/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StatusValue statusValue)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldStatusValue = Repository.GetEntityByEntityKey(statusValue);
                
                Repository.SetPropertyValuesFrom(ref oldStatusValue,statusValue);

                try
                {
                    Repository.Update(oldStatusValue);     
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
                    return View(statusValue);
                }                
            }          
        }

        //
        // POST: /StatusValue/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(StatusValue statusValue)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldStatusValue = Repository.GetEntityByEntityKey(statusValue);
            try
            {
                Repository.Delete(oldStatusValue);     
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
        
        var enity = T4Helper.GetEntityType("StatusValue", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}