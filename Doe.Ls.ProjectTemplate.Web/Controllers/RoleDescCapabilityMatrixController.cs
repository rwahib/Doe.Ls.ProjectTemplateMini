


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
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class RoleDescCapabilityMatrixController : AppControllerBase
    {
     private RoleDescCapabilityMatrixRepository _repository=null;
     public RoleDescCapabilityMatrixRepository Repository
         {
            get {
                return _repository=_repository ?? ServiceRepository.RoleDescCapabilityMatrixRepository();

             }
         }
        public ActionResult Index()
        {
            var roleDescCapabilityMatrixs = Repository.List();
            return View(roleDescCapabilityMatrixs.ToList());
        }

        //
        // GET: /RoleDescCapabilityMatrix/Details/5
        public ActionResult Details(string gradeCode = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var roleDescCapabilityMatrix = Repository.GetEntityByKey(gradeCode);
            if (roleDescCapabilityMatrix == null)
            {
                var msg = MessageHelper.NotFoundMessage("capability comparison matrix");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", roleDescCapabilityMatrix);
              } else {
            return View(roleDescCapabilityMatrix);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
         
            try
            {
                var roleDescCapabilityMatrixs = Repository.List();
                IQueryable<RoleDescCapabilityMatrix> displayedRoleDescCapabilityMatrixs;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedRoleDescCapabilityMatrixs = Repository.FilterRoleDescCapabilityMatrixs(roleDescCapabilityMatrixs, searchArgs);
                }
                else
                {
                    displayedRoleDescCapabilityMatrixs = CustomOrderBy.CustomSort(roleDescCapabilityMatrixs, arg);
                }

                var totalRecord =  displayedRoleDescCapabilityMatrixs.Count();
                var totalDisplayRecord =  displayedRoleDescCapabilityMatrixs.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedRoleDescCapabilityMatrixs =  displayedRoleDescCapabilityMatrixs.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedRoleDescCapabilityMatrixs.AsEnumerable().ToArray().Select(ent => ent.To(new RoleDescCapabilityMatrixLight()));

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

         // GET: /RoleDescCapabilityMatrix/Details/5
        public ActionResult DetailsJson(string gradeCode = "")
        {
            var ajaxResult = new Result();
            
            try {
                var roleDescCapabilityMatrixLight = Repository.GetEntityByKey(gradeCode).To(new RoleDescCapabilityMatrixLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = roleDescCapabilityMatrixLight;

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



        // GET: /RoleDescCapabilityMatrix/Delete/5
        public ActionResult Delete(string gradeCode = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var roleDescCapabilityMatrix = Repository.GetEntityByKey(gradeCode);
            if (roleDescCapabilityMatrix == null)
            {
                var msg = MessageHelper.NotFoundMessage("capability comparison matrix");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", roleDescCapabilityMatrix);
              } else {
            return View(roleDescCapabilityMatrix);
              }

            
        }
        //
        // GET: /RoleDescCapabilityMatrix/Create
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            var grades = ServiceRepository.GradeRepository()
                   .List().Where(l => l.TeachingBased==false).ToArray()
                   .Select(pe => new SelectListItemExtension { Value = pe.GradeCode.ToString(), Text = pe.GradeCode })
                   .ToArray();
            ViewBagWrapper.ListBag.SetList("gradeItems", grades, ViewData);
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new RoleDescCapabilityMatrix());
            } else {
            return View(new RoleDescCapabilityMatrix());
            }
        }

        //
        // POST: /RoleDescCapabilityMatrix/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleDescCapabilityMatrix roleDescCapabilityMatrix)
        {
            
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(roleDescCapabilityMatrix);
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
                    return View(roleDescCapabilityMatrix);
                }
                
            }
            
        }

              
        //
        // GET: /RoleDescCapabilityMatrix/Edit/5
        public ActionResult Edit(string  gradeCode = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var roleDescCapabilityMatrix = Repository.GetEntityByKey(gradeCode);
            if (roleDescCapabilityMatrix == null)
            {
                var msg = MessageHelper.NotFoundMessage("capability comparison matrix");
                throw new HttpException(msg);
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", roleDescCapabilityMatrix);
              } else {
                  return View(roleDescCapabilityMatrix);
              }
        }

        //
        // POST: /RoleDescCapabilityMatrix/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleDescCapabilityMatrix roleDescCapabilityMatrix)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldRoleDescCapabilityMatrix = Repository.GetEntityByEntityKey(roleDescCapabilityMatrix);
                
                Repository.SetPropertyValuesFrom(ref oldRoleDescCapabilityMatrix,roleDescCapabilityMatrix);

                try
                {
                    Repository.Update(oldRoleDescCapabilityMatrix);     
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
                    return View(roleDescCapabilityMatrix);
                }                
            }          
        }

        //
        // POST: /RoleDescCapabilityMatrix/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(RoleDescCapabilityMatrix roleDescCapabilityMatrix)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldRoleDescCapabilityMatrix = Repository.GetEntityByEntityKey(roleDescCapabilityMatrix);
            try
            {
                Repository.Delete(oldRoleDescCapabilityMatrix);     
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
        
        var enity = T4Helper.GetEntityType("RoleDescCapabilityMatrix", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}