


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
    public partial class AppObjectInfoController : AppControllerBase
    {
     private AppObjectInfoRepository _repository=null;
     public AppObjectInfoRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.AppObjectInfoRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var appObjectInfos = Repository.List();
            return View(appObjectInfos.ToList());
        }

        //
        // GET: /AppObjectInfo/Details/5
        public ActionResult Details(string objectName = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var appObjectInfo = Repository.GetEntityByKey(objectName);
            if (appObjectInfo == null)
            {
                var msg = MessageHelper.NotFoundMessage($"app object info ({objectName})");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", appObjectInfo);
              } else {
            return View(appObjectInfo);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var appObjectInfos = Repository.List();
                IQueryable<AppObjectInfo> displayedAppObjectInfos;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedAppObjectInfos = Repository.FilterAppObjectInfos(appObjectInfos, searchArgs);
                }
                else
                {
                    displayedAppObjectInfos = CustomOrderBy.CustomSort(appObjectInfos, arg);
                }

                var totalRecord =  displayedAppObjectInfos.Count();
                var totalDisplayRecord =  displayedAppObjectInfos.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedAppObjectInfos =  displayedAppObjectInfos.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedAppObjectInfos.AsEnumerable().ToArray().Select(ent => ent.To(new AppObjectInfoLight()));

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

         // GET: /AppObjectInfo/Details/5
        public ActionResult DetailsJson(string objectName = "")
        {
            var ajaxResult = new Result();
            
            try {
                var appObjectInfoLight = Repository.GetEntityByKey(objectName).To(new AppObjectInfoLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = appObjectInfoLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            } catch (Exception exception) {
                LogException(exception);
                ajaxResult.Status = Status.Error;
                ajaxResult.Message = "Errors";
                var msg = MessageHelper.ErrorOccured();
                ajaxResult.AddError(new DbValidationError("Error",
                    msg + exception.Message));
                //ajaxResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            }
            
        }



        // GET: /AppObjectInfo/Delete/5
        public ActionResult Delete(string objectName = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var appObjectInfo = Repository.GetEntityByKey(objectName);
            if (appObjectInfo == null)
            {
                var msg = MessageHelper.NotFoundMessage($"app object info ({objectName})");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", appObjectInfo);
              } else {
            return View(appObjectInfo);
              }

            
        }
        //
        // GET: /AppObjectInfo/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new AppObjectInfo());
            } else {
            return View(new AppObjectInfo());
            }
        }

        //
        // POST: /AppObjectInfo/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppObjectInfo appObjectInfo)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(appObjectInfo);
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
                    return View(appObjectInfo);
                }
                
            }
            
        }

              
        //
        // GET: /AppObjectInfo/Edit/5
        public ActionResult Edit(string  objectName = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var appObjectInfo = Repository.GetEntityByKey(objectName);
            if (appObjectInfo == null)
            {
                var msg = MessageHelper.NotFoundMessage($"app object info ({objectName})");
                throw new HttpException(msg);
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", appObjectInfo);
              } else {
                  return View(appObjectInfo);
              }
        }

        //
        // POST: /AppObjectInfo/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppObjectInfo appObjectInfo)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldAppObjectInfo = Repository.GetEntityByEntityKey(appObjectInfo);
                
                Repository.SetPropertyValuesFrom(ref oldAppObjectInfo,appObjectInfo);

                try
                {
                    Repository.Update(oldAppObjectInfo);     
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
                    return View(appObjectInfo);
                }                
            }          
        }

        //
        // POST: /AppObjectInfo/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(AppObjectInfo appObjectInfo)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldAppObjectInfo = Repository.GetEntityByEntityKey(appObjectInfo);
            try
            {
                Repository.Delete(oldAppObjectInfo);     
            }
            catch (Exception exception)
            {
                LogException(exception);
            
                var errors = Repository.GetBackendValidationErrors().ToList();
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
        
        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("AppObjectInfo", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}