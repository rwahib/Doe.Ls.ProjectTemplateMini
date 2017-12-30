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
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class PositionLevelController : AppControllerBase
        {
        private PositionLevelRepository _repository = null;
        public PositionLevelRepository Repository
            {
            get
                {

                return _repository = _repository ?? ServiceRepository.PositionLevelRepository();

                }

            }


        public ActionResult Index()
            {
            var positionLevels = Repository.List();
            return View(positionLevels.ToList());
            }

        //
        // GET: /PositionLevel/Details/5
        public ActionResult Details(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var positionLevel = Repository.GetPositionLevelById(id);
            if(positionLevel == null)
                {
                var msg = MessageHelper.NotFoundMessage("position level");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Details-modal", positionLevel);
                }
            else
                {
                return View(positionLevel);
                }


            }

        public ActionResult ListJson([FromUri] JQueryDataTableParamModel arg)
            {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();

            try
                {
                var positionLevels = Repository.List();
                IQueryable<PositionLevel> displayedPositionLevels;

                if(!string.IsNullOrWhiteSpace(arg.sSearch))
                    {
                    var searchArgs = new SearchArg { Search = arg.sSearch };
                    displayedPositionLevels = Repository.FilterPositionLevels(positionLevels, searchArgs);
                    }
                else
                    {
                    displayedPositionLevels = CustomOrderBy.CustomSort(positionLevels, arg);
                    }

                var totalRecord = displayedPositionLevels.Count();
                var totalDisplayRecord = displayedPositionLevels.Count();

                if(arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedPositionLevels = displayedPositionLevels.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedPositionLevels.AsEnumerable().ToArray().Select(ent => ent.To(new PositionLevelLight()));

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
                dataTableResult.AddError(new DbValidationError("Error", msg + exception.Message));
                //dataTableResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
            }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
            }

        // GET: /PositionLevel/Details/5
        public ActionResult DetailsJson(int id = 0)
            {
            var ajaxResult = new Result();

            try
                {
                var positionLevelLight = Repository.GetPositionLevelById(id).To(new PositionLevelLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = positionLevelLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
                }
            catch(Exception exception)
                {
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


        [HasAdminOrPowerRole]
        // GET: /PositionLevel/Delete/5
        public ActionResult Delete(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var positionLevel = Repository.GetPositionLevelById(id);
            if(positionLevel == null)
                {
                var msg = MessageHelper.NotFoundMessage("position level");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Delete-modal", positionLevel);
                }
            else
                {
                return View(positionLevel);
                }


            }
        //
        // GET: /PositionLevel/Create
        [HasAdminOrPowerRole]
        public ActionResult Create()
            {
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            var model = new PositionLevel()
                {
                PositionLevelId = Repository.GetMaxKeyValue() + 10
                };
            if(Request.IsAjaxRequest())
                {
                return View("Create-modal", model);
                }
            else
                {
                return View(model);
                }
            }


        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAdminOrPowerRole]
        public ActionResult Create(PositionLevel positionLevel)
            {


            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if(ModelState.IsValid)
                {
                try
                    {
                    Repository.Insert(positionLevel);
                    }
                catch(Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error", "Oops! something went wrong. " + exception.Message));

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
                    return View(positionLevel);
                    }

                }

            }


        //
        // GET: /PositionLevel/Edit/5
        [HasAdminOrPowerRole]
        public ActionResult Edit(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var positionLevel = Repository.GetPositionLevelById(id);
            if(positionLevel == null)
                {
                var msg = MessageHelper.NotFoundMessage("position level");
                throw new HttpException(msg);
                }
            CreateLookups();

            if(Request.IsAjaxRequest())
                {
                return View("Edit-modal", positionLevel);
                }
            else
                {
                return View(positionLevel);
                }
            }

        //
        // POST: /PositionLevel/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAdminOrPowerRole]
        public ActionResult Edit(PositionLevel positionLevel)
            {

            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            
             var trailingList = positionLevel.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if(ModelState.IsValid)
                {
                var oldPositionLevel = Repository.GetEntityByEntityKey(positionLevel);

                Repository.SetPropertyValuesFrom(ref oldPositionLevel, positionLevel,false);

                try
                    {
                    Repository.Update(oldPositionLevel);
                    }
                catch(Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("Error", msg + exception.Message));
                    //errors.Add(new DbValidationError("Error", "Oops! something went wrong. " + exception.Message));

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
                    return View(positionLevel);
                    }
                }
            }

        //
        // POST: /PositionLevel/Delete/5
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(PositionLevel positionLevel)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();
            var oldPositionLevel = Repository.GetEntityByEntityKey(positionLevel);
            try
                {
                Repository.Delete(oldPositionLevel);
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

            var enity = T4Helper.GetEntityType("PositionLevel", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);

            }



        protected override void Dispose(bool disposing)
            {

            base.Dispose(disposing);
            }
        }
    }