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
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {
    public partial class WfActionController : AppControllerBase
        {
        private WfActionRepository _repository = null;
        public WfActionRepository Repository
            {
            get
                {

                return _repository = _repository ?? ServiceRepository.WfActionRepository();

                }

            }

        [HasAnyAdminRole]
        public ActionResult Index()
            {
            var wfActions = Repository.List();
            return View(wfActions.ToList());
            }

        //
        // GET: /WfAction/Details/5

        [HasAnyAdminRole]
        public ActionResult Details(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var wfAction = Repository.GetEntityByKey(id);
            if(wfAction == null)
                {
                var msg = MessageHelper.NotFoundMessage("workflow action");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Details-modal", wfAction);
                }
            else
                {
                return View(wfAction);
                }


            }

        public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg)
            {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();

            try
                {
                var wfActions = Repository.List();
                IQueryable<WfAction> displayedWfActions;

                if(!string.IsNullOrWhiteSpace(arg.sSearch))
                    {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedWfActions = Repository.FilterWfActions(wfActions, searchArgs);
                    }
                else
                    {
                    displayedWfActions = CustomOrderBy.CustomSort(wfActions, arg);
                    }

                var totalRecord = displayedWfActions.Count();
                var totalDisplayRecord = displayedWfActions.Count();

                if(arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedWfActions = displayedWfActions.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedWfActions.AsEnumerable().ToArray().Select(ent => ent.To(new WfActionLight()));

                dataTableResult.sEcho = arg.sEcho;
                dataTableResult.iTotalRecords = totalRecord;
                dataTableResult.iTotalDisplayRecords = totalDisplayRecord;
                dataTableResult.aaData = result;
                dataTableResult.Status = Status.Success;
                dataTableResult.Message = "Success";
                }
            catch(Exception exception)
                {
                LogException(exception);

                dataTableResult.Status = Status.Error;
                dataTableResult.Message = "Errors";
                var msg = MessageHelper.ErrorOccured();
                dataTableResult.AddError(new DbValidationError("DB error", msg + exception.Message));
                //dataTableResult.AddError(new DbValidationError("DB error",
                //    "Oops! something went wrong. " + exception.Message));
            }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
            }

        // GET: /WfAction/Details/5

        public ActionResult DetailsJson(int id = 0)
            {
            var ajaxResult = new Result();

            try
                {
                var wfActionLight = Repository.GetEntityByKey(id).To(new WfActionLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = wfActionLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
                }
            catch(Exception exception)
                {
                LogException(exception);
                ajaxResult.Status = Status.Error;
                ajaxResult.Message = "Errors";
                var msg = MessageHelper.ErrorOccured();
                ajaxResult.AddError(new DbValidationError("DB ", msg + exception.Message));
                //ajaxResult.AddError(new DbValidationError("DB ",
                //    "Oops! something went wrong. " + exception.Message));
                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
                }

            }

        // GET: /WfAction/Delete/5
        [HasSysAdminRole]
        public ActionResult Delete(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var wfAction = Repository.GetEntityByKey(id);
            if(wfAction == null)
                {
                var msg = MessageHelper.NotFoundMessage("workflow action");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Delete-modal", wfAction);
                }
            else
                {
                return View(wfAction);
                }


            }
        //
        [HasSysAdminRole]

        public ActionResult Create()
            {
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            if(Request.IsAjaxRequest())
                {
                return View("Create-modal", new WfAction());
                }
            else
                {
                return View(new WfAction());
                }
            }

        //
        // POST: /WfAction/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasSysAdminRole]
        public ActionResult Create(WfAction wfAction)
            {


            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if(ModelState.IsValid)
                {
                try
                    {
                    Repository.Insert(wfAction);
                    }
                catch(Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("DB error", msg + exception.Message));
                    //errors.Add(new DbValidationError("DB error", "Oops! something went wrong. " + exception.Message));

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
                if(Request.IsAjaxRequest())
                    {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";
                    return Json(ajaxResult);
                    }
                return RedirectToAction("Index");

                }
            else
                {
                if(Request.IsAjaxRequest())
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
                    return View(wfAction);
                    }

                }

            }


        //
        // GET: /WfAction/Edit/5

        [HasAdminOrPowerRole]
        public ActionResult Edit(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var wfAction = Repository.GetEntityByKey(id);
            if(wfAction == null)
                {
                var msg = MessageHelper.NotFoundMessage("workflow action");
                throw new HttpException(msg);
                }
            CreateLookups();

            if(Request.IsAjaxRequest())
                {
                return View("Edit-modal", wfAction);
                }
            else
                {
                return View(wfAction);
                }
            }

        //
        // POST: /WfAction/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAdminOrPowerRole]
        public ActionResult Edit(WfAction wfAction)
            {

            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if(ModelState.IsValid)
                {
                var oldWfAction = Repository.GetEntityByEntityKey(wfAction);

                Repository.SetPropertyValuesFrom(ref oldWfAction, wfAction);

                try
                    {
                    Repository.Update(oldWfAction);
                    }
                catch(Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("DB error", msg + exception.Message));
                    //errors.Add(new DbValidationError("DB error", "Oops! Oops! something went wrong. " + exception.Message));

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

                if(Request.IsAjaxRequest())
                    {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";

                    return Json(ajaxResult);
                    }
                return RedirectToAction("Index");
                }
            else
                {
                if(Request.IsAjaxRequest())
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
                    return View(wfAction);
                    }
                }
            }

        //
        // POST: /WfAction/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasSysAdminRole]
        public ActionResult DeleteConfirmed(WfAction wfAction)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();
            var oldWfAction = Repository.GetEntityByEntityKey(wfAction);
            try
                {
                Repository.Delete(oldWfAction);
                }
            catch(Exception exception)
                {
                LogException(exception);

                var errors = Repository.GetBackendValidationErrors().ToList();

                errors.Add(new DbValidationError("", string.Format(GetMessage("ERR-GENERAL").MessageFormat, exception.Message)));
                errors.Add(new DbValidationError("", GetMessage("ERR-GENERAL-DELETE-HAS-CHILDS").MessageFormat));

                if((Request.IsAjaxRequest()))
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

            if(Request.IsAjaxRequest())
                {
                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";

                return Json(ajaxResult);
                }

            return RedirectToAction("Index");
            }

        private void CreateLookups()
            {

            var enity = T4Helper.GetEntityType("WfAction", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);

            }



        protected override void Dispose(bool disposing)
            {

            base.Dispose(disposing);
            }
        }
    }