


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

    public partial class CapabilityNameController : AppControllerBase
    {
     private CapabilityNameRepository _repository=null;
     public CapabilityNameRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.CapabilityNameRepository();

             }
 
         }

       
        public ActionResult Index()
            {
            var capabilityNames = Repository.List();
            CreateLookups();
            return View(capabilityNames.ToList());
            }


        //
        // GET: /CapabilityName/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var capabilityName = Repository.GetEntityByKey(id);
            if (capabilityName == null)
            {
                var msg = MessageHelper.NotFoundMessage("capability name");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", capabilityName);
              } else {
            return View(capabilityName);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableCapabiltyName arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var capabilityNames = Repository.List();
                IQueryable<CapabilityName> displayedCapabilityNames;
                if (arg.CapabilityGroupId > 0)
                {
                    capabilityNames = capabilityNames.Where(l => l.CapabilityGroupId == arg.CapabilityGroupId);
                }
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedCapabilityNames = Repository.FilterCapabilityNames(capabilityNames, searchArgs);
                }
                else
                {
                    displayedCapabilityNames = CustomOrderBy.CustomSort(capabilityNames, arg);
                }

                var totalRecord =  displayedCapabilityNames.Count();
                var totalDisplayRecord =  displayedCapabilityNames.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedCapabilityNames =  displayedCapabilityNames.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedCapabilityNames.AsEnumerable().ToArray().Select(ent => ent.To(new CapabilityNameLight()
                {
                    CapabilityGroupName = ent.CapabilityGroup.GroupName,
                    CanDelete = ent.RoleCapabilities.Count == 0 && ent.CapabilityBehaviourIndicators.Count == 0
                    
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

         // GET: /CapabilityName/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var capabilityNameLight = Repository.GetEntityByKey(id).To(new CapabilityNameLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = capabilityNameLight;

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



        // GET: /CapabilityName/Delete/5
        [HasAdminOrPowerRole]
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var capabilityName = Repository.GetEntityByKey(id);
            if (capabilityName == null)
            {
                var msg = MessageHelper.NotFoundMessage("capability name");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", capabilityName);
              } else {
            return View(capabilityName);
              }

            
        }
        //
        // GET: /CapabilityName/Create
        [HasAdminOrPowerRole]
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new CapabilityName());
            } else {
            return View(new CapabilityName());
            }
        }

        //
        // POST: /CapabilityName/Create
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CapabilityName capabilityName)
        {
            ModelState.Remove("CreatedBy");
            ModelState.Remove("LastModifiedBy");

            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(capabilityName);
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
                    return View(capabilityName);
                }
                
            }
            
        }


        //
        // GET: /CapabilityName/Edit/5
        [HasAdminOrPowerRole]
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var capabilityName = Repository.GetEntityByKey(id);
            if (capabilityName == null)
            {
                var msg = MessageHelper.NotFoundMessage("capability name");
                throw new HttpException(msg);
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", capabilityName);
              } else {
                  return View(capabilityName);
              }
        }

        //
        // POST: /CapabilityName/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAdminOrPowerRole]
        public ActionResult Edit(CapabilityName capabilityName)
        {
            ModelState.Remove("CreatedBy");
            ModelState.Remove("LastModifiedBy");
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var trailingList = capabilityName.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            var errors = Repository.GetValidationErrors(ModelState).ToList();


            if (ModelState.IsValid)
            {
                var oldCapabilityName = Repository.GetEntityByEntityKey(capabilityName);
                var olddateCreated = oldCapabilityName.DateCreated;


                Repository.SetPropertyValuesFrom(ref oldCapabilityName,capabilityName);

                try
                {
                    oldCapabilityName.DateCreated = olddateCreated;
                    Repository.Update(oldCapabilityName);     
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
                    return View(capabilityName);
                }                
            }          
        }

        //
        // POST: /CapabilityName/Delete/5
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(CapabilityName capabilityName)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldCapabilityName = Repository.GetEntityByEntityKey(capabilityName);
            try
            {
                Repository.Delete(oldCapabilityName);     
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
        
        var enity = T4Helper.GetEntityType("CapabilityName", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var capabilityGroupItems=ServiceRepository.CapabilityGroupRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.CapabilityGroupId.ToString(), Text =pe.GroupName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("capabilityGroupItems",capabilityGroupItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}