


using System;
using System.Collections.Generic;
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
    public partial class RolePositionDescriptionController : AppControllerBase
    {
     private RolePositionDescriptionRepository _repository=null;
     public RolePositionDescriptionRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.RolePositionDescriptionRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var rolePositionDescriptions = Repository.List();
            return View(rolePositionDescriptions.ToList());
        }
        public ActionResult Manage(int id = 0)
        {
            var rolePositionDescription = Repository.GetRolePositionDescById(id);
            if (rolePositionDescription == null)
            {
                var msg = MessageHelper.NotFoundMessage($"role/position description ({id})");
                throw new InvalidOperationException(msg);
            }
            return RedirectToAction("ManageSummary", rolePositionDescription.IsPositionDescription ? "PositionDescription" : "RoleDescription", new {id = id});
        }
        //
        // GET: /RolePositionDescription/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var rolePositionDescription = Repository.GetEntityByKey(id);
            if (rolePositionDescription == null)
            {
                var msg = MessageHelper.NotFoundMessage($"role/position description ({id})");
                throw new HttpException(msg);
            }
            if (rolePositionDescription.IsPositionDescription)
            {
                return RedirectToAction("ManageSummary", "PositionDescription",new {id=id});
            }
            else
            {
                return RedirectToAction("Details", "RoleDescription", new { id = id });
            }


        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var rolePositionDescriptions = Repository.List();
                IQueryable<RolePositionDescription> displayedRolePositionDescriptions;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedRolePositionDescriptions = Repository.FilterRolePositionDescriptions(rolePositionDescriptions, searchArgs);
                }
                else
                {
                    displayedRolePositionDescriptions = CustomOrderBy.CustomSort(rolePositionDescriptions, arg);
                }

                var totalRecord =  displayedRolePositionDescriptions.Count();
                var totalDisplayRecord =  displayedRolePositionDescriptions.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedRolePositionDescriptions =  displayedRolePositionDescriptions.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedRolePositionDescriptions.AsEnumerable().ToArray().Select(ent => ent.To(new RolePositionDescriptionLight()));

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

         // GET: /RolePositionDescription/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var rolePositionDescriptionLight = Repository.GetEntityByKey(id).To(new RolePositionDescriptionLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = rolePositionDescriptionLight;

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



        // GET: /RolePositionDescription/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var rolePositionDescription = Repository.GetEntityByKey(id);
            if (rolePositionDescription == null)
            {
                var msg = MessageHelper.NotFoundMessage($"role/position description ({id})");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", rolePositionDescription);
              } else {
            return View(rolePositionDescription);
              }

            
        }
        //
        // GET: /RolePositionDescription/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new RolePositionDescription());
            } else {
            return View(new RolePositionDescription());
            }
        }

        //
        // POST: /RolePositionDescription/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolePositionDescription rolePositionDescription)
        {
            this.ModelState.Remove("IsPositionDescription"); 

            rolePositionDescription.IsPositionDescription = Request["IsPositionDescription"].IsOn();
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(rolePositionDescription);
                    
                    
                   
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
                    return View(rolePositionDescription);
                }
                
            }
            
        }

              
        //
        // GET: /RolePositionDescription/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var rolePositionDescription = Repository.GetEntityByKey(id);
            if (rolePositionDescription == null)
            {
                var msg = MessageHelper.NotFoundMessage($"role/position description ({id})");
                throw new HttpException(msg);
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", rolePositionDescription);
              } else {
                  return View(rolePositionDescription);
              }
        }

        //
        // POST: /RolePositionDescription/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RolePositionDescription rolePositionDescription)
        {
            this.ModelState.Remove("IsPositionDescription"); 

rolePositionDescription.IsPositionDescription = Request["IsPositionDescription"].IsOn();
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldRolePositionDescription = Repository.GetEntityByEntityKey(rolePositionDescription);
                
               Repository.SetPropertyValuesFrom(ref oldRolePositionDescription,rolePositionDescription);

                try
                {
                    Repository.Update(oldRolePositionDescription);
                  
                }
                catch (Exception exception) 
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error",  "Oops! something went wrong. " + exception.Message));

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
                    return View(rolePositionDescription);
                }                
            }          
        }

        //
        // POST: /RolePositionDescription/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(RolePositionDescription rolePositionDescription)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldRolePositionDescription = Repository.GetEntityByEntityKey(rolePositionDescription);
            try
            {
                Repository.Delete(oldRolePositionDescription);     
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

        [System.Web.Mvc.HttpGet, System.Web.Mvc.ActionName("AttachPositions")]
        
        public ActionResult AttachPositions(int id)
        {
            var rolePositionDescription = Repository.List().FirstOrDefault(l => l.RolePositionDescId == id);

            if (rolePositionDescription == null)
            {
                var msg = MessageHelper.NotFoundMessage($"role/position description ({id})");
                throw new InvalidOperationException(msg);
            }
            return View("AttachPosition-Modal", rolePositionDescription);


        }
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("AttachPositions")]
        [ValidateAntiForgeryToken]
        public ActionResult AttachPositions()
        {
            int rolePositionDescId;
            int.TryParse(Request["RolePositionDescId"], out rolePositionDescId);
            int positionId; int.TryParse(Request["PositionId"], out positionId);

            var rolePositionDescription = Repository.List().FirstOrDefault(l => l.RolePositionDescId == rolePositionDescId);

            if (rolePositionDescription == null)
            {
                var msg = MessageHelper.NotFoundMessage($"role/position description ({rolePositionDescId})");
                throw new InvalidOperationException(msg);
            }
            ServiceRepository.PositionRepository().AttachRolePositionDescToPosition(positionId, rolePositionDescId);
            return View("AttachPosition-Modal", rolePositionDescription);
        }

        public ActionResult LoadDocNumbers(string term)
        {
            var notValidStatus = new List<int>()
            {
                (int) Enums.StatusValue.Deleted,
                (int) Enums.StatusValue.Archived,                
            };

            var list = Repository.ListHasRdOrPd().Where(ent=>!notValidStatus.Contains(ent.StatusId)).Where(l => l.DocNumber.Contains(term) || l.Title.Contains(term)).AsEnumerable().ToArray()

                 .Select(pe => new SelectListItemExtension { Value = pe.RolePositionDescId.ToString(), Text = pe.DocNumber + "|" + pe.Title })
                      .ToArray();


            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("RolePositionDescription", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var gradeItems=ServiceRepository.GradeRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.GradeCode.ToString(), Text =pe.GradeTitle})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("gradeItems",gradeItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}