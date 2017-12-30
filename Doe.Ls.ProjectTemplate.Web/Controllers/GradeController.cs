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
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class GradeController : AppControllerBase
        {
        private GradeRepository _repository = null;
        public GradeRepository Repository
            {
            get
                {
                return _repository = _repository ?? ServiceRepository.GradeRepository();
                }
            }


        public ActionResult Index()
            {
            var grades = Repository.List();
            CreateLookups();
            return View(grades.ToList());
            }

        //
        // GET: /Grade/Details/5
        public ActionResult Details(string gradeCode = "")
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var grade = Repository.GetGradeByCode(gradeCode);
            if(grade == null)
                {
                var msg = MessageHelper.NotFoundMessage($"grade ({gradeCode})");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Details-modal", grade);
                }
            else
                {
                return View(grade);
                }

            }

        public ActionResult ListJson([FromUri] BasicArgument arg)
            {
            InitialiseArgument(arg);
            if (arg.StatusCode.HasAnyValue() && arg.StatusCode.Length == 1&& arg.StatusCode.First().Split(',').Length>1)
                {
                arg.StatusCode = arg.StatusCode.First()
                    .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

            }
            var dataTableResult = new DataTableResult();
            var task = UserTaskFactory.GetTask(this.CurrentUser, this.Repository.RepositoryFactory);
            try
                {
                var grades = Repository.List();
                IQueryable<Grade> displayedGrades;
                var filterMode = !string.IsNullOrWhiteSpace(arg.sSearch) || arg.StatusCode.HasAnyValue() ||
                                  !string.IsNullOrWhiteSpace(arg.GradeType);
                if(filterMode)
                    {
                    displayedGrades = Repository.FilterGrades(grades, arg);
                    displayedGrades = CustomOrderBy.CustomSort(displayedGrades, arg);
                    }
                else
                    {
                    displayedGrades = CustomOrderBy.CustomSort(grades, arg);
                    }

                var totalRecord = displayedGrades.Count();
                var totalDisplayRecord = displayedGrades.Count();

                if(arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedGrades = displayedGrades.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedGrades.AsEnumerable().ToArray().Select(ent =>

                {
                    var light = ent.To(new GradeLight());
                    light.StatusName = ent.StatusValue.StatusName;
                    light.Privilege = task.GetGradePrivilege(ent.GradeCode);

                    return light;
                }

                );

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

        // GET: /Grade/Details/5
        public ActionResult DetailsJson(string gradeCode = "")
            {
            var ajaxResult = new Result();

            try
                {
                var gradeLight = Repository.GetGradeByCode(gradeCode).To(new GradeLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = gradeLight;

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



        // GET: /Grade/Delete/5
        public ActionResult Delete(string gradeCode = "")
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var grade = Repository.GetGradeByCode(gradeCode);
            if(grade == null)
                {
                var msg = MessageHelper.NotFoundMessage($"grade ({gradeCode})");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Delete-modal", grade);
                }
            else
                {
                return View(grade);
                }


            }
        //
        // GET: /Grade/Create
        public ActionResult Create()
            {
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            var grade = new Grade
                {
                StatusId = Enums.StatusValue.Active.ToInteger()

                };
            if(Request.IsAjaxRequest())
                {
                return View("Create-modal", grade);
                }
            else
                {
                return View(grade);
                }
            }

        //
        // POST: /Grade/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Grade grade)
            {
            var ajaxResult = new Result();
            var trailingList = grade.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            this.ModelState.Remove("TeachingBased");
            grade.TeachingBased = Request["TeachingBased"].IsOn();

            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);

            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if(ModelState.IsValid)
                {
                try
                    {
                    Repository.Insert(grade);
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
                    return View(grade);
                    }

                }

            }


        //
        // GET: /Grade/Edit/5
        public ActionResult Edit(string gradeCode = "")
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var grade = Repository.GetGradeByCode(gradeCode);
            if(grade == null)
                {
                var msg = MessageHelper.NotFoundMessage($"grade ({gradeCode})");
                throw new HttpException(msg);
                }
            CreateLookups();

            if(Request.IsAjaxRequest())
                {
                return View("Edit-modal", grade);
                }
            else
                {
                return View(grade);
                }
            }

        //
        // POST: /Grade/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Grade grade)
            {
            var ajaxResult = new Result();
            var trailingList = grade.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);

            this.ModelState.Remove("TeachingBased");

            grade.TeachingBased = Request["TeachingBased"].IsOn();
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);

            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if(ModelState.IsValid)
                {
                var oldGrade = Repository.GetEntityByEntityKey(grade);

                Repository.SetPropertyValuesFrom(ref oldGrade, grade);

                try
                    {
                    Repository.Update(oldGrade);
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
                    return View(grade);
                    }
                }
            }

        //
        // POST: /Grade/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Grade grade)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();
            var oldGrade = Repository.GetEntityByEntityKey(grade);
            try
                {
                Repository.Delete(oldGrade);
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
        public ActionResult GetGrades(string descType)
            {
            bool type = Enums.DescriptionType.Position.ToString() == descType;

            var grades = Repository.List().Where(l => l.TeachingBased == type).ToArray()
                   .Select(l => new SelectListItemExtension
                       {
                       Value = l.GradeCode.ToString(),
                       Text = l.GradeTitle
                       })
                   .ToArray();


            return Json(grades, JsonRequestBehavior.AllowGet);

            }


        private void CreateLookups()
            {

            var enity = T4Helper.GetEntityType("Grade", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);

            var statusValueItems = ServiceRepository.StatusValueRepository()
                    .ActiveNotActiveList().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.StatusId.ToString(), Text = pe.StatusName })
                    .ToArray();

            ViewBagWrapper.ListBag.SetList("statusValueItems", statusValueItems, ViewData);

            var gradeTypeItems = Repository.GetGradeTypeList().ToArray()
                    .Select(gt => new SelectListItemExtension { Value = gt.ToString(), Text = gt.ToString() })
                    .ToArray();

            ViewBagWrapper.ListBag.SetList("gradeTypeItems", gradeTypeItems, ViewData);

            }



        protected override void Dispose(bool disposing)
            {

            base.Dispose(disposing);
            }
        }
    }