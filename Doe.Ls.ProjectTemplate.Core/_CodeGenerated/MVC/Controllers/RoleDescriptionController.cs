  



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
    
        
    public partial class RoleDescriptionController : AppControllerBase
    {
     private RoleDescriptionRepository _repository=null;
     public RoleDescriptionRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.RoleDescriptionRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var roleDescriptions = Repository.List();
            return View(roleDescriptions.ToList());
        }

        //
        // GET: /RoleDescription/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var roleDescription = Repository.GetEntityByKey(id);
            if (roleDescription == null)
            {
                throw new HttpException("roleDescription not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", roleDescription);
              } else {
            return View(roleDescription);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var roleDescriptions = Repository.List();
                IQueryable<RoleDescription> displayedRoleDescriptions;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedRoleDescriptions = Repository.FilterRoleDescriptions(roleDescriptions, searchArgs);
                }
                else
                {
                    displayedRoleDescriptions = CustomOrderBy.CustomSort(roleDescriptions, arg);
                }

                var totalRecord =  displayedRoleDescriptions.Count();
                var totalDisplayRecord =  displayedRoleDescriptions.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedRoleDescriptions =  displayedRoleDescriptions.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedRoleDescriptions.AsEnumerable().ToArray().Select(ent => ent.To(new RoleDescriptionLight()));

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

         // GET: /RoleDescription/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var roleDescriptionLight = Repository.GetEntityByKey(id).To(new RoleDescriptionLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = roleDescriptionLight;

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



        // GET: /RoleDescription/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var roleDescription = Repository.GetEntityByKey(id);
            if (roleDescription == null)
            {
                throw new HttpException("roleDescription not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", roleDescription);
              } else {
            return View(roleDescription);
              }

            
        }
        //
        // GET: /RoleDescription/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new RoleDescription());
            } else {
            return View(new RoleDescription());
            }
        }

        //
        // POST: /RoleDescription/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleDescription roleDescription)
        {
            this.ModelState.Remove("ManagerRole"); 
roleDescription.ManagerRole = Request["ManagerRole"].IsOn();
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(roleDescription);
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
                    return View(roleDescription);
                }
                
            }
            
        }

              
        //
        // GET: /RoleDescription/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var roleDescription = Repository.GetEntityByKey(id);
            if (roleDescription == null)
            {
                throw new HttpException("roleDescription not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", roleDescription);
              } else {
                  return View(roleDescription);
              }
        }

        //
        // POST: /RoleDescription/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleDescription roleDescription)
        {
            this.ModelState.Remove("ManagerRole"); 
roleDescription.ManagerRole = Request["ManagerRole"].IsOn();
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldRoleDescription = Repository.GetEntityByEntityKey(roleDescription);
                
                Repository.SetPropertyValuesFrom(ref oldRoleDescription,roleDescription);

                try
                {
                    Repository.Update(oldRoleDescription);     
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
                    return View(roleDescription);
                }                
            }          
        }

        //
        // POST: /RoleDescription/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(RoleDescription roleDescription)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldRoleDescription = Repository.GetEntityByEntityKey(roleDescription);
            try
            {
                Repository.Delete(oldRoleDescription);     
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
        
        var enity = T4Helper.GetEntityType("RoleDescription", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var rolePositionDescriptionItems=ServiceRepository.RolePositionDescriptionRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.RolePositionDescId.ToString(), Text =pe.Title})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("rolePositionDescriptionItems",rolePositionDescriptionItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}