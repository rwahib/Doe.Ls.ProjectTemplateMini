  



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
    
        
    public partial class RelationshipScopeController : AppControllerBase
    {
     private RelationshipScopeRepository _repository=null;
     public RelationshipScopeRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.RelationshipScopeRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var relationshipScopes = Repository.List();
            return View(relationshipScopes.ToList());
        }

        //
        // GET: /RelationshipScope/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var relationshipScope = Repository.GetEntityByKey(id);
            if (relationshipScope == null)
            {
                throw new HttpException("relationshipScope not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", relationshipScope);
              } else {
            return View(relationshipScope);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var relationshipScopes = Repository.List();
                IQueryable<RelationshipScope> displayedRelationshipScopes;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedRelationshipScopes = Repository.FilterRelationshipScopes(relationshipScopes, searchArgs);
                }
                else
                {
                    displayedRelationshipScopes = CustomOrderBy.CustomSort(relationshipScopes, arg);
                }

                var totalRecord =  displayedRelationshipScopes.Count();
                var totalDisplayRecord =  displayedRelationshipScopes.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedRelationshipScopes =  displayedRelationshipScopes.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedRelationshipScopes.AsEnumerable().ToArray().Select(ent => ent.To(new RelationshipScopeLight()));

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

         // GET: /RelationshipScope/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var relationshipScopeLight = Repository.GetEntityByKey(id).To(new RelationshipScopeLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = relationshipScopeLight;

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



        // GET: /RelationshipScope/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var relationshipScope = Repository.GetEntityByKey(id);
            if (relationshipScope == null)
            {
                throw new HttpException("relationshipScope not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", relationshipScope);
              } else {
            return View(relationshipScope);
              }

            
        }
        //
        // GET: /RelationshipScope/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new RelationshipScope());
            } else {
            return View(new RelationshipScope());
            }
        }

        //
        // POST: /RelationshipScope/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RelationshipScope relationshipScope)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(relationshipScope);
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
                    return View(relationshipScope);
                }
                
            }
            
        }

              
        //
        // GET: /RelationshipScope/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var relationshipScope = Repository.GetEntityByKey(id);
            if (relationshipScope == null)
            {
                throw new HttpException("relationshipScope not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", relationshipScope);
              } else {
                  return View(relationshipScope);
              }
        }

        //
        // POST: /RelationshipScope/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RelationshipScope relationshipScope)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldRelationshipScope = Repository.GetEntityByEntityKey(relationshipScope);
                
                Repository.SetPropertyValuesFrom(ref oldRelationshipScope,relationshipScope);

                try
                {
                    Repository.Update(oldRelationshipScope);     
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
                    return View(relationshipScope);
                }                
            }          
        }

        //
        // POST: /RelationshipScope/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(RelationshipScope relationshipScope)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldRelationshipScope = Repository.GetEntityByEntityKey(relationshipScope);
            try
            {
                Repository.Delete(oldRelationshipScope);     
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
        
        var enity = T4Helper.GetEntityType("RelationshipScope", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}