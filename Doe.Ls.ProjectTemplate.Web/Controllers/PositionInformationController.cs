


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
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
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
                var msg = MessageHelper.NotFoundMessage("position information");
                throw new HttpException(msg);
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
                var msg = MessageHelper.ErrorOccured();
                dataTableResult.AddError(new DbValidationError("Error", msg + exception.Message));
                //dataTableResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
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
                var msg = MessageHelper.ErrorOccured();
                ajaxResult.AddError(new DbValidationError("Error", msg + exception.Message));
                //ajaxResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
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
                var msg = MessageHelper.NotFoundMessage($"position ({id}) information");
                throw new HttpException(msg);
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
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error",  "Oops! something went wrong.  "+exception.Message));

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
                var msg = MessageHelper.NotFoundMessage($"position ({id}) information");
                throw new HttpException(msg);
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
        [HasAnyAdminRole]
        public ActionResult Edit(PositionInformation positionInformation)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid)
            {
                var note = Request["PositionNote"];
                //load old positionInfo
                var oldInfo = Repository.GetPositionInformationById(positionInformation.PositionId);

               
                //catch the history before updating
                var historyChanges =
                    ServiceRepository.PositionHistoryRepository()
                      .GetPositionInfoChanges(oldInfo, positionInformation, string.Empty, note);

                try
                {
                    Repository.UpdateOrInsertPositionInformation(positionInformation, note);

                    //add to position history
                    var position =
                        ServiceRepository.PositionRepository().GetBasePositionById(positionInformation.PositionId);

                     if (position.StatusId != (int) Enums.StatusValue.Draft)
                     {
                        
                        ServiceRepository.PositionHistoryRepository()
                            .LogHistoryWhenUpdated(positionInformation.PositionId,
                                position.StatusId, position.StatusId,
                                historyChanges, "Position information", CurrentUser.UserName);
                    }
                } 
                catch (Exception exception) 
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    errors.Add(new DbValidationError("More info",exception.Message));

                    CreateLookups();
                    ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                    SetErrorsInTempData(errors);
                  return RedirectToAction("EditMoreInfo","Position",new {id=positionInformation.PositionId});
                   
                }

                return RedirectToAction("ManageCostCentre", "Position", new { id = positionInformation.PositionId });
            }
            else
            {
                CreateLookups();
                SetErrorsInTempData(errors);
                return RedirectToAction("EditMoreInfo", "Position", new { id = positionInformation.PositionId });
                
                                
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

/*
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrUpdate(PositionInformation positionInformation)
        {

            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if (ModelState.IsValid)
            {
                
                try
                {
                    Repository.CreateOrEditPositionInformation(positionInformation);
                   
                    
                }
                catch (Exception exception)
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    errors.Add(new DbValidationError("Error", "Oops! something went wrong  " + exception.Message));
                    SetErrorsInTempData(errors);
                    return RedirectToAction("EditMoreInfo","Position");
                }

               
                //TODO redirect to costcenmter
               
                return RedirectToAction("Index");
            }
            else
            {
                SetErrorsInTempData(Repository.GetValidationErrors(ModelState));
                return RedirectToAction("EditMoreInfo", "Position");

            }
        }*/
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
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}