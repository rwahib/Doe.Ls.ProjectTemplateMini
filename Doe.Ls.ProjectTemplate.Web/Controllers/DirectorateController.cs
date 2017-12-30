using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;


namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    public partial class DirectorateController : AppControllerBase
    {
        private DirectorateRepository _repository = null;
        public DirectorateRepository Repository
        {
            get
            {

                return _repository = _repository ?? ServiceRepository.DirectorateRepository();

            }

        }

        [HasAnyAdminRole]
        public ActionResult Index()
        {
            var directorates = Repository.List();
            this.CreateLookups();
            return View(directorates.ToList());
        }

        //
        // GET: /Directorate/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var directorate = Repository.GetDirectorateById(id);
            if (directorate == null)
            {
                var msg = MessageHelper.NotFoundMessage("directorate");
                throw new HttpException(msg);
            }
            if (Request.IsAjaxRequest())
            {
                return View("Details-modal", directorate);
            }
            else
            {
                return View(directorate);
            }


        }

        public ActionResult ListJson([FromUri] BasicStructureArgument arg)
        {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
            var task = UserTaskFactory.GetTask(this.CurrentUser, this.Repository.RepositoryFactory);

            try
            {
                var directorates = Repository.List();
                IQueryable<Directorate> displayedDirectorates;

                if (!string.IsNullOrWhiteSpace(arg.sSearch) || !string.IsNullOrWhiteSpace(arg.DivisionCode))
                {

                    displayedDirectorates = Repository.FilterDirectorates(directorates, arg);

                    displayedDirectorates = CustomOrderBy.CustomSort(displayedDirectorates, arg.SortColumnName, arg.SortColumnDesc);
                }
                else
                {
                    displayedDirectorates = CustomOrderBy.CustomSort(directorates, arg.SortColumnName, arg.SortColumnDesc);
                }

                var totalRecord = displayedDirectorates.Count();
                var totalDisplayRecord = displayedDirectorates.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedDirectorates = displayedDirectorates.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedDirectorates.AsEnumerable().ToArray().Select(ent =>
                {
                    var light = ent.To(new DirectorateLight());
                    light.ExecutiveTitle = ent.Executive.ExecutiveTitle;
                    light.Status = ent.StatusValue.StatusName;
                    light.Privilege = task.GetDirectoratePrivilege(light.DirectorateId);
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

        // GET: /Directorate/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();

            try
            {
                var directorateLight = Repository.GetEntityByKey(id).To(new DirectorateLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = directorateLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
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

            var directorate = Repository.GetDirectorateById(id);
            if (directorate == null)
            {
                var msg = MessageHelper.NotFoundMessage("directorate");
                throw new HttpException(msg);
            }
            if (Request.IsAjaxRequest())
            {
                return View("Delete-modal", directorate);
            }
            else
            {
                return View(directorate);
            }


        }

        [HasAdminOrPowerRole]
        public ActionResult Create(string executiveCode)
        {
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            CreateLookups();
            var model = new Directorate
            {
                DirectorateId = Repository.GetMaxKeyValue() + 10,
                ExecutiveCod = executiveCode,
                StatusId = Enums.StatusValue.Active.ToInteger()
            };
            if (Request.IsAjaxRequest())
            {
                return View("Create-modal", model);
            }
            else
            {
                return View(model);
            }
        }

        //
        // POST: /Directorate/Create
        [HasAdminOrPowerRole]

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Directorate directorate)
        {

            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var trailingList = directorate.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);

            var ajaxResult = new Result();
            ClearModelState();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    var selectedLocations = Request["LocationId"];

                    string[] delimiter1 = { "," };
                   
                    string[] sectors = selectedLocations.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries);

                    if (directorate != null)
                    {
                        List<Location> locationList = BuildLocationList(directorate, sectors);
                        directorate.Locations = locationList;
                        //save directorate and its locations
                        Repository.Insert(directorate);
                    }
                }
                catch (Exception exception)
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
                    return View(directorate);
                }

            }

        }

       
        [HasAdminOrPowerRole]
        public ActionResult Edit(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var directorate = Repository.GetDirectorateById(id);

            if (directorate == null)
            {
                var msg = MessageHelper.NotFoundMessage("directorate");
                throw new HttpException(msg);
            }
            CreateLookups(id);

            if (Request.IsAjaxRequest())
            {
                return View("Edit-modal", directorate);
            }
            else
            {
                return View(directorate);
            }
        }

        //
        // POST: /Directorate/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAdminOrPowerRole]
        public ActionResult Edit(Directorate directorate)
        {

            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var trailingList = directorate.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);

            ClearModelState();
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if (ModelState.IsValid)
            {
                var oldDirectorate = Repository.GetEntityByEntityKey(directorate);

                Repository.SetPropertyValuesFrom(ref oldDirectorate, directorate);

                try
                {

                    var selectedLocations = Request["LocationId"];

                    string[] delimiter1 = { "," };

                    string[] sectors = selectedLocations.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries);

                    List<Location> locationList = BuildLocationList(directorate, sectors);
                    oldDirectorate.Locations = locationList;
                    //save directorate and its locations
                    Repository.Update(oldDirectorate);

                   

                }
                catch (Exception exception)
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    errors.Add(new DbValidationError("Error", "Oops! something went wrong  " + exception.Message));

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
                    return View(directorate);
                }
            }
        }


        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Directorate directorate)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();
            var oldDirectorate = Repository.GetDirectorateById(directorate.DirectorateId);
            try
            {
                Repository.Delete(oldDirectorate);
            }
            catch (Exception exception)
            {
                LogException(exception);
                
                var errors = Repository.GetBackendValidationErrors().ToList();
                errors.Add(new DbValidationError("",string.Format(GetMessage("ERR-GENERAL").MessageFormat,exception.Message) ));
                errors.Add(new DbValidationError("",GetMessage("ERR-GENERAL-DELETE-HAS-CHILDS").MessageFormat));

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
        public ActionResult GetDirectorates(string divisionCode = null, bool displayNumbers = true,bool currentUser=false)
            {

            var liveStatus = new int[] { Enums.StatusValue.Approved.ToInteger(), Enums.StatusValue.Imported.ToInteger() };
            //   bu.Units.SelectMany(u=>u.Positions.Where(p=> liveStatus.Contains(p.StatusId))).Count()
            var list =
                Repository.BasicList()
                    .Where(d => d.ExecutiveCod == divisionCode)
                    .Include(d => d.BusinessUnits)
                    .Include("BusinessUnits.Units.Positions").ToList();
            if(currentUser) list = list.FilterDefaults(this.CurrentUser).ToList();
            var directorateList = list
               .Select(dir => new SelectListItemExtension
               {
                   Value = dir.DirectorateId.ToString(),
                   Text = displayNumbers ? $"{dir.DirectorateName}({dir.BusinessUnits.SelectMany(b => b.Units.SelectMany(u => u.Positions)).Count(p => liveStatus.Contains(p.StatusId))})" : $"{dir.DirectorateName}"
               })
               .ToArray();


            return Json(directorateList, JsonRequestBehavior.AllowGet);

        }

        private void CreateLookups(int directorateId = 0)
        {

            var enity = T4Helper.GetEntityType("Directorate", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);

            var divisionItems = ServiceRepository.ExecutiveRepository()
                        .List().ToArray()
                        .Select(pe => new SelectListItemExtension { Value = pe.ExecutiveCod.ToString(), Text = pe.ExecutiveTitle })
                        .ToArray();

            ViewBagWrapper.ListBag.SetList("divisionItems", divisionItems, ViewData);

            var statusValueItems = ServiceRepository.StatusValueRepository()
                    .ActiveNotActiveList().ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.StatusId.ToString(), Text = pe.StatusName })
                    .ToArray();

            ViewBagWrapper.ListBag.SetList("statusValueItems", statusValueItems, ViewData);

            if (directorateId > 0)
            {
                var selected = ServiceRepository.LocationRepository().LocationListByDirectorate(directorateId);
                
                var locationItems = ServiceRepository.LocationRepository().DistinctList()
                    .ToArray()
                .Select(lo => new SelectListItemExtension { Value = lo.Name, Text = lo.Name,
                    Selected = (selected.Any(x=>x.Name == lo.Name))
                })
                .ToArray();
                ViewBagWrapper.ListBag.SetList("locationItems", locationItems, ViewData);
            }
            else
            {
                var locationItems = ServiceRepository.LocationRepository().DistinctList()
                .Select(lo => new SelectListItemExtension
                {
                    Value = lo.Name,
                    Text = lo.Name
                })
                .ToArray();
                ViewBagWrapper.ListBag.SetList("locationItems", locationItems, ViewData);
            }

        }


        private List<Location> BuildLocationList(Directorate directorate, string[] sectors)
        {
            List<Location> locationList = new List<Location>();

            foreach (var s in sectors)
            {
                var location = new Location
                {
                    LocationId = ServiceRepository.LocationRepository().GetDbNewId("Location"),
                    Name = s,
                    DirectorateId = directorate.DirectorateId,
                    CreatedBy = CurrentUser.UserName,
                    LastModifiedBy = CurrentUser.UserName,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                };

                locationList.Add(location);

            }

            return locationList;
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}