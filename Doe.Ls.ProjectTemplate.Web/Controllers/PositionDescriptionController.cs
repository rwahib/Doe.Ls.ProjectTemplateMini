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
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Web.Controllers._partials;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    [HasDoeRole]
    public partial class PositionDescriptionController : WorkflowController
        {
     private PositionDescriptionRepository _repository=null;
     public PositionDescriptionRepository Repository
         {
         get
         {

             return _repository=_repository ?? ServiceRepository.PositionDescriptionRepository();

             }
 
         }

        [HasDoeRole]
        public ActionResult Index()
        {
            var task = GetTask();
            
            var statusCodeItems = task.GetPositionStatus().ToArray()
                .Select(pe => new SelectListItemExtension
                {
                    Value = ((int)pe).ToString(),
                    Text = pe.ToString(),
                    // Selected = intArr != null && intArr.Contains((int)pe)
                })
                .ToArray();
            var gradeItems = ServiceRepository.GradeRepository().GradeOnType(true).ToArray().Select(pe => new SelectListItemExtension { Value = pe.GradeCode.ToString(), Text = pe.GradeTitle })
                   .ToArray();
            ViewBagWrapper.ListBag.SetList("gradeItems", gradeItems, ViewData);
            ViewBagWrapper.ListBag.SetList("statusItems", statusCodeItems, ViewData);
            
            return View();
        }

      
         public ActionResult ListJson([FromUri] JQueryDataTableRolePositionDesc arg) 
         {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
             var task = GetTask();

            try
            {
                
                var positionDescriptions= task.GetPositionDescriptions().AsNoTracking();


                var displayedPositionDescriptions = Repository.FilterDisplayedPositions(arg, positionDescriptions);
                var totalRecord =  displayedPositionDescriptions.Count();
                var totalDisplayRecord =  displayedPositionDescriptions.Count();

                if (arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedPositionDescriptions =  displayedPositionDescriptions.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);
                var cachedPositions = ServiceRepository.PositionRepository().CachedPositionListForChart();

              

                var result = displayedPositionDescriptions.AsEnumerable().ToArray().Select(ent => ent.To(new PositionDescriptionLight() 
                {
                    DateOfApproval = ent.RolePositionDescription.DateOfApproval?.ToEasyDateFormat(),
                    LastModifiedDate = ent.RolePositionDescription.LastModifiedDate.ToEasyDateTimeFormat(),
                    Title =ent.RolePositionDescription.Title,
                    DocNumber = ent.RolePositionDescription.DocNumber,
                    GradeCode =ent.RolePositionDescription.GradeCode,
                    GradeTitle = ent.RolePositionDescription.Grade.GradeTitle,
                    StatusValue = ent.RolePositionDescription.StatusValue.StatusName,
                    LinkedPositions = cachedPositions.Count(p=>p.RolePositionDescriptionId == ent.PositionDescriptionId),
                    Privilege = GetPositionDescriptionPrivilege(ent)

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
        
        //
        // GET: /PositionDescription/Create
        [System.Web.Mvc.HttpGet]
        [HasAnyAdminRole]
        public ActionResult Create()
        {
            ViewBagWrapper.FormOperations.SetNullModel(true,ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            GetErrorsFromTempData();
            
            return RedirectToAction("ManageOverview",new {id=0});

        }

        //
        // POST: /PositionDescription/Create

        
      

        //
        // GET: /PositionDescription/Edit/5
        [HasAnyAdminRole]
        public ActionResult Edit(int  id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var positionDescription = Repository.GetEntityByKey(id);
            if (positionDescription == null)
            {
                var msg = MessageHelper.NotFoundMessage("position description");
                throw new HttpException(msg);
            }
            var wf = SetWorkflowEngine(positionDescription);
            return RedirectToAction("ManageOverview", new { id = id });

        }

        //
        // POST: /PositionDescription/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAnyAdminRole]
        public ActionResult ManageOverview(FormCollection collection)
        {
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            ModelState.Remove("RolePositionDescription.CreatedBy");
            ModelState.Remove("RolePositionDescription.LastModifiedBy");
            ModelState.Remove("RolePositionDescription.DocNumber");
            var rolePosDesc = new RolePositionDescription();
            var positionDescId = Convert.ToInt32(collection["PositionDescriptionId"]);
            if (positionDescId == 0) // create new pd
            {
                ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
                var positionDescription = new PositionDescription
                {
                    PositionDescriptionId = 0,
                    RolePositionDescription = null,
                    StatementOfDuties= collection["StatementOfDuties"],
                    BriefRoleStatement = collection["BriefRoleStatement"],
                    };
                if(ModelState.IsValid)
                    {
                    try
                        {
                        var model = new RolePositionDescription();
                        rolePosDesc = ValidateAndSetRolePosDescObject(ref model);
                        if(
                                   this.Repository.RolePositionDescriptionRepository.HasDocNumber(rolePosDesc.DocNumber))
                            {
                            var msg = MessageHelper.DocNumExistsMessage();
                            throw new InvalidOperationException(msg);
                            }

                        if(positionDescription.StatementOfDuties != null)
                            {
                            var result = CommonHelper.ValidBulletPoints(positionDescription.StatementOfDuties, 6, 8,
                                "Statement of duties");
                            if(result.Status == Status.Error)
                            {
                                positionDescription.RolePositionDescription = rolePosDesc;
                                throw new InvalidOperationException(result.Message);
                                }
                            }

                        Repository.CreatePositionDescription(positionDescription, rolePosDesc);

                            var trimRecordRep = ServiceRepository.TrimRecordRepository();
                        // just in case if there is any old in complete transactions

                            trimRecordRep.SynchRolePosDescription(rolePosDesc.RolePositionDescId, "Initialise");
                        

                        //add to history
                        ServiceRepository.RolePositionDescriptionHistoryRepository()
                            .LogHistoryOnCreateRolePositioinDesc(rolePosDesc, CurrentUser.UserName);

                        }
                    catch(Exception exception)
                        {
                        LogException(exception);
                        errors.AddRange(Repository.GetBackendValidationErrors());
                        errors.Add(new DbValidationError("", exception.Message));
                        ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                        CreateLookups();
                        ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.Overview, ViewData);
                        ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
                            if (positionDescription.RolePositionDescription == null)
                            {        positionDescription.RolePositionDescription = rolePosDesc;}

                        positionDescription.RolePositionDescription.StatusValue = new StatusValue
                            {
                                StatusId = (int) Enums.StatusValue.Draft,
                                StatusName = Enums.StatusValue.Draft.ToString()

                            };
                         SetWorkflowEngine(positionDescription);
                        return View("Manage", positionDescription);                      
                        }
                    CreateLookups();
                    var wf = SetWorkflowEngine(positionDescription);
                    return RedirectToAction("ListSelectionCriteria", new { id = positionDescription.PositionDescriptionId });
                    }
                else
                    {
                    ViewBagWrapper.ErrorBag.SetErrors(Repository.GetValidationErrors(ModelState), ViewData);
                    CreateLookups();
                    SetNumberOfLinkedPositions(positionDescription.PositionDescriptionId);
                    var wf = SetWorkflowEngine(positionDescription);
                    return View("Manage", positionDescription);
                    }

                }
            else // Edit
            {
                var task = GetTask();
              

                ModelState.Remove("RolePositionDescription.Title");
                ModelState.Remove("RolePositionDescription.GradeCode");
                ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
                var newPpositionDescription = new PositionDescription
                    {                    
                    StatementOfDuties = collection["StatementOfDuties"],
                    BriefRoleStatement = collection["BriefRoleStatement"],
                    };

                if(ModelState.IsValid)
                    {

                    var oldRolePosDesc =
                        ServiceRepository.RolePositionDescriptionRepository().List().FirstOrDefault(l => l.RolePositionDescId == positionDescId);
                    var posDesc = Repository.LoadPositionDescById(positionDescId);

                    var oldStatusId = oldRolePosDesc.StatusId;
                    
                    
                    //catch history before updating
                    var historyChanges =
                        ServiceRepository.RolePositionDescriptionHistoryRepository()
                            .GetBreifStatementDutiesChanges(oldRolePosDesc.PositionDescription, newPpositionDescription);

                    try
                        {

                        var result = CommonHelper.ValidBulletPoints(newPpositionDescription.StatementOfDuties, 6, 8, "Statement of duties");

                         rolePosDesc = ValidateAndSetRolePosDescObject(ref oldRolePosDesc);
                        
                        rolePosDesc.PositionDescription.BriefRoleStatement = newPpositionDescription.BriefRoleStatement;
                        rolePosDesc.PositionDescription.StatementOfDuties = newPpositionDescription.StatementOfDuties;
                        var modelState = task.GetRolePosModelState(positionDescId);
                            if (modelState.TitleEnabled)
                            { rolePosDesc.Title = collection["RolePositionDescription.Title"];}
                        if(modelState.GradeEnabled)
                            { rolePosDesc.GradeCode = collection["RolePositionDescription.GradeCode"];}

                            if (modelState.DocumentNumberEnabled)
                            {
                                rolePosDesc.DocNumber = CommonHelper.ConstructDocNumber(Request);

                                if (
                                    this.Repository.RolePositionDescriptionRepository.HasOtherDocumentNumber(
                                        rolePosDesc.DocNumber, rolePosDesc.RolePositionDescId))
                                {
                                    throw new InvalidOperationException("This document number already exists");
                                }


                            }

                            if(result.Status == Status.Error)
                            {
                            throw new InvalidOperationException(result.Message);
                            }
                        
                            Repository.UpdatePositionDescription(rolePosDesc);
                       
                       
                        //add to history
                        if(oldRolePosDesc.StatusId != (int)Enums.StatusValue.Draft)
                            {
                            ServiceRepository.RolePositionDescriptionHistoryRepository()
                                     .LogHistoryWhenUpdated(positionDescId, oldStatusId, oldStatusId,
                                     historyChanges, "BriefRoleStatement/StatementOfDuties", CurrentUser.UserName);
                            }

                        }
                    catch(Exception exception)
                        {
                        LogException(exception);
                        errors.AddRange(Repository.GetBackendValidationErrors());
                        errors.Add(new DbValidationError("", exception.Message));
                        ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                        CreateLookups();
                        ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.Overview, ViewData);
                        ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
                        SetWorkflowEngine(oldRolePosDesc.PositionDescription);
                        return View("Manage", oldRolePosDesc.PositionDescription);
                        }

                    return RedirectToAction("ListSelectionCriteria", new { id = positionDescId });
                    }
                else
                    {
                    SetErrorsInTempData(errors);
                    CreateLookups();

                    return RedirectToAction("ManageOverview", new { id = positionDescId });
                    }

                }

        }

    
        private void CreateLookups() 
        {        
        
        var enity = T4Helper.GetEntityType("PositionDescription", this.ServiceRepository.GetUnitOfWork().DbContext);
        ViewBagWrapper.EntityInfo.SetEntityType(enity,ViewData);        
              
            var grades = ServiceRepository.GradeRepository()
                    .List().Where(l=>l.GradeType=="NSBTS").ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.GradeCode.ToString(), Text = pe.GradeTitle })
                    .ToArray();
            ViewBagWrapper.ListBag.SetList("gradesItems", grades, ViewData);
        }
        private Enums.Privilege GetPositionDescriptionPrivilege(PositionDescription ent)
            {
            var wf = SetWorkflowEngine(ent);
            return wf.GetWorkflowObjectPrivilege();
            }


        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }


       
        }
}