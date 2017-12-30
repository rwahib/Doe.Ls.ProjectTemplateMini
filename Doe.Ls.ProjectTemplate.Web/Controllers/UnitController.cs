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
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {


    public partial class UnitController : AppControllerBase
        {
        private UnitRepository _repository = null;
        public UnitRepository Repository
            {
            get
                {

                return _repository = _repository ?? ServiceRepository.UnitRepository();

                }

            }


        [HasAnyAdminRole]
        public ActionResult Index()
        {
            CreateLookups();
            var units = Repository.List();
            return View(units.ToList());
            }

        //
        // GET: /Unit/Details/5
        public ActionResult Details(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var unit = Repository.GetUnitById(id);
            if(unit == null)
                {
                var msg = MessageHelper.NotFoundMessage($"team ({id})");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Details-modal", unit);
                }
            else
                {
                return View(unit);
                }


            }

        public ActionResult ListJson([FromUri] BasicStructureArgument arg)
            {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
            var task = UserTaskFactory.GetTask(this.CurrentUser, this.Repository.RepositoryFactory);
            try
                {
                var units = Repository.List();
                IQueryable<Unit> displayedUnits;

                if(!string.IsNullOrWhiteSpace(arg.sSearch) || arg.DirectorateId > 0 || !string.IsNullOrWhiteSpace(arg.DivisionCode) || arg.BusinessUnitId > 0)
                    {                    
                    displayedUnits = Repository.FilterUnits(units, arg);
                    displayedUnits = CustomOrderBy.CustomSort(displayedUnits, arg);
                    }
                else
                    {
                    displayedUnits = CustomOrderBy.CustomSort(units, arg);
                    }

                var totalRecord = displayedUnits.Count();
                var totalDisplayRecord = displayedUnits.Count();

                if(arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedUnits = displayedUnits.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedUnits.AsEnumerable().ToArray().Select(ent =>
                {
                    var unit = ent.To(new UnitLight());
                    unit.ExecutiveTitle = ent.BusinessUnit.Directorate.Executive.ExecutiveTitle;
                    unit.ExecutiveCode = ent.BusinessUnit.Directorate.ExecutiveCod;

                    unit.DirectorateName = ent.BusinessUnit.Directorate.DirectorateName;
                    unit.DirectorateId = ent.BusinessUnit.DirectorateId;

                    unit.BUnitName = ent.BusinessUnit.BUnitName;

                    unit.AreanName = ent.FunctionalArea.AreanName;
                    unit.FuncationalAreaId = ent.FunctionalArea.FuncationalAreaId;

                    unit.TeamTypeName = ent.TeamType.TeamTypeName;

                    unit.StatusName = ent.StatusValue.StatusName;
                    unit.HierarchyName = ent.HierarchyLevel.HierarchyName;
                    

                    unit.Privilege = task.GetTeamPrivilege(ent);
                    return unit;
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

        // GET: /Unit/Details/5
        public ActionResult DetailsJson(int id = 0)
            {
            var ajaxResult = new Result();

            try
                {
                var unitLight = Repository.GetUnitById(id).To(new UnitLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = unitLight;

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
        public ActionResult Delete(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var unit = Repository.GetUnitById(id);
            if(unit == null)
                {
                var msg = MessageHelper.NotFoundMessage($"team ({id})");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Delete-modal", unit);
                }
            else
                {
                return View(unit);
                }

            }
        //
        [HasAdminOrPowerRole]
        public ActionResult Create(string divisionCode, int? directorateId,int? bUnitId,int? functionalAreaId)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            if(!string.IsNullOrWhiteSpace(divisionCode))
                {
                ViewBagWrapper.SetGeneralObject("divisionCode", divisionCode, ViewData);
                }
            if(directorateId.HasValue && directorateId.Value > 0)
                {
                var directorate = this.ServiceRepository.DirectorateRepository().GetDirectorateById(directorateId.Value);
                divisionCode = directorate.ExecutiveCod;
                ViewBagWrapper.SetGeneralObject("divisionCode", divisionCode, ViewData);
                ViewBagWrapper.SetGeneralObject("directorateId", directorateId.Value, ViewData);
                }
            if(bUnitId.HasValue && bUnitId.Value > 0)
            {
                var bUnit = this.ServiceRepository.BusinessUnitRepository().GetBUnitById(bUnitId.Value);
                ViewBagWrapper.SetGeneralObject("divisionCode", bUnit.Directorate.ExecutiveCod, ViewData);
                ViewBagWrapper.SetGeneralObject("directorateId", bUnit.DirectorateId, ViewData);
                ViewBagWrapper.SetGeneralObject("bUnitId", bUnit.BUnitId, ViewData);
                }
            CreateLookups();
            
            CreateLookups();
            var unit = new Unit
            {
                UnitId = Repository.GetMaxKeyValue()+ 10,
                BUnitId = bUnitId??0,
                FunctionalAreaId = functionalAreaId??0,
                TeamTypeId = Enums.TeamType.Regular.ToInteger(),
                HierarchyId = Enums.HierarchyLevel.Team.ToInteger(),
                StatusId = Enums.StatusValue.Active.ToInteger()
                
                };
            if(Request.IsAjaxRequest())
                {
                return View("Create-modal", unit);
                }
            else
                {
                return View(unit);
                }
            }

        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Unit unit)
            {            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var trailingList = unit.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if(ModelState.IsValid)
                {
                try
                    {
                    Repository.Insert(unit);
                    }
                catch(Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("DB", msg + exception.Message));
                    //errors.Add(new DbValidationError("DB", "Oops! something went wrong. " + exception.Message));

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
                    return View(unit);
                    }

                }

            }


        //
        [HasAdminOrPowerRole]
        public ActionResult Edit(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var unit = Repository.GetUnitById(id);
            if(unit == null)
                {
                var msg = MessageHelper.NotFoundMessage($"team ({id})");
                throw new HttpException(msg);
                }
            ViewBagWrapper.SetGeneralObject("divisionCode", unit.BusinessUnit.Directorate.ExecutiveCod, ViewData);
            ViewBagWrapper.SetGeneralObject("directorateId", unit.BusinessUnit.DirectorateId, ViewData);
            
            CreateLookups();

            if(Request.IsAjaxRequest())
                {
                return View("Edit-modal", unit);
                }
            else
                {
                return View(unit);
                }
            }

        //
        // POST: /Unit/Edit/5
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Unit unit)
            {

            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var trailingList = unit.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if(ModelState.IsValid)
                {
                var oldUnit = Repository.GetEntityByEntityKey(unit);

                Repository.SetPropertyValuesFrom(ref oldUnit, unit);

                try
                    {
                    Repository.Update(oldUnit);
                    }
                catch(Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("DB", msg + exception.Message));
                    //errors.Add(new DbValidationError("DB", "Oops! something went wrong. " + exception.Message));

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
                    return View(unit);
                    }
                }
            }

        //
        // POST: /Unit/Delete/5
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Unit unit)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();
            var oldUnit = Repository.GetEntityByEntityKey(unit);
            try
                {
                Repository.Delete(oldUnit);
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

            var enity = T4Helper.GetEntityType("Unit", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);

            var divisionCode = ViewBagWrapper.GetGeneralObject<string>("divisionCode", ViewData);
            var directorateId = ViewBagWrapper.GetGeneralObject<int?>("directorateId", ViewData);
            
            var divisionItems = ServiceRepository.ExecutiveRepository()
                      .List().ToArray()
                      .Select(pe => new SelectListItemExtension { Value = pe.ExecutiveCod.ToString(), Text = pe.ExecutiveTitle })
                      .ToArray();

            ViewBagWrapper.ListBag.SetList("divisionItems", divisionItems, ViewData);


           

            var directorateList = ServiceRepository.DirectorateRepository()
                    .List();
            if(!string.IsNullOrWhiteSpace(divisionCode)) directorateList = directorateList.Where(d => d.ExecutiveCod == divisionCode);
            var directorateItems = directorateList.ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.DirectorateId.ToString(), Text = pe.DirectorateName })
                    .ToArray();

            ViewBagWrapper.ListBag.SetList("directorateItems", directorateItems, ViewData);


            var businessUnitList = ServiceRepository.BusinessUnitRepository()
                         .List();
            if(!string.IsNullOrWhiteSpace(divisionCode)) businessUnitList = businessUnitList.Where(b => b.Directorate.ExecutiveCod == divisionCode);
            if(directorateId>0) businessUnitList = businessUnitList.Where(b =>b.DirectorateId==directorateId.Value);

            var businessUnitItems = businessUnitList.ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.BUnitId.ToString(), Text = pe.BUnitName })
                    .ToArray();

            ViewBagWrapper.ListBag.SetList("businessUnitItems", businessUnitItems, ViewData);


            var functionalArealist = ServiceRepository.FunctionalAreaRepository()
                         .List();
            if(!string.IsNullOrWhiteSpace(divisionCode)) functionalArealist = functionalArealist.Where(b => b.Directorate.ExecutiveCod == divisionCode);
            if(directorateId > 0) functionalArealist = functionalArealist.Where(b => b.DirectorateId == directorateId.Value);
            var functionalAreaItems = functionalArealist.ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.FuncationalAreaId.ToString(), Text = pe.AreanName })
                    .ToArray();

            ViewBagWrapper.ListBag.SetList("functionalAreaItems", functionalAreaItems, ViewData);


            var hierarchyLevelItems = ServiceRepository.HierarchyLevelRepository()
                      .List().ToArray()
                      .Select(pe => new SelectListItemExtension { Value = pe.HierarchyId.ToString(), Text = pe.HierarchyName })
                      .ToArray();

            ViewBagWrapper.ListBag.SetList("hierarchyLevelItems", hierarchyLevelItems, ViewData);


            var teamTypeItems = ServiceRepository.TeamTypeRepository()
                      .List().ToArray()
                      .Select(pe => new SelectListItemExtension { Value = pe.TeamTypeId.ToString(), Text = pe.TeamTypeName })
                      .ToArray();

            ViewBagWrapper.ListBag.SetList("teamTypeItems", teamTypeItems, ViewData);


            var statusValueItems = ServiceRepository.StatusValueRepository()
                      .ActiveNotActiveList().ToArray()
                      .Select(pe => new SelectListItemExtension { Value = pe.StatusId.ToString(), Text = pe.StatusName })
                      .ToArray();

            ViewBagWrapper.ListBag.SetList("statusValueItems", statusValueItems, ViewData);


            }

        protected override void Dispose(bool disposing)
            {

            base.Dispose(disposing);
            }
        }
    }