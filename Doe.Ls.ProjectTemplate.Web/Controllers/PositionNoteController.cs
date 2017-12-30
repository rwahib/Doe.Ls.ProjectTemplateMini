


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
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class PositionNoteController : AppControllerBase
    {
     private PositionNoteRepository _repository=null;
     public PositionNoteRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.PositionNoteRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var positionNotes = Repository.List();
            return View(positionNotes.ToList());
        }

        //
        // GET: /PositionNote/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var positionNote = Repository.GetEntityByKey(id);
            if (positionNote == null)
            {
                throw new HttpException("The position notes were not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", positionNote);
              } else {
            return View(positionNote);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var positionNotes = Repository.List();
                IQueryable<PositionNote> displayedPositionNotes;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedPositionNotes = Repository.FilterPositionNotes(positionNotes, searchArgs);
                }
                else
                {
                    displayedPositionNotes = CustomOrderBy.CustomSort(positionNotes, arg);
                }

                var totalRecord =  displayedPositionNotes.Count();
                var totalDisplayRecord =  displayedPositionNotes.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedPositionNotes =  displayedPositionNotes.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedPositionNotes.AsEnumerable().ToArray().Select(ent => ent.To(new PositionNoteLight()));

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

         // GET: /PositionNote/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var positionNoteLight = Repository.GetEntityByKey(id).To(new PositionNoteLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = positionNoteLight;

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



        // GET: /PositionNote/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var positionNote = Repository.GetEntityByKey(id);
            if (positionNote == null)
            {
                throw new HttpException("The position notes were not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", positionNote);
              } else {
            return View(positionNote);
              }

            
        }
        //
        // GET: /PositionNote/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new PositionNote());
            } else {
            return View(new PositionNote());
            }
        }

        //
        // POST: /PositionNote/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PositionNote positionNote)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(positionNote);
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
                    return View(positionNote);
                }
                
            }
            
        }

              
        //
        // GET: /PositionNote/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var positionNote = Repository.GetEntityByKey(id);
            if (positionNote == null)
            {
                throw new HttpException("The position notes were not found");
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", positionNote);
              } else {
                  return View(positionNote);
              }
        }

        //
        // POST: /PositionNote/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PositionNote positionNote)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            ModelState.Remove("CreatedBy");
            ModelState.Remove("CreatedDate");
            ModelState.Remove("LastModifiedDate");
            var trailingList = positionNote.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            var oldPositionNote = Repository.GetEntityByEntityKey(positionNote);
            if (ModelState.IsValid)
            {
              
                var olddate = oldPositionNote.CreatedDate;
                Repository.SetPropertyValuesFrom(ref oldPositionNote,positionNote);

                try
                {
                    oldPositionNote.CreatedDate = olddate;
                    Repository.Update(oldPositionNote);     
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
                return RedirectToAction("EditMoreInfo","Position",new {id= oldPositionNote.PositionId});
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
                    return View(positionNote);
                }                
            }          
        }

        //
        // POST: /PositionNote/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(PositionNote positionNote)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldPositionNote = Repository.GetEntityByEntityKey(positionNote);
            try
            {
                Repository.Delete(oldPositionNote);     
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

            return RedirectToAction("EditMoreInfo", "Position", new { id = oldPositionNote.PositionId });
        }
        
        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("PositionNote", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var positionInformationItems=ServiceRepository.PositionInformationRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.PositionId.ToString(), Text =pe.OlderPositionNumber3})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("positionInformationItems",positionInformationItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}