


using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class GlobalItemController : AppControllerBase
    {
     private GlobalItemRepository _repository=null;
     public GlobalItemRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.GlobalItemRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var globalItems = Repository.List();
            return View(globalItems.ToList());
        }

        //
        // GET: /GlobalItem/Details/5
        public ActionResult Details(string itemCode = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var globalItem = Repository.GetEntityByKey(itemCode);
            if (globalItem == null)
            {
                throw new HttpException($"The global item ({itemCode}) not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", globalItem);
              } else {
            return View(globalItem);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var globalItems = Repository.List();
                IQueryable<GlobalItem> displayedGlobalItems;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedGlobalItems = Repository.FilterItems(globalItems, searchArgs);
                }
                else
                {
                    displayedGlobalItems = CustomOrderBy.CustomSort(globalItems, arg);
                }

                var totalRecord =  displayedGlobalItems.Count();
                var totalDisplayRecord =  displayedGlobalItems.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedGlobalItems =  displayedGlobalItems.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedGlobalItems.AsEnumerable().ToArray().Select(ent => ent.To(new GlobalItemLight()));

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

         // GET: /GlobalItem/Details/5
        public ActionResult DetailsJson(string itemCode = "")
        {
            var ajaxResult = new Result();
            
            try {
                var globalItemLight = Repository.GetEntityByKey(itemCode).To(new GlobalItemLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = globalItemLight;

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



        // GET: /GlobalItem/Delete/5
        public ActionResult Delete(string itemCode = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var globalItem = Repository.GetEntityByKey(itemCode);
            if (globalItem == null)
            {
                throw new HttpException($"The global item ({itemCode}) not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", globalItem);
              } else {
            return View(globalItem);
              }

            
        }
        //
        // GET: /GlobalItem/Create
        [HasAdminOrPowerRole]
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new GlobalItem());
            } else {
            return View(new GlobalItem());
            }
        }

        //
        // POST: /GlobalItem/Create

        [System.Web.Mvc.HttpPost]
        [HasAdminOrPowerRole]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GlobalItem globalItem)
        {

            ModelState.Remove("CreatedDate");
            ModelState.Remove("LastupdatedBy");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("LastupdatedDate");
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try
                {

                    globalItem.CreatedBy = CurrentUser.UserName;
                    globalItem.LastupdatedBy = CurrentUser.UserName;
                    globalItem.CreatedDate = DateTime.Now;
                    globalItem.LastupdatedDate = DateTime.Now;
                    Repository.Insert(globalItem);
                } catch (Exception exception) {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error",  "Oops! something went wrong. "+exception.Message));

                    if (Request.IsAjaxRequest()) {
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
                    return View(globalItem);
                }
                
            }
            
        }


        //
        // GET: /GlobalItem/Edit/5
        [HasAdminOrPowerRole]
        public ActionResult Edit(string  itemCode = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var globalItem = Repository.GetEntityByKey(itemCode);
            if (globalItem == null)
            {
                throw new HttpException($"The global item ({itemCode}) not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", globalItem);
              } else {
                  return View(globalItem);
              }
        }

        //
        // POST: /GlobalItem/Edit/5
        [System.Web.Mvc.HttpPost]
        [HasAdminOrPowerRole]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GlobalItem globalItem)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var trailingList = globalItem.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            ModelState.Remove("CreatedDate");
            ModelState.Remove("LastupdatedBy");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("LastupdatedDate");
            if (ModelState.IsValid)
            {
                var oldGlobalItem = Repository.GetEntityByEntityKey(globalItem);

                globalItem.CreatedBy = oldGlobalItem.CreatedBy;
                globalItem.CreatedDate = oldGlobalItem.CreatedDate;
                Repository.SetPropertyValuesFrom(ref oldGlobalItem,globalItem);

                try
                {
                    oldGlobalItem.LastupdatedBy = CurrentUser.UserName;
                    oldGlobalItem.LastupdatedDate = DateTime.Now;
                    Repository.Update(oldGlobalItem);     
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
                    return View(globalItem);
                }                
            }          
        }

        //
        // POST: /GlobalItem/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(GlobalItem globalItem)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldGlobalItem = Repository.GetEntityByEntityKey(globalItem);
            try
            {
                Repository.Delete(oldGlobalItem);     
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
        
        var enity = T4Helper.GetEntityType("GlobalItem", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}