
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
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Web.Controllers._partials;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {

    public partial class RoleDescriptionController : WorkflowController
        {
        private RoleDescriptionRepository _repository = null;
        public RoleDescriptionRepository Repository
            {
            get
                {
                return _repository = _repository ?? ServiceRepository.RoleDescriptionRepository();
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
            var gradeItems = ServiceRepository.GradeRepository().GradeOnType(false).ToArray().Select(pe => new SelectListItemExtension { Value = pe.GradeCode.ToString(), Text = pe.GradeTitle })
                   .ToArray();
            ViewBagWrapper.ListBag.SetList("gradeItems", gradeItems, ViewData);
            ViewBagWrapper.ListBag.SetList("statusItems", statusCodeItems, ViewData);
            return View();
            }

        [HasDoeRole]
        public ActionResult ManageSummary(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.Summary, ViewData);

            ViewBagWrapper.VariableBag.SetBoolVariable("IsPositionRoleDesc", false, ViewData);
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);

            GetErrorsFromTempData();
            CreateLookups();
            GetNumberOfLinkedPositions(id, task);
            if(id == 0)
                {
                var model = new RoleDescription()
                    {
                    RolePositionDescription = new RolePositionDescription()
                    };
                SetWorkflowEngine(model);
                return View("Manage", model);
                }
            else
                {
                var model = Repository.ListForCapabilityFramework().FirstOrDefault(l => l.RoleDescriptionId == id);
                    if (model == null)
                    {
                    var msg = MessageHelper.NotFoundMessage("role description");
                    throw new HttpException(msg);
                }

                    if(model != null)
                    {
                    model.RoleCapabilities = RoleDescriptionExtensions.SortCapabilityGroup(model).ToList();
                    
                    SetWorkflowEngine(model);
                    return View("Manage", model);


                    }
                ViewBagWrapper.VariableBag.SetBoolVariable("MissingRoleDesc", true, ViewData);
                SetWorkflowEngine(new RoleDescription());
                return View("Manage", new RoleDescription());
                }

            }

        [System.Web.Mvc.HttpGet]
        [MustHaveAdminOrPowerRoleAttribute]
        public ActionResult Trim(int id) {

            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.Trim,
                 ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var model = Repository.GetRoleDescriptionById(id);
            if(model == null) {
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
                return View("Manage", model);
                }

            var recordInfo = ServiceRepository.TrimRecordRepository().GetRecordInfoModel(id, true);
            ViewBagWrapper.SetGeneralObject("TrimRecordInfo", recordInfo, ViewData);
            SetWorkflowEngine(model);
            return View("Manage", model);

            }

        [MustHaveAdminOrPowerRole]
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Trim")]
        public ActionResult TrimConfirm(int id) {
            var model = Repository.GetRoleDescriptionById(id);
            var comment = $"Re synced from {CurrentUser.DisplayName} at {DateTime.Now}";
            ServiceRepository.TrimRecordRepository().SynchRolePosDescription(id, comment);
            
                var hisItem = new RolePositionDescriptionHistory {
                    Action = Enums.WorkflowActions.SyncPdOrRdAttachmentToTrim.ToString().Wordify(),
                    RolePositionDescId = id,
                    StatusFrom = model.StatusValue.ToString(),
                    StatusTo = model.StatusValue.ToString(),
                    CreatedBy = CurrentUser.UserName,
                    CreatedDate = DateTime.Now,
                    AdditionalInfo = comment

                    };
            this.ServiceRepository.RolePositionDescriptionHistoryRepository().Insert(hisItem);
                return RedirectToAction("Trim", new { id });

            }

        public ActionResult ListJson([FromUri] JQueryDataTableRolePositionDesc arg)
            {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
            var task = UserTaskFactory.GetTask(CurrentUser, ServiceRepository.RepositoryFactory);

            try
                {
                var roleDescriptions = task.GetRoleDescriptions().AsNoTracking();
                var displayedRoleDescriptions = Repository.FilterDisplayedRoleDescriptions(arg, roleDescriptions);



                var totalRecord = displayedRoleDescriptions.Count();
                var totalDisplayRecord = displayedRoleDescriptions.Count();

                if(arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedRoleDescriptions = displayedRoleDescriptions.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);


                var cachedPositions = ServiceRepository.PositionRepository().CachedPositionListForChart();

                var result = displayedRoleDescriptions.AsEnumerable().ToArray()
                    .Select(ent => ent.To(new RoleDescriptionLight
                        {
                        //Note, these property names must match to in the datatable
                        DateOfApproval = ent.RolePositionDescription.DateOfApproval?.ToEasyDateFormat(),
                        Title = ent.RolePositionDescription.Title,
                        DocNumber = ent.RolePositionDescription.DocNumber,
                        GradeTitle = ent.RolePositionDescription.Grade.GradeTitle,
                        Status = ent.RolePositionDescription.StatusValue.StatusName,

                        LinkedPositions = cachedPositions.Count(p => p.RolePositionDescriptionId == ent.RoleDescriptionId),

                        Privilege = GetRoleDescriptionPrivilege(ent)
                        }));

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
                dataTableResult.AddError(new DbValidationError("", msg + exception.Message));
                //dataTableResult.AddError(new DbValidationError("",
                //    "Oops! something went wrong. " + exception.Message));
            }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
            }

        // GET: /RoleDescription/Details/5
        public ActionResult DetailsJson(int id = 0)
            {
            var ajaxResult = new Result();

            try
                {
                var roleDescriptionLight = Repository.GetRoleDescriptionById(id).To(new RoleDescriptionLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = roleDescriptionLight;

                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
                }
            catch(Exception exception)
                {
                LogException(exception);
                ajaxResult.Status = Status.Error;
                ajaxResult.Message = "Errors";
                var msg = MessageHelper.ErrorOccured();
                ajaxResult.AddError(new DbValidationError("", msg + exception.Message));
                //ajaxResult.AddError(new DbValidationError("",
                //    "Oops! something went wrong. " + exception.Message));
                return Json(ajaxResult, JsonRequestBehavior.AllowGet);
                }

            }


        [HasAnyAdminRole]
        public ActionResult Delete(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);

            var roleDescription = Repository.GetRoleDescriptionById(id);
            if(roleDescription == null)
                {
                var msg = MessageHelper.NotFoundMessage("role description");
                throw new HttpException(msg);
                }
            if(Request.IsAjaxRequest())
                {
                return View("Delete-modal", roleDescription);
                }
            else
                {
                return View(roleDescription);
                }


            }
        //
        [HasAdminOrPowerRole]
        public ActionResult Create()
            {
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            //GetErrorsFromTempData();
            return RedirectToAction("ManageBasicDetails", new
                {
                id = 0
                });
            }

        //
        // POST: /RoleDescription/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAdminOrPowerRole]
        public ActionResult Create(FormCollection fm)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var ajaxResult = new Result();
            var roleDescription = new RoleDescription();

            ModelState.Remove("RolePositionDescription.CreatedBy");
            ModelState.Remove("RolePositionDescription.LastModifiedBy");

            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if(ModelState.IsValid)
                {
                try
                    {
                    //insert to RolePositionDescription and RoleDescription
                        var rolePositionDesc = new RolePositionDescription
                        {
                            Version = 1,
                            StatusId = (int) Enums.StatusValue.Draft,
                            Title = Request["Title"],
                            GradeCode = Request["RolePositionDescription.GradeCode"],
                            DocNumber = CommonHelper.ConstructDocNumber(Request)
                        };



                        var roleDesc = new RoleDescription
                        {
                            RolePrimaryPurpose = Request["RolePrimaryPurpose"],
                            DecisionMaking = Request.Unvalidated.Form["DecisionMaking"],
                            ANZSCOCode = Request["ANZSCOCode"],
                            PCATCode = Request["PCATCode"],
                            SeniorExecutiveWorkLevelStandards = Request["SeniorExecutiveWorkLevelStandards"]
                        };

                    if(
                         this.Repository.RolePositionDescriptionRepository.HasDocNumber(rolePositionDesc.DocNumber))
                        {
                        throw new InvalidOperationException("This document number already exists");
                        }

                    Repository.CreateRoleDescription(roleDesc, rolePositionDesc);

                    //add to history
                    ServiceRepository.RolePositionDescriptionHistoryRepository()
                        .LogHistoryOnCreateRolePositioinDesc(rolePositionDesc, CurrentUser.UserName);

                    return RedirectToAction("ManageKeyAccountabilities", new { id = roleDesc.RoleDescriptionId });
                    }
                catch(Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("DB", msg + exception.Message));
                    //errors.Add(new DbValidationError("DB", "Oops! something went wrong. " + exception.Message));

                    SetErrorsInTempData(errors);

                    ViewBagWrapper.ErrorBag.SetErrors(Repository.GetValidationErrors(ModelState), ViewData);
                    CreateLookups();
                    return View(roleDescription);
                    }
                
                }
            else
                {
                ViewBagWrapper.ErrorBag.SetErrors(Repository.GetValidationErrors(ModelState), ViewData);
                CreateLookups();
                return View(roleDescription);
                }

            }


        [HasAnyAdminRole]
        public ActionResult Edit(int id = 0)
            {
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var task = GetTask();
            GetNumberOfLinkedPositions(id, task);
            return RedirectToAction("ManageBasicDetails", new { id = id });


            }

        //
        // POST: /RoleDescription/Edit/5
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAnyAdminRole]
        public ActionResult Edit(FormCollection collection)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var rolePositionDescId = Convert.ToInt32(Request["RoleDescriptionId"]);
            var task = GetTask();
            var state = task.GetRolePosModelState(rolePositionDescId);
            RemoveModelStates();
            var rolePositionDesc = new RolePositionDescription();

            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if(ModelState.IsValid)
                {
                try
                {

                    rolePositionDesc =
                        ServiceRepository.RolePositionDescriptionRepository().GetRolePositionDescById(rolePositionDescId);

                    var roleDesc = Repository.GetRoleDescriptionById(rolePositionDescId);



                    var oldStatusId = rolePositionDesc.StatusId;

                    var newRpd = new
                    {
                        GradeCode = collection["RolePositionDescription.GradeCode"],
                        Title = collection["Title"],
                        DocNumber = state.DocumentNumberEnabled ? CommonHelper.ConstructDocNumber(this.Request) : Request["RolePositionDescription.DocNumber"]
                    };

                    var newRd = new
                    {
                        RolePrimaryPurpose = collection["RolePrimaryPurpose"],
                        DecisionMaking = collection["DecisionMaking"],
                        ANZSCOCode = collection["ANZSCOCode"],
                        PCATCode = collection["PCATCode"],
                        SeniorExecutiveWorkLevelStandards = collection["SeniorExecutiveWorkLevelStandards"],
                        DivisionCode = collection["selectDivision"]
                    };


                    var newRpdObj = new RolePositionDescription
                    {
                        DocNumber = newRpd.DocNumber,
                        Title = newRpd.Title,
                        GradeCode = newRpd.GradeCode
                    };

                    var newRdObj = new RoleDescription
                    {
                        RolePrimaryPurpose = newRd.RolePrimaryPurpose,
                        DecisionMaking = newRd.DecisionMaking,
                        ANZSCOCode = newRd.ANZSCOCode,
                        PCATCode = newRd.PCATCode,
                        SeniorExecutiveWorkLevelStandards = newRd.SeniorExecutiveWorkLevelStandards
                    };


                    //catch the history change before updating
                    var historyChanges = ServiceRepository.RolePositionDescriptionHistoryRepository()
                        .GetBasicDetailsChanges(roleDesc, rolePositionDesc, newRdObj, newRpdObj);

                    var propState = task.GetRolePosModelState(rolePositionDesc.RolePositionDescId);

                    if (propState.GradeEnabled)
                    {
                        rolePositionDesc.GradeCode = newRpd.GradeCode;
                      

                        }
                    if (propState.TitleEnabled)
                    {
                        rolePositionDesc.Title = newRpd.Title;
                    }


                    var docNumber = newRpd.DocNumber;

                    if (!string.IsNullOrEmpty(docNumber))
                    {
                        if (propState.DocumentNumberEnabled)
                        {
                            rolePositionDesc.DocNumber = docNumber;
                            
                    }
                    else
                    {
                        rolePositionDesc.DocNumber = docNumber;
                            if(
                                  this.Repository.RolePositionDescriptionRepository.HasOtherDocumentNumber(
                                      rolePositionDesc.DocNumber, rolePositionDesc.RolePositionDescId))
                            {
                                var msg = MessageHelper.DocNumExistsMessage();
                                throw new InvalidOperationException(msg);
                                //throw new InvalidOperationException("This document number already exists");
                            }
                            }
                        }

                    ServiceRepository.RolePositionDescriptionRepository().Update(rolePositionDesc);

                    var isCreate = false;
                    if (roleDesc == null)
                    {
                        roleDesc = new RoleDescription();
                        isCreate = true;
                    }
                    roleDesc.RoleDescriptionId = rolePositionDesc.RolePositionDescId;
                    roleDesc.RolePrimaryPurpose = newRd.RolePrimaryPurpose;
                    roleDesc.DecisionMaking = newRd.DecisionMaking;
                    roleDesc.ANZSCOCode = newRd.ANZSCOCode;
                    roleDesc.PCATCode = newRd.PCATCode;
                    roleDesc.SeniorExecutiveWorkLevelStandards = newRd.SeniorExecutiveWorkLevelStandards;
                    

                    UpdateMissingItems(roleDesc);

                    if (!isCreate)
                    {
                        Repository.Update(roleDesc);
                    }
                    else
                    {
                        Repository.Insert(roleDesc);
                    }

                    //add history
                    if (roleDesc.RolePositionDescription.StatusId != (int)Enums.StatusValue.Draft)
                    {

                        ServiceRepository.RolePositionDescriptionHistoryRepository()
                                    .LogHistoryWhenUpdated(roleDesc.RoleDescriptionId, oldStatusId, oldStatusId,
                                    historyChanges, "BasicDetails", CurrentUser.UserName);
                    }
                }
                catch (Exception exception)
                    {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("DB", msg + exception.Message));
                    //errors.Add(new DbValidationError("DB", "Oops! something went wrong. " + exception.Message));

                    SetErrorsInTempData(errors);
                    return RedirectToAction("ManageBasicDetails", new { id = rolePositionDescId });
                    }

                return RedirectToAction("ManageKeyAccountabilities", new { id = rolePositionDescId });

                }
            else
                {
                ViewBagWrapper.ErrorBag.SetErrors(Repository.GetValidationErrors(ModelState), ViewData);
                CreateLookups();
                return View(new RoleDescription());
                }


            }

        private void UpdateMissingItems(RoleDescription roleDesc)
        {
            //load default from global items
            var globalItems = ServiceRepository.GlobalItemRepository().LoadAll();
            if (string.IsNullOrEmpty(roleDesc.Cluster))
            {
                roleDesc.Cluster =
                    globalItems.SingleOrDefault(g => g.ItemCode == Enums.GlobalItem.Cluster).ItemContent;
            }
            if (string.IsNullOrEmpty(roleDesc.Agency))
            {
                roleDesc.Agency =
                    globalItems.SingleOrDefault(g => g.ItemCode == Enums.GlobalItem.Agency).ItemContent;
            }
            if (string.IsNullOrEmpty(roleDesc.AgencyOverview))
            {
                roleDesc.AgencyOverview =
                    globalItems.SingleOrDefault(g => g.ItemCode == Enums.GlobalItem.AgencyOverview).ItemContent;
            }
            if (string.IsNullOrEmpty(roleDesc.AgencyWebsite))
            {
                roleDesc.AgencyWebsite =
                    globalItems.SingleOrDefault(g => g.ItemCode == Enums.GlobalItem.AgencyWebsite).ItemContent;
            }

            if (string.IsNullOrEmpty(roleDesc.RoleCapabilityItems) || string.IsNullOrEmpty(roleDesc.CapabilitySummary) ||
                string.IsNullOrEmpty(roleDesc.FocusCapabilities))
            {
                
                var roleCapForIntro =
                    globalItems.SingleOrDefault(g => g.ItemCode == Enums.GlobalItem.CapabilitiesForTheRole);
                roleDesc.RoleCapabilityItems = roleCapForIntro.ItemContent;

                var capabilitySummaryIntro =
                    globalItems.SingleOrDefault(g => g.ItemCode == Enums.GlobalItem.CapabilitySummary);
                roleDesc.CapabilitySummary = capabilitySummaryIntro.ItemContent;

                var focusCapabilityIntro =
                    globalItems.SingleOrDefault(g => g.ItemCode == Enums.GlobalItem.FocusCapabilitiesForTheRole);
                roleDesc.FocusCapabilities = focusCapabilityIntro.ItemContent;
            }
        }

        private void RemoveModelStates()
            {
            ModelState.Remove("RolePositionDescription.CreatedBy");
            ModelState.Remove("RolePositionDescription.LastModifiedBy");
            ModelState.Remove("RolePositionDescription.GradeCode");
            }

        //
        // POST: /RoleDescription/Delete/5

        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasAnyAdminRole]
        public ActionResult DeleteConfirmed(RoleDescription roleDescription)
            {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Delete, ViewData);
            var ajaxResult = new Result();
            var oldRoleDescription = Repository.GetEntityByEntityKey(roleDescription);
            try
                {
                Repository.Delete(oldRoleDescription);
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

            var enity = T4Helper.GetEntityType("RoleDescription", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);

            var psne = Enum.GetName(typeof(Enums.GradeType), 1);
            var psse = Enum.GetName(typeof(Enums.GradeType), 2);

            var grades = ServiceRepository.GradeRepository()
                    .List().Where(l => l.GradeType == psne || l.GradeType == psse).ToArray()
                    .Select(pe => new SelectListItemExtension { Value = pe.GradeCode.ToString(), Text = pe.GradeTitle })
                    .ToArray();
            ViewBagWrapper.ListBag.SetList("gradesItems", grades, ViewData);

            }

        private Enums.Privilege GetRoleDescriptionPrivilege(RoleDescription ent)
            {
            var wf = SetWorkflowEngine(ent);
            return wf.GetWorkflowObjectPrivilege();
            }

        public ActionResult DocNumberExists(string docNumPart1, string docNumPart2, 
            int versionNum, string formType, string oldDocNum)
        {
            var result = new { valid = true };

            var docNum = "DOC" + docNumPart1 + "/" + docNumPart2;

            if (formType == "Create")
            {
                versionNum = 1;
            }
            var docNumberExists = Repository.RolePositionDescriptionRepository.HasDocNumber(docNum, versionNum);
            if (docNum == oldDocNum)
            {
                result = new {valid = true};
            }
            else
            {
                result = new { valid = !docNumberExists };
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
            {

            base.Dispose(disposing);
            }



        }
    }