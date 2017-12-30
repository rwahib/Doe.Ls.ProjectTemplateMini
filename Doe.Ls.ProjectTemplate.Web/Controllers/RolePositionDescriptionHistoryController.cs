


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
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    
        
    public partial class RolePositionDescriptionHistoryController : AppControllerBase
    {
     private RolePositionDescriptionHistoryRepository _repository=null;
     public RolePositionDescriptionHistoryRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.RolePositionDescriptionHistoryRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var rolePositionDescriptionHistorys = Repository.List();
            return View(rolePositionDescriptionHistorys.ToList());
        }

        //
        // GET: /RolePositionDescriptionHistory/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var rolePositionDescriptionHistory = Repository.GetEntityByKey(id);
            if (rolePositionDescriptionHistory == null)
            {
                var msg = MessageHelper.NotFoundMessage("role/position description history");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", rolePositionDescriptionHistory);
              } else {
            return View(rolePositionDescriptionHistory);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var rolePositionDescriptionHistorys = Repository.List();
                IQueryable<RolePositionDescriptionHistory> displayedRolePositionDescriptionHistorys;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedRolePositionDescriptionHistorys = Repository.FilterRolePositionDescriptionHistorys(rolePositionDescriptionHistorys, searchArgs);
                }
                else
                {
                    displayedRolePositionDescriptionHistorys = CustomOrderBy.CustomSort(rolePositionDescriptionHistorys, arg);
                }

                var totalRecord =  displayedRolePositionDescriptionHistorys.Count();
                var totalDisplayRecord =  displayedRolePositionDescriptionHistorys.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedRolePositionDescriptionHistorys =  displayedRolePositionDescriptionHistorys.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedRolePositionDescriptionHistorys.AsEnumerable().ToArray().Select(ent => ent.To(new RolePositionDescriptionHistoryLight()));

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
                dataTableResult.AddError(new DbValidationError("DB ", msg + exception.Message));
                //dataTableResult.AddError(new DbValidationError("DB ",
                //    "Oops! something went wrong. " + exception.Message));
            }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
        }

         // GET: /RolePositionDescriptionHistory/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var rolePositionDescriptionHistoryLight = Repository.GetEntityByKey(id).To(new RolePositionDescriptionHistoryLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = rolePositionDescriptionHistoryLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            } catch (Exception exception) {
                LogException(exception);
                ajaxResult.Status = Status.Error;
                ajaxResult.Message = "Errors";
                var msg = MessageHelper.ErrorOccured();
                ajaxResult.AddError(new DbValidationError("DB ", msg + exception.Message));
                //ajaxResult.AddError(new DbValidationError("DB ",
                //    "Oops! something went wrong. " + exception.Message));
                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            }
            
        }



        // GET: /RolePositionDescriptionHistory/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var rolePositionDescriptionHistory = Repository.GetEntityByKey(id);
            if (rolePositionDescriptionHistory == null)
            {
                var msg = MessageHelper.NotFoundMessage("role/position description history");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", rolePositionDescriptionHistory);
              } else {
            return View(rolePositionDescriptionHistory);
              }

            
        }
        //
        // GET: /RolePositionDescriptionHistory/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new RolePositionDescriptionHistory());
            } else {
            return View(new RolePositionDescriptionHistory());
            }
        }

        //
        // POST: /RolePositionDescriptionHistory/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolePositionDescriptionHistory rolePositionDescriptionHistory)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(rolePositionDescriptionHistory);
                } catch (Exception exception) {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("DB", msg + exception.Message));
                    //errors.Add(new DbValidationError("DB",  "Oops! something went wrong. "+exception.Message));

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
                    return View(rolePositionDescriptionHistory);
                }
                
            }
            
        }

              
        //
        // GET: /RolePositionDescriptionHistory/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var rolePositionDescriptionHistory = Repository.GetEntityByKey(id);
            if (rolePositionDescriptionHistory == null)
            {
                var msg = MessageHelper.NotFoundMessage("role/position description history");
                throw new HttpException(msg);
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", rolePositionDescriptionHistory);
              } else {
                  return View(rolePositionDescriptionHistory);
              }
        }

        //
        // POST: /RolePositionDescriptionHistory/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RolePositionDescriptionHistory rolePositionDescriptionHistory)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldRolePositionDescriptionHistory = Repository.GetEntityByEntityKey(rolePositionDescriptionHistory);
                
                Repository.SetPropertyValuesFrom(ref oldRolePositionDescriptionHistory,rolePositionDescriptionHistory);

                try
                {
                    Repository.Update(oldRolePositionDescriptionHistory);     
                } 
                catch (Exception exception) 
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("DB error", msg + exception.Message));
                    //errors.Add(new DbValidationError("DB error",  "Oops! something went wrong. "+exception.Message));

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
                    return View(rolePositionDescriptionHistory);
                }                
            }          
        }

        //
        // POST: /RolePositionDescriptionHistory/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(RolePositionDescriptionHistory rolePositionDescriptionHistory)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldRolePositionDescriptionHistory = Repository.GetEntityByEntityKey(rolePositionDescriptionHistory);
            try
            {
                Repository.Delete(oldRolePositionDescriptionHistory);     
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
        
        var enity = T4Helper.GetEntityType("RolePositionDescriptionHistory", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);

            var rolePositionDescriptionList = ServiceRepository.RolePositionDescriptionRepository()
                .List().ToArray();

            
                var rolePositionDescriptionItems = rolePositionDescriptionList
                    .Select(
                        pe => new SelectListItemExtension {Value = pe.RolePositionDescId.ToString(), Text = pe.Title})
                    .ToArray();

                ViewBagWrapper.ListBag.SetList("rolePositionDescriptionItems", rolePositionDescriptionItems, ViewData);
            

        }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}