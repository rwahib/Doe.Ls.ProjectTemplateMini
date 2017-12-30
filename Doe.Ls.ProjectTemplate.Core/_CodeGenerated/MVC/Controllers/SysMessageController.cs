  



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
    
        
    public partial class SysMessageController : AppControllerBase
    {
     private SysMessageRepository _repository=null;
     public SysMessageRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.SysMessageRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var sysMessages = Repository.List();
            return View(sysMessages.ToList());
        }

        //
        // GET: /SysMessage/Details/5
        public ActionResult Details(string code = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var sysMessage = Repository.GetEntityByKey(code);
            if (sysMessage == null)
            {
                throw new HttpException("sysMessage not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", sysMessage);
              } else {
            return View(sysMessage);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var sysMessages = Repository.List();
                IQueryable<SysMessage> displayedSysMessages;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedSysMessages = Repository.FilterSysMessages(sysMessages, searchArgs);
                }
                else
                {
                    displayedSysMessages = CustomOrderBy.CustomSort(sysMessages, arg);
                }

                var totalRecord =  displayedSysMessages.Count();
                var totalDisplayRecord =  displayedSysMessages.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedSysMessages =  displayedSysMessages.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedSysMessages.AsEnumerable().ToArray().Select(ent => ent.To(new SysMessageLight()));

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

         // GET: /SysMessage/Details/5
        public ActionResult DetailsJson(string code = "")
        {
            var ajaxResult = new Result();
            
            try {
                var sysMessageLight = Repository.GetEntityByKey(code).To(new SysMessageLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = sysMessageLight;

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



        // GET: /SysMessage/Delete/5
        public ActionResult Delete(string code = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var sysMessage = Repository.GetEntityByKey(code);
            if (sysMessage == null)
            {
                throw new HttpException("sysMessage not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", sysMessage);
              } else {
            return View(sysMessage);
              }

            
        }
        //
        // GET: /SysMessage/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new SysMessage());
            } else {
            return View(new SysMessage());
            }
        }

        //
        // POST: /SysMessage/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SysMessage sysMessage)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(sysMessage);
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
                    return View(sysMessage);
                }
                
            }
            
        }

              
        //
        // GET: /SysMessage/Edit/5
        public ActionResult Edit(string  code = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var sysMessage = Repository.GetEntityByKey(code);
            if (sysMessage == null)
            {
                throw new HttpException("sysMessage not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", sysMessage);
              } else {
                  return View(sysMessage);
              }
        }

        //
        // POST: /SysMessage/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SysMessage sysMessage)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldSysMessage = Repository.GetEntityByEntityKey(sysMessage);
                
                Repository.SetPropertyValuesFrom(ref oldSysMessage,sysMessage);

                try
                {
                    Repository.Update(oldSysMessage);     
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
                    return View(sysMessage);
                }                
            }          
        }

        //
        // POST: /SysMessage/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(SysMessage sysMessage)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldSysMessage = Repository.GetEntityByEntityKey(sysMessage);
            try
            {
                Repository.Delete(oldSysMessage);     
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
        
        var enity = T4Helper.GetEntityType("SysMessage", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var sysMsgCategoryItems=ServiceRepository.SysMsgCategoryRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.MsgCategoryId.ToString(), Text =pe.MsgCategoryName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("sysMsgCategoryItems",sysMsgCategoryItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}