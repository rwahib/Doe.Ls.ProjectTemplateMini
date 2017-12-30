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
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{


    public partial class ExecutiveController : AppControllerBase
    {
     private ExecutiveRepository _repository=null;
     public ExecutiveRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.ExecutiveRepository();

             }
 
         }

        [HasAnyAdminRole]
        public ActionResult Index()
        {
            var executives = Repository.List();
            return View(executives.ToList());
        }

        //
        // GET: /Executive/Details/5
        public ActionResult Details(string executiveCod = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var executive = Repository.GetExecutiveByCode(executiveCod);
            if (executive == null)
            {
                var msg = MessageHelper.NotFoundMessage($"divistion ({executiveCod})");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", executive);
              } else {
            return View(executive);
              }

            
        }

         public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
            var task = UserTaskFactory.GetTask(this.CurrentUser, this.Repository.RepositoryFactory);

            try
            {
                var executives = Repository.List();
                IQueryable<Executive> displayedExecutives;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch))
                {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedExecutives = Repository.FilterExecutives(executives, searchArgs);
                }
                else
                {
                    displayedExecutives = CustomOrderBy.CustomSort(executives, arg);
                }

                var totalRecord =  displayedExecutives.Count();
                var totalDisplayRecord =  displayedExecutives.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedExecutives =  displayedExecutives.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedExecutives.AsEnumerable().ToArray().Select(ent => 
                new ExecutiveLight()
                {
                    ExecutiveCod = ent.ExecutiveCod,
                    ExecutiveTitle = ent.ExecutiveTitle,
                    Privilege= task.GetExecutivePrivilege(ent.ExecutiveCod)
                }
                
                );
                

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

         // GET: /Executive/Details/5
        public ActionResult DetailsJson(string executiveCod = "")
        {
            var ajaxResult = new Result();
            
            try {
                var executiveLight = Repository.GetEntityByKey(executiveCod).To(new ExecutiveLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = executiveLight;

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



        // GET: /Executive/Delete/5
        [HasAdminOrPowerRole]        
        public ActionResult Delete(string executiveCod = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var executive = Repository.GetExecutiveByCode(executiveCod);
            if (executive == null)
            {
                var msg = MessageHelper.NotFoundMessage($"divistion ({executiveCod})");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", executive);
              } else {
            return View(executive);
              }

            
        }
        //
        // GET: /Executive/Create
        [HasAdminOrPowerRole]
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if (Request.IsAjaxRequest()) {
                return View("Create-modal", new Executive());
            } else {
            return View(new Executive());
            }
        }

        //
        // POST: /Executive/Create
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Executive executive)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);

            var trailingList =executive.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);

            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(executive);
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
                    return View(executive);
                }
                
            }
            
        }


        //
        [HasAdminOrPowerRole]
        public ActionResult Edit(string  executiveCod = "")
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var executive = Repository.GetEntityByKey(executiveCod);
            if (executive == null)
            {
                var msg = MessageHelper.NotFoundMessage($"divistion ({executiveCod})");
                throw new HttpException(msg);
            }
            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", executive);
              } else {
                  return View(executive);
              }
        }

        //
        // POST: /Executive/Edit/5
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Executive executive)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var trailingList = executive.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);

            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid)
            {
                var oldExecutive = Repository.GetEntityByEntityKey(executive);
                
                Repository.SetPropertyValuesFrom(ref oldExecutive,executive);

                try
                {
                    Repository.Update(oldExecutive);     
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
                    return View(executive);
                }                
            }          
        }

        //
        // POST: /Executive/Delete/5
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Executive executive)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldExecutive = Repository.GetEntityByEntityKey(executive);
            try
            {
                Repository.Delete(oldExecutive);     
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

        //public ActionResult GetExecutiveCodes(string executiveCode = null, bool displayNumbers = true)
        //{

            
        //    var executiveList = Repository.List().Where(l => l.ExecutiveCod == executiveCode).ToArray()
        //           .Select(l => new SelectListItemExtension
        //           {
        //               Value = l.ExecutiveCod.ToString(),
        //               Text = l.ExecutiveDescription
        //           })
        //           .ToArray();


        //    return Json(executiveList, JsonRequestBehavior.AllowGet);

        //}

        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("Executive", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
        var statusValueItems=ServiceRepository.StatusValueRepository()
                    .ActiveNotActiveList().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.StatusId.ToString(), Text =pe.StatusName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("statusValueItems",statusValueItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}