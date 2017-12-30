


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
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class CapabilityBehaviourIndicatorController : AppControllerBase
    {
     private CapabilityBehaviourIndicatorRepository _repository=null;
     public CapabilityBehaviourIndicatorRepository Repository
     {
         get
         {
             return _repository=_repository ?? ServiceRepository.CapabilityBehaviourIndicatorRepository();
          }
      }
                

        public ActionResult Index()
        {
            
            var capabilityBehaviourIndicators = Repository.List();
            CreateLookups();
            return View(capabilityBehaviourIndicators.ToList());
        }

        //
        // GET: /CapabilityBehaviourIndicator/Details/5
        public ActionResult Details(int nameId, int levelId)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);

            var capabilityBehaviourIndicator = ServiceRepository.CapabilityBehaviourIndicatorRepository().GetBehaviourIndicatorByNameAndLevel(levelId, nameId);
            if (capabilityBehaviourIndicator == null)
            {
                var msg = MessageHelper.NotFoundMessage("capability behavioural indicator");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", capabilityBehaviourIndicator);
              } else {
            return View(capabilityBehaviourIndicator);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableCapabiltyBehaviourInd arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var capabilityBehaviourIndicators = Repository.List();
                IQueryable<CapabilityBehaviourIndicator> displayedCapabilityBehaviourIndicators;
                if (arg.CapabilityNameId > 0)
                {
                    capabilityBehaviourIndicators = capabilityBehaviourIndicators.Where(l => l.CapabilityNameId == arg.CapabilityNameId);
                }
                if (arg.CapabilityLevelId > 0)
                {
                    capabilityBehaviourIndicators = capabilityBehaviourIndicators.Where(l => l.CapabilityLevelId == arg.CapabilityLevelId);
                }
               
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedCapabilityBehaviourIndicators = Repository.FilterCapabilityBehaviourIndicators(capabilityBehaviourIndicators, searchArgs);
                }
                else
                {
                    displayedCapabilityBehaviourIndicators = CustomOrderBy.CustomSort(capabilityBehaviourIndicators, arg);
                }

                var totalRecord =  displayedCapabilityBehaviourIndicators.Count();
                var totalDisplayRecord =  displayedCapabilityBehaviourIndicators.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedCapabilityBehaviourIndicators =  displayedCapabilityBehaviourIndicators.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedCapabilityBehaviourIndicators.AsEnumerable().ToArray().Select(ent => ent.To(new CapabilityBehaviourIndicatorLight()
                {
                    CapabilityName=ent.CapabilityName.Name,
                    CapabilityLevelName=ent.CapabilityLevel.LevelName
                }));

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

         // GET: /CapabilityBehaviourIndicator/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var capabilityBehaviourIndicatorLight = Repository.GetEntityByKey(id).To(new CapabilityBehaviourIndicatorLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = capabilityBehaviourIndicatorLight;

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



        // GET: /CapabilityBehaviourIndicator/Delete/5
        [HasAnyAdminRole]
        public ActionResult Delete(int nameId,int levelId)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var capabilityBehaviourIndicator = ServiceRepository.CapabilityBehaviourIndicatorRepository().GetBehaviourIndicatorByNameAndLevel(levelId, nameId);
            if (capabilityBehaviourIndicator == null)
            {
                var msg = MessageHelper.NotFoundMessage("capability behavioural indicator");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", capabilityBehaviourIndicator);
              } else {
            return View(capabilityBehaviourIndicator);
              }

            
        }
        //
        // GET: /CapabilityBehaviourIndicator/Create
        [HasAnyAdminRole]
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new CapabilityBehaviourIndicator());
            } else {
            return View(new CapabilityBehaviourIndicator());
            }
        }

        //
        // POST: /CapabilityBehaviourIndicator/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAnyAdminRole]
        public ActionResult Create(CapabilityBehaviourIndicator capabilityBehaviourIndicator)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var trailingList = capabilityBehaviourIndicator.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if (ModelState.IsValid) {
                try {
                    Repository.Insert(capabilityBehaviourIndicator);
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
                    return View(capabilityBehaviourIndicator);
                }
                
            }
            
        }


        //
        // GET: /CapabilityBehaviourIndicator/Edit/5
        [HasAnyAdminRole]
        public ActionResult Edit(int  nameId = 0,int levelId=0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var capabilityBehaviourIndicator = Repository.List().FirstOrDefault(l=>l.CapabilityLevelId ==levelId && l.CapabilityNameId ==nameId);
            if (capabilityBehaviourIndicator == null)
            {
                var msg = MessageHelper.NotFoundMessage("capability behavioural indicator");
                throw new HttpException(msg);
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", capabilityBehaviourIndicator);
              } else {
                  return View(capabilityBehaviourIndicator);
              }
        }

        //
        // POST: /CapabilityBehaviourIndicator/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAnyAdminRole]
        public ActionResult Edit(CapabilityBehaviourIndicator capabilityBehaviourIndicator)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var trailingList = capabilityBehaviourIndicator.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if (ModelState.IsValid)
            {
                var oldCapabilityBehaviourIndicator = Repository.GetEntityByEntityKey(capabilityBehaviourIndicator);
                
                Repository.SetPropertyValuesFrom(ref oldCapabilityBehaviourIndicator,capabilityBehaviourIndicator);

                try
                {
                    Repository.Update(oldCapabilityBehaviourIndicator);     
                } 
                catch (Exception exception) 
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error",  "Oops! something went wrong. "+exception.Message));

                    if (Request.IsAjaxRequest())
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
                    return View(capabilityBehaviourIndicator);
                }                
            }          
        }

        //
        // POST: /CapabilityBehaviourIndicator/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasAnyAdminRole]
        public ActionResult DeleteConfirmed(CapabilityBehaviourIndicator capabilityBehaviourIndicator)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldCapabilityBehaviourIndicator = Repository.GetEntityByEntityKey(capabilityBehaviourIndicator);
            try
            {
                Repository.Delete(oldCapabilityBehaviourIndicator);     
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
         public ActionResult GetBehaviourIndicators(int levelId=0, int nameId=0)
        {
            var indContext = ServiceRepository.CapabilityBehaviourIndicatorRepository().GetBehaviourIndicatorByNameAndLevel(levelId,nameId);
                
            if (indContext != null)
            {
                return Json(indContext.IndicatorContext, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    
        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("CapabilityBehaviourIndicator", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var capabilityNameItems=ServiceRepository.CapabilityNameRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.CapabilityNameId.ToString(), Text =pe.Name})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("capabilityNameItems",capabilityNameItems,ViewData);
                

          var capabilityLevelItems=ServiceRepository.CapabilityLevelRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.CapabilityLevelId.ToString(), Text =pe.LevelName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("capabilityLevelItems",capabilityLevelItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}