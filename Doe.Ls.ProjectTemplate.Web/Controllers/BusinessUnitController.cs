using System;
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
    
    public partial class BusinessUnitController : AppControllerBase
    {
     private BusinessUnitRepository _repository=null;
     public BusinessUnitRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.BusinessUnitRepository();

             }
 
         }

        [HasAnyAdminRole]
        public ActionResult Index()
        {
            var businessUnits = Repository.List();
            CreateLookups();
            return View(businessUnits.ToList());
        }

        //
        // GET: /BusinessUnit/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var businessUnit = Repository.GetBUnitById(id);
            if (businessUnit == null)
            {
                throw new HttpException($"The business unit ({id}) was not found");
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", businessUnit);
              } else {
            return View(businessUnit);
              }

            
        }
        
         public ActionResult ListJson([FromUri] BasicStructureArgument arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
             var task = UserTaskFactory.GetTask(this.CurrentUser, this.Repository.RepositoryFactory);
             try
             {
                 var businessUnits = Repository.List();
                 IQueryable<BusinessUnit> displayedBusinessUnits;

                 if (!string.IsNullOrWhiteSpace(arg.sSearch) || arg.DirectorateId>0 || !string.IsNullOrWhiteSpace(arg.DivisionCode) )
                 {
                    
                     displayedBusinessUnits = Repository.FilterBusinessUnits(businessUnits, arg);
                     displayedBusinessUnits = CustomOrderBy.CustomSort(displayedBusinessUnits, arg);

                    }
                else
                 {
                     displayedBusinessUnits = CustomOrderBy.CustomSort(businessUnits, arg);
                 }

                 var totalRecord = displayedBusinessUnits.Count();
                 var totalDisplayRecord = displayedBusinessUnits.Count();

                 if (arg.iDisplayLength == -1)
                     arg.iDisplayLength = totalRecord;

                 displayedBusinessUnits = displayedBusinessUnits.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                 var result = displayedBusinessUnits.AsEnumerable().ToArray().Select(ent =>
                 {
                     var buLight = ent.To(new BusinessUnitLight());
                     buLight.DirectorateName= ent.Directorate.DirectorateName;
                     buLight.ExecutiveTitle = ent.Directorate.Executive.ExecutiveTitle;
                     buLight.StatusName = ent.Directorate.StatusValue.StatusName;
                     buLight.Privilege = task.GetBUnitPrivilege(ent.BUnitId);
                     buLight.ExecutiveCode = ent.Directorate.ExecutiveCod;
                     return buLight;
                 });

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
                dataTableResult.AddError(new DbValidationError("Error", msg+ exception.Message));
                //dataTableResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
            }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
        }

         // GET: /BusinessUnit/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var businessUnitLight = Repository.GetBUnitById(id).To(new BusinessUnitLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = businessUnitLight;

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

        [HasAdminOrPowerRole]
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var businessUnit = Repository.GetBUnitById(id);
            if (businessUnit == null)
            {
                var msg = MessageHelper.NotFoundMessage("business unit");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", businessUnit);
              } else {
            return View(businessUnit);
              }            
        }

        [HasAdminOrPowerRole]
        public ActionResult Create(string divisionCode, int? directorateId)
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
                }
            CreateLookups();
            var model = new BusinessUnit()
                {
                BUnitId = Repository.GetMaxKeyValue() + 10,
                DirectorateId = directorateId ?? 0,
                HierarchyId = Enums.HierarchyLevel.BusinessUnit.ToInteger(),
                StatusId = Enums.StatusValue.Active.ToInteger()
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

        //
        // POST: /BusinessUnit/Create

        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BusinessUnit businessUnit)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var trailingList = businessUnit.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);

            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(businessUnit);
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
                    return View(businessUnit);
                }
                
            }
            
        }

        [HasAdminOrPowerRole]
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var businessUnit = Repository.GetBUnitById(id);            
            if (businessUnit == null)
            {
                var msg = MessageHelper.NotFoundMessage("business unit");
                throw new HttpException(msg);
            }

            ViewBagWrapper.SetGeneralObject("divisionCode", businessUnit.Directorate.ExecutiveCod, ViewData);
            ViewBagWrapper.SetGeneralObject("directorateId", businessUnit.DirectorateId, ViewData);

            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", businessUnit);
              } else {
                  return View(businessUnit);
              }
        }

        
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BusinessUnit businessUnit)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            var trailingList = businessUnit.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            if (ModelState.IsValid)
            {
                var oldBusinessUnit = Repository.GetBUnitById(businessUnit.BUnitId);
                
                Repository.SetPropertyValuesFrom(ref oldBusinessUnit,businessUnit);

                try
                {
                    Repository.Update(oldBusinessUnit);     
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
                    return View(businessUnit);
                }                
            }          
        }

        //
        // POST: /BusinessUnit/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [HasAdminOrPowerRole]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(BusinessUnit businessUnit)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldBusinessUnit = Repository.GetEntityByEntityKey(businessUnit);
            try
            {
                Repository.Delete(oldBusinessUnit);     
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
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult GetBusinessUnits(int directorateId = 0, bool displayNumbers = true, bool currentUser = false,bool fromChart = false)
        {
            var liveStatus=new int[] {Enums.StatusValue.Approved.ToInteger(), Enums.StatusValue.Imported.ToInteger()};

            var list =
                Repository.BasicList()
                    .Include(bu => bu.Units.Select(u => u.Positions))
                    .Where(l => l.DirectorateId == directorateId).ToList();
            if (currentUser) list = list.FilterDefaults(this.CurrentUser).ToList();
            var bunits = list
                   .Select(bu => new SelectListItemExtension
                   {
                       Value = bu.BUnitId.ToString(),
                       Text = displayNumbers ? $"{bu.BUnitName}({ bu.Units.SelectMany(u => u.Positions.Where(p => liveStatus.Contains(p.StatusId))).Count()})" : $"{bu.BUnitName}"
                       })
                   .ToArray();


            return Json(bunits, JsonRequestBehavior.AllowGet);

        }

        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("BusinessUnit", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);

            var divisionCode = ViewBagWrapper.GetGeneralObject<string>("divisionCode", ViewData);

            var directorateList = ServiceRepository.DirectorateRepository()
                    .List();
            if(!string.IsNullOrWhiteSpace(divisionCode)) directorateList = directorateList.Where(d => d.ExecutiveCod == divisionCode);
            var directorateItems = directorateList.ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.DirectorateId.ToString(), Text = pe.DirectorateName })
                    .ToArray();

            ViewBagWrapper.ListBag.SetList("directorateItems", directorateItems, ViewData);

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

            var filteredOrgList = new[] {Enums.HierarchyLevel.Division.ToInteger(), Enums.HierarchyLevel.Directorate.ToInteger(),Enums.HierarchyLevel.BusinessUnit.ToInteger()};
          var hierarchyLevelItems=ServiceRepository.HierarchyLevelRepository()                  
                .List().Where(l=> filteredOrgList.Contains(l.HierarchyId)).ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.HierarchyId.ToString(), Text =pe.HierarchyDescription})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("hierarchyLevelItems",hierarchyLevelItems,ViewData);

            }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}