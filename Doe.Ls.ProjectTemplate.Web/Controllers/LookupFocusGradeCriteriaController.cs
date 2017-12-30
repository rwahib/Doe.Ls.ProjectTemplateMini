


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
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class LookupFocusGradeCriteriaController : AppControllerBase
    {
     private LookupFocusGradeCriteriaRepository _repository=null;
     public LookupFocusGradeCriteriaRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.LookupFocusGradeCriteriaRepository();

             }
 
         }
                

        public ActionResult Index()
        {
            var lookupFocusGradeCriterias = Repository.List();
            var gradeItems = ServiceRepository.GradeRepository().ListForPositiondesc().ToArray().Select(pe => new SelectListItemExtension { Value = pe.GradeCode.ToString(), Text = pe.GradeTitle })
                    .ToArray();
            ViewBagWrapper.ListBag.SetList("gradeItems", gradeItems, ViewData);
            var focusItems = ServiceRepository.FocusRepository().List().ToArray().Select(pe => new SelectListItemExtension { Value = pe.FocusId.ToString(), Text = pe.FocusName })
                    .ToArray();
            ;ViewBagWrapper.ListBag.SetList("focusItems", focusItems, ViewData);
            return View(lookupFocusGradeCriterias.ToList());
        }

        //
        // GET: /LookupFocusGradeCriteria/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var lookupFocusGradeCriteria = Repository.List().FirstOrDefault(l=>l.LookupId==id);
            if (lookupFocusGradeCriteria == null)
            {
                var msg = MessageHelper.NotFoundMessage("lookup Focus Grade Criteria");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", lookupFocusGradeCriteria);
              } else {
            return View(lookupFocusGradeCriteria);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableFocusCriteria arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var lookupFocusGradeCriterias = Repository.List();
                IQueryable<LookupFocusGradeCriteria> displayedLookupFocusGradeCriterias;

                if (!string.IsNullOrEmpty(arg.GradeCode))
                {
                    lookupFocusGradeCriterias = lookupFocusGradeCriterias.Where(l => l.GradeCode==arg.GradeCode);
                }
                if (arg.FocusId>0)
                {
                    lookupFocusGradeCriterias = lookupFocusGradeCriterias.Where(l => l.FocusId == arg.FocusId);
                }
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedLookupFocusGradeCriterias = Repository.FilterLookupFocusGradeCriterias(lookupFocusGradeCriterias, searchArgs);
                }
                else
                {
                    displayedLookupFocusGradeCriterias = CustomOrderBy.CustomSort(lookupFocusGradeCriterias, arg);
                }

                var totalRecord =  displayedLookupFocusGradeCriterias.Count();
                var totalDisplayRecord =  displayedLookupFocusGradeCriterias.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedLookupFocusGradeCriterias =  displayedLookupFocusGradeCriterias.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedLookupFocusGradeCriterias.AsEnumerable().ToArray().Select(ent => ent.To(new LookupFocusGradeCriteriaLight()
                {
                    Criteria=ent.SelectionCriteria.Criteria,
                    FocusName = ent.Focus.FocusName,
                    GradeName = ent.Grade.GradeTitle
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

         // GET: /LookupFocusGradeCriteria/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var lookupFocusGradeCriteriaLight = Repository.GetEntityByKey(id).To(new LookupFocusGradeCriteriaLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = lookupFocusGradeCriteriaLight;

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



        // GET: /LookupFocusGradeCriteria/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var lookupFocusGradeCriteria = Repository.GetEntityByKey(id);
            if (lookupFocusGradeCriteria == null)
            {
                var msg = MessageHelper.NotFoundMessage("lookup Focus Grade Criteria");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", lookupFocusGradeCriteria);
              } else {
            return View(lookupFocusGradeCriteria);
              }

            
        }
        //
        // GET: /LookupFocusGradeCriteria/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new LookupFocusGradeCriteria());
            } else {
            return View(new LookupFocusGradeCriteria());
            }
        }

        //
        // POST: /LookupFocusGradeCriteria/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LookupFocusGradeCriteria lookupFocusGradeCriteria)
        {
            ModelState.Remove("LastUpdatedBy");
            ModelState.Remove("IsMandatory");
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    var isMandatory = Request["IsMandatory"] == "on";
                    lookupFocusGradeCriteria.IsMandatory = isMandatory;
                    Repository.Insert(lookupFocusGradeCriteria);
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
                    return View(lookupFocusGradeCriteria);
                }
                
            }
            
        }

              
        //
        // GET: /LookupFocusGradeCriteria/Edit/5
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var lookupFocusGradeCriteria = Repository.GetEntityByKey(id);
            if (lookupFocusGradeCriteria == null)
            {
                var msg = MessageHelper.NotFoundMessage("lookup Focus Grade Criteria");
                throw new HttpException(msg);
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", lookupFocusGradeCriteria);
              } else {
                  return View(lookupFocusGradeCriteria);
              }
        }

        //
        // POST: /LookupFocusGradeCriteria/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LookupFocusGradeCriteria lookupFocusGradeCriteria)
        {
            ModelState.Remove("LastUpdatedBy");
            ModelState.Remove("IsMandatory");
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
             var trailingList = lookupFocusGradeCriteria.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldLookupFocusGradeCriteria = Repository.GetEntityByEntityKey(lookupFocusGradeCriteria);
                
               // Repository.SetPropertyValuesFrom(ref oldLookupFocusGradeCriteria,lookupFocusGradeCriteria);

                try
                {
                    var isMandatory = Request["IsMandatory"] == "on";
                    oldLookupFocusGradeCriteria.IsMandatory = isMandatory;
                    Repository.Update(oldLookupFocusGradeCriteria);     
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
                    return View(lookupFocusGradeCriteria);
                }                
            }          
        }

        //
        // POST: /LookupFocusGradeCriteria/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(LookupFocusGradeCriteria lookupFocusGradeCriteria)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldLookupFocusGradeCriteria = Repository.GetEntityByEntityKey(lookupFocusGradeCriteria);
            try
            {
                Repository.Delete(oldLookupFocusGradeCriteria);     
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
        public ActionResult GetFocusList(string gradeCode)
        {

            var list = Repository.LoadFocusListNotInLookup(gradeCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCriteriaList(string gradeCode,int focusId)
        {

            var list = Repository.LoadCriteriaListByFocusAndGrade(gradeCode,focusId);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("LookupFocusGradeCriteria", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);

            var focusItems = ServiceRepository.FocusRepository()
                        .List().ToArray()
                        .Select(pe => new SelectListItemExtension { Value = pe.FocusId.ToString(), Text = pe.FocusName })
                        .ToArray();

            ViewBagWrapper.ListBag.SetList("focusItems", focusItems, ViewData);


            var gradeItems = ServiceRepository.GradeRepository()
                      .List().Where(g=>g.TeachingBased == true )
                      .Select(pe => new SelectListItemExtension { Value = pe.GradeCode.ToString(), Text = pe.GradeTitle })
                      .ToArray();
            ViewBagWrapper.ListBag.SetList("gradeItems",gradeItems,ViewData);
                

          var selectionCriteriaItems=ServiceRepository.SelectionCriteriaRepository()
                    .List().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.SelectionCriteriaId.ToString(), Text = pe.Criteria })
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("selectionCriteriaItems",selectionCriteriaItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}