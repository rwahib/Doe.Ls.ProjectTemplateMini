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
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    public partial class FunctionalAreaController : AppControllerBase
    {
     private FunctionalAreaRepository _repository=null;
     public FunctionalAreaRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.FunctionalAreaRepository();

             }
 
         }

        [HasAnyAdminRole]
        public ActionResult Index()
        {
            var functionalAreas = Repository.List();
            CreateLookups();
            return View(functionalAreas);
        }

        //
        // GET: /FunctionalArea/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var functionalArea = Repository.GetFunctionalAreaById(id);
            if (functionalArea == null)
            {
                var msg = MessageHelper.NotFoundMessage($"functional area ({id})");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Details-modal", functionalArea);
              } else {
            return View(functionalArea);
              }

            
        }

         public ActionResult ListJson([FromUri] BasicStructureArgument arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
            var task = UserTaskFactory.GetTask(this.CurrentUser, this.Repository.RepositoryFactory);
            try
            {
                var functionalAreas = Repository.List();
                IQueryable<FunctionalArea> displayedFunctionalAreas;
            
                if (!string.IsNullOrWhiteSpace(arg.sSearch) || !string.IsNullOrWhiteSpace(arg.DivisionCode) || arg.DirectorateId>0)
                {                    
                    displayedFunctionalAreas = Repository.FilterFunctionalAreas(functionalAreas, arg);
                    displayedFunctionalAreas = CustomOrderBy.CustomSort(displayedFunctionalAreas, arg);
                    }
                else
                {
                    displayedFunctionalAreas = CustomOrderBy.CustomSort(functionalAreas, arg);
                }

                var totalRecord =  displayedFunctionalAreas.Count();
                var totalDisplayRecord =  displayedFunctionalAreas.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedFunctionalAreas =  displayedFunctionalAreas.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                var result = displayedFunctionalAreas.AsEnumerable().ToArray().Select(ent =>
                {
                    var light = ent.To(new FunctionalAreaLight());
                    light.DirectorateName = ent.Directorate.DirectorateName;
                    light.ExecutiveTitle = ent.Directorate.Executive.ExecutiveTitle;
                    light.StatusName = ent.StatusValue.StatusName;
                    light.Privilege = task.GetFunctionAreaPrivilege(ent.FuncationalAreaId);
                    return light;
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
                dataTableResult.AddError(new DbValidationError("Error", msg + exception.Message));
                //dataTableResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
            }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
        }

         // GET: /FunctionalArea/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();
            
            try {
                var functionalAreaLight = Repository.GetFunctionalAreaById(id).To(new FunctionalAreaLight());
                

                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = functionalAreaLight;

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



        // GET: /FunctionalArea/Delete/5
        [HasAdminOrPowerRole]
        public ActionResult Delete(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var functionalArea = Repository.GetFunctionalAreaById(id);
            if (functionalArea == null)
            {
                var msg = MessageHelper.NotFoundMessage($"functional area ({id})");
                throw new HttpException(msg);
            }
             if (Request.IsAjaxRequest()) {
                  return View("Delete-modal", functionalArea);
              } else {
            return View(functionalArea);
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
                var directorate=this.ServiceRepository.DirectorateRepository().GetDirectorateById(directorateId.Value);
                divisionCode = directorate.ExecutiveCod;
                ViewBagWrapper.SetGeneralObject("divisionCode", divisionCode, ViewData);
                }
            CreateLookups();
            var model = new FunctionalArea()
                {
                FuncationalAreaId = Repository.GetMaxKeyValue() + 10,
                DirectorateId = directorateId ?? 0,             
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
        // POST: /FunctionalArea/Create
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FunctionalArea functionalArea)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var trailingList = functionalArea.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);            
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            
            if (ModelState.IsValid) {
                try {
                    Repository.Insert(functionalArea);
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
                    return View(functionalArea);
                }
                
            }
            
        }


        [HasAdminOrPowerRole]
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var functionalArea = Repository.GetFunctionalAreaById(id);
            if (functionalArea == null)
            {
                var msg = MessageHelper.NotFoundMessage($"functional area ({id})");
                throw new HttpException(msg, 404);
            }

            ViewBagWrapper.SetGeneralObject("divisionCode", functionalArea.Directorate.ExecutiveCod, ViewData);
            ViewBagWrapper.SetGeneralObject("directorateId", functionalArea.DirectorateId, ViewData);

            CreateLookups();
            
              if (Request.IsAjaxRequest()) {
                  return View("Edit-modal", functionalArea);
              } else {
                  return View(functionalArea);
              }
        }

        //
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FunctionalArea functionalArea)
        {
            
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var ajaxResult = new Result();
            
            var trailingList = functionalArea.UpdateSignature(this.Repository.RepositoryFactory);
            this.ModelState.RemoveList(trailingList);
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            if (ModelState.IsValid)
            {
                var oldFunctionalArea = Repository.GetEntityByEntityKey(functionalArea);
                
                Repository.SetPropertyValuesFrom(ref oldFunctionalArea,functionalArea);

                try
                {
                    Repository.Update(oldFunctionalArea);     
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
                    return View(functionalArea);
                }                
            }          
        }

        
        [HasAdminOrPowerRole]
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(FunctionalArea functionalArea)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();            
            var oldFunctionalArea = Repository.GetEntityByEntityKey(functionalArea);
            try
            {
                Repository.Delete(oldFunctionalArea);     
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
        public ActionResult GetFunctionalAreas(int directorateId = 0, bool displayNumbers = true)
        {

            var liveStatus = new int[] { Enums.StatusValue.Approved.ToInteger(), Enums.StatusValue.Imported.ToInteger() };

            var fas = Repository.BasicList().Include(bu => bu.Units.Select(u => u.Positions)).Where(l => l.DirectorateId == directorateId).ToArray()
               .Select(fa => new SelectListItemExtension
                   {
                   Value = fa.FuncationalAreaId.ToString(),
                   Text = displayNumbers ? $"{fa.AreanName}({ fa.Units.SelectMany(u => u.Positions.Where(p => liveStatus.Contains(p.StatusId))).Count()})" : $"{fa.AreanName}"
                   })
               .ToArray();


            return Json(fas, JsonRequestBehavior.AllowGet);

            }

        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("FunctionalArea", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);
            var divisionCode=ViewBagWrapper.GetGeneralObject<string>("divisionCode", ViewData);

            var directorateList = ServiceRepository.DirectorateRepository()
                    .List();
            if(!string.IsNullOrWhiteSpace(divisionCode)) directorateList = directorateList.Where(d=>d.ExecutiveCod==divisionCode);
            var directorateItems=directorateList.ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.DirectorateId.ToString(), Text =pe.DirectorateName})
                    .ToArray();
        
        ViewBagWrapper.ListBag.SetList("directorateItems",directorateItems,ViewData);

            var divisionItems = ServiceRepository.ExecutiveRepository()
                       .List().ToArray()
                       .Select(pe => new SelectListItemExtension { Value = pe.ExecutiveCod.ToString(), Text = pe.ExecutiveTitle })
                       .ToArray();

            ViewBagWrapper.ListBag.SetList("divisionItems", divisionItems, ViewData);

            var statusValueItems = ServiceRepository.StatusValueRepository()
                      .ActiveNotActiveList().ToArray()
                      .Select(pe => new SelectListItemExtension { Value = pe.StatusId.ToString(), Text = pe.StatusName })
                      .ToArray();


            ViewBagWrapper.ListBag.SetList("statusValueItems",statusValueItems,ViewData);
                

          }

               
        
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}