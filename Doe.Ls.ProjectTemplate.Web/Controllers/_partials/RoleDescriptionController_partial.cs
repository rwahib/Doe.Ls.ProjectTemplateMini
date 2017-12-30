using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Exceptions;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    [HasDoeRole]
    public partial class RoleDescriptionController
    {
        public RoleDescriptionController()
        {
        }

        public RoleDescriptionController(ServiceRepository srv)
        {
            _serviceRepository = srv;
            _repository = srv.RoleDescriptionRepository();
        }
        [HasAnyAdminRole]
        [System.Web.Mvc.HttpGet]
        public ActionResult ManageBasicDetails(int id)
        {
            var task = UserTaskFactory.GetTask(CurrentUser, ServiceRepository.RepositoryFactory);
            Enums.Privilege priv=null;
            RoleDescription model=null;
            if (id == 0)
            {
                priv = task.GetRoleDescriptionPrivilege();
                model = Repository.GetEmptyModel();              
                SetWorkflowEngine(model);
                }
            else
            {
                model = Repository.GetRoleDescriptionById(id);
                if (model == null)
                {
                    var msg = MessageHelper.NotFoundMessage("role description");
                    throw new HttpException(msg);
                }

                if (model != null && string.IsNullOrEmpty(model.Cluster) && string.IsNullOrEmpty(model.DecisionMaking))
                {
                    var globalItem = ServiceRepository.GlobalItemRepository()
                        .List().SingleOrDefault(g => g.ItemCode == Enums.GlobalItem.DecisionMaking);
                    model.DecisionMaking = globalItem.ItemContent;
                }
                
                var wf=SetWorkflowEngine(model);
                priv = wf.GetWorkflowObjectPrivilege();
            }


            if (priv.CanCreate || priv.CanEdit)
            {

                ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
                ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
                ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.BasicDetails, ViewData);
                GetErrorsFromTempData();
                CreateLookups();
                GetNumberOfLinkedPositions(id,task);

                if (id == 0)
                {

                    return ReturnMissingRoleDescription();
                }
                else
                {
                   
                    if (model == null)
                    {
                        
                        var rpd = ServiceRepository.RolePositionDescriptionRepository().GetRolePositionDescById(id);
                        model = Repository.GetEmptyModel();
                        if (rpd != null)
                        {
                            model.RolePositionDescription = rpd;
                        }

                    }
                    SetWorkflowEngine(model);
                    return View("Manage", model);
                }
            }
            else
            {
                var msg = MessageHelper.AccessDeniedMessage();
                throw new AccessDeniedException(msg);
                //throw new AccessDeniedException("Access denied");

            }
        }

        [HasAnyAdminRole]
        [System.Web.Mvc.HttpGet]
        public ActionResult ManageKeyAccountabilities(int id)
        {
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            Enums.Privilege priv = null;
            RoleDescription model = null;
            if (id == 0)
            {
                priv = task.GetRoleDescriptionPrivilege();
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
            }
            else
            {
                model = Repository.GetRoleDescriptionById(id);
                if (model == null)
                {
                    var msg = MessageHelper.NotFoundMessage("role description");
                    throw new HttpException(msg);
                }
                var wf = SetWorkflowEngine(model);
                priv = wf.GetWorkflowObjectPrivilege();
            }

            if (priv.CanCreate || priv.CanEdit)
            {
                //continue process
                ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
                ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
                ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.KeyAccountbilities, ViewData);
                GetErrorsFromTempData();
                CreateLookups();

                GetNumberOfLinkedPositions(id, task);
                if (id == 0)
                {
                    return ReturnMissingRoleDescription();
                }
                else
                {
                    if (model == null)
                    {

                        var rpd = ServiceRepository.RolePositionDescriptionRepository().GetRolePositionDescById(id);
                        model = Repository.GetEmptyModel();
                        if (rpd != null)
                        {
                            model.RolePositionDescription = rpd;
                        }
                    }
                    SetWorkflowEngine(model);
                    return View("Manage", model);
                }
            }
            else
            {
                var msg = MessageHelper.AccessDeniedMessage();
                throw new AccessDeniedException(msg);
            }
        }

        [HasAnyAdminRole]
        [System.Web.Mvc.HttpPost]
        public ActionResult SaveKeyAccountabilities(FormCollection collection)
        {
            var roleDescId = Convert.ToInt32(collection["RoleDescriptionId"]);
            var roleDesc = Repository.GetRoleDescPrimitiveItemsById(roleDescId);
            
            //set Grade
            ViewBagWrapper.SetGeneralObject("Grade", roleDesc.RolePositionDescription.Grade, ViewData);
            
            var oldStatusId = roleDesc.RolePositionDescription.StatusId;

            var newKeyAcctChallenges = new
            {
                KeyAccountabilities = collection["KeyAccountabilities"],
                KeyChallenges = collection["KeyChallenges"]
            };

            var newRoleDesc = new RoleDescription();

            newRoleDesc.KeyAccountabilities = newKeyAcctChallenges.KeyAccountabilities;
            newRoleDesc.KeyChallenges = newKeyAcctChallenges.KeyChallenges;

            //catch the changes before updating
            var historyChanges =
                ServiceRepository.RolePositionDescriptionHistoryRepository()
                    .GetKeyAccountabilityChallengeChanges(roleDesc, newRoleDesc);


            //updating...
            roleDesc.KeyAccountabilities = newKeyAcctChallenges.KeyAccountabilities;
            roleDesc.KeyChallenges = newKeyAcctChallenges.KeyChallenges;

            ViewBagWrapper.FormOperations.SetFormType(roleDesc.RoleDescriptionId == 0 ? FormType.Create : FormType.Edit, ViewData);
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            
            //Check bullet points
            var validate_Accountabilities = CommonHelper.ValidBulletPoints(roleDesc.KeyAccountabilities, 6, 9,
                "Key Accountabilities");
            var validate_KeyChallenges = CommonHelper.ValidBulletPoints(roleDesc.KeyChallenges, 2, 4,
                "Key Challenges");

            if (validate_Accountabilities.Status == Status.Success && validate_KeyChallenges.Status == Status.Success)
            {
                Repository.Update(roleDesc);
                ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.Budget, ViewData);

                //Add to history
                if (roleDesc.RolePositionDescription.StatusId != (int)Enums.StatusValue.Draft)
                {
                   ServiceRepository.RolePositionDescriptionHistoryRepository()
                            .LogHistoryWhenUpdated(roleDescId, oldStatusId, oldStatusId,
                            historyChanges, "KeyAccountabilities/KeyChallenges", CurrentUser.UserName);
                }

            }
            else
            {
                //display error message
                var errors = Repository.GetValidationErrors(ModelState).ToList();
                errors.AddRange(Repository.GetBackendValidationErrors());

                var msg = string.Empty;
                if (validate_Accountabilities.Status == Status.Error)
                {
                    msg = msg + validate_Accountabilities.Message + "<br />";
                }

                if (validate_KeyChallenges.Status == Status.Error)
                {
                    msg = msg + validate_KeyChallenges.Message;
                }


                errors.Add(new DbValidationError("Form validation ", msg));
                ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.KeyAccountbilities, ViewData);
                SetWorkflowEngine(roleDesc);
                return View("Manage", roleDesc);
            }
            //return View("Manage", roleDesc);
            return RedirectToAction("ManageBudget", new { id = roleDesc.RoleDescriptionId });

        }

        [HasAnyAdminRole]
        [System.Web.Mvc.HttpGet]
        public ActionResult ManageBudget(int id)
        {
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            Enums.Privilege priv = null;
            RoleDescription model = null;
            if (id == 0)
            {
                priv = task.GetRoleDescriptionPrivilege();
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
            }
            else
            {
                model = Repository.GetRoleDescriptionById(id);
                if (model == null)
                {
                    var msg = MessageHelper.NotFoundMessage("role description");
                    throw new HttpException(msg);
                }
                var wf = SetWorkflowEngine(model);
                priv = wf.GetWorkflowObjectPrivilege();
            }
            if (priv.CanCreate || priv.CanEdit)
            {
                ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
                ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
                ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.Budget, ViewData);
                GetErrorsFromTempData();
                CreateLookups();

                GetNumberOfLinkedPositions(id, task);
                if (id == 0)
                {
                    return ReturnMissingRoleDescription();
                }
                else
                {
                    if (model == null)
                    {
                        var rpd = ServiceRepository.RolePositionDescriptionRepository().GetRolePositionDescById(id);
                        model = Repository.GetEmptyModel();
                        if (rpd != null)
                        {
                            model.RolePositionDescription = rpd;
                        }
                    }

                    var rolePositionDesc = ServiceRepository.RolePositionDescriptionRepository().GetRolePositionDescById(id);
                    var grade = ServiceRepository.GradeRepository().GetGradeByCode(rolePositionDesc.GradeCode);

                    ViewBagWrapper.SetGeneralObject("Grade", grade, ViewData);

                    SetWorkflowEngine(model);
                    return View("Manage", model);
                }

            }
            else
            {
                var msg = MessageHelper.AccessDeniedMessage();
                throw new AccessDeniedException(msg);
            }
            
        }

        [HasAnyAdminRole]
        [System.Web.Mvc.HttpPost]
        public ActionResult SaveBudgetExpenditure(FormCollection collection)
        {

            var roleDescId = Convert.ToInt32(collection["RoleDescriptionId"]);
            var roleDesc = Repository.GetRoleDescPrimitiveItemsById(roleDescId);

            var historyRep = ServiceRepository.RolePositionDescriptionHistoryRepository();

            //these are the changed budget values
            var budgetExpenditure = new
            {
                BudgetExpenditure= collection["BudgetExpenditure"],
                BudgetExtraNotes= collection["BudgetExtraNotes"],
                BudgetValue= collection["BudgetValue"]
                
            };

            var newRd = new RoleDescription();
            newRd.BudgetExpenditure = budgetExpenditure.BudgetExpenditure; //checkbox
            newRd.BudgetExtraNotes = budgetExpenditure.BudgetExtraNotes;

            newRd.BudgetExpenditureValue = budgetExpenditure.BudgetValue;

            //catch the changes before update the roleResc
            var historyChanges = historyRep.GetBudgetChanges(roleDesc, newRd);
            
            //update roleResc for updating.
            roleDesc.BudgetExpenditure = budgetExpenditure.BudgetExpenditure;
            roleDesc.BudgetExtraNotes = budgetExpenditure.BudgetExtraNotes;

            if ( !string.IsNullOrEmpty(roleDesc.BudgetExpenditure) && roleDesc.BudgetExpenditure != "Nil")
            {
                roleDesc.BudgetExpenditureValue = newRd.BudgetExpenditureValue;
            }
            else
            {
                roleDesc.BudgetExpenditureValue = string.Empty;
            }

            Repository.Update(roleDesc);

            //add to history
            if (roleDesc.RolePositionDescription.StatusId != (int)Enums.StatusValue.Draft)
            {
                ServiceRepository.RolePositionDescriptionHistoryRepository()
                        .LogHistoryWhenUpdated(roleDescId, roleDesc.RolePositionDescription.StatusId, 
                        roleDesc.RolePositionDescription.StatusId,
                        historyChanges, "Budget/Expenditure", CurrentUser.UserName);
            }

            ViewBagWrapper.FormOperations.SetFormType(roleDesc.RoleDescriptionId == 0 ? FormType.Create : FormType.Edit, ViewData);
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.EssentialRequirements, ViewData);

            return RedirectToAction("ManageEssentialReq", new { id = roleDesc.RoleDescriptionId });
        }

        

        [HasAnyAdminRole]
        [System.Web.Mvc.HttpGet]
        public ActionResult ManageEssentialReq(int id)
        {
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            Enums.Privilege priv = null;
            RoleDescription model = null;
            if (id == 0)
            {
                priv = task.GetRoleDescriptionPrivilege();
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
            }
            else
            {
                model = Repository.GetRoleDescriptionById(id);
                if (model == null)
                {
                    var msg = MessageHelper.NotFoundMessage("role description");
                    throw new HttpException(msg);
                }
                var wf = SetWorkflowEngine(model);
                priv = wf.GetWorkflowObjectPrivilege();
            }


            if (priv.CanCreate || priv.CanEdit)
            {
                ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
                ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
                ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.EssentialRequirements, ViewData);
                GetErrorsFromTempData();
                CreateLookups();
                GetNumberOfLinkedPositions(id, task);

                if (id == 0)
                {
                    return ReturnMissingRoleDescription();
                }
                else
                {
                    if (model == null)
                    {
                        var rpd = ServiceRepository.RolePositionDescriptionRepository().GetRolePositionDescById(id);
                        model = Repository.GetEmptyModel();
                        if (rpd != null)
                        {
                            model.RolePositionDescription = rpd;
                        }
                    }

                    if (string.IsNullOrEmpty(model.EssentialRequirements))
                    {
                        var globalItems = ServiceRepository.GlobalItemRepository().LoadAll();
                        var content = string.Empty;

                        if (model.RolePositionDescription.Grade.GradeType == "PSSE")
                        {
                            content =
                                ServiceRepository.GlobalItemRepository()
                                    .GetGlobalItemByCode(globalItems, Enums.GlobalItem.PSSEManagersEssentialRequirement)
                                    .ItemContent;
                        }
                        else
                        {
                            content =
                               ServiceRepository.GlobalItemRepository()
                                   .GetGlobalItemByCode(globalItems, Enums.GlobalItem.GeneralEssentialRequirement)
                                   .ItemContent;

                        }

                        model.EssentialRequirements = content;
                    }
                    SetWorkflowEngine(model);
                    return View("Manage", model);
                }

            }
            else
            {
                var msg = MessageHelper.AccessDeniedMessage();
                throw new AccessDeniedException(msg);
            }
        }

        private ActionResult ReturnMissingRoleDescription()
        {
            RoleDescription model;
            ViewBagWrapper.VariableBag.SetBoolVariable("MissingRoleDesc", true, ViewData);
            model = Repository.GetEmptyModel();
            SetWorkflowEngine(model);
            return View("Manage", model);
        }

        [HasAnyAdminRole]
        [System.Web.Mvc.HttpPost]
        public ActionResult SaveEssentialReq(FormCollection collection)
        {
            var roleDescId = Convert.ToInt32(collection["RoleDescriptionId"]);
            var roleDesc = Repository.GetRoleDescPrimitiveItemsById(roleDescId);

            var oldStatusId = roleDesc.RolePositionDescription.StatusId;
            
            var newEssentialReq = collection["EssentialRequirements"];
            
            var newRoleDesc = new RoleDescription();
            newRoleDesc.EssentialRequirements = newEssentialReq;

            //catch the history before updating
            var historyChanges =
                ServiceRepository.RolePositionDescriptionHistoryRepository()
                    .GetEssentialReqChanges(roleDesc, newRoleDesc);

            roleDesc.EssentialRequirements = newEssentialReq;
            
            Repository.Update(roleDesc);
            
            //add to history
            if (roleDesc.RolePositionDescription.StatusId != (int)Enums.StatusValue.Draft)
            {
                
                ServiceRepository.RolePositionDescriptionHistoryRepository()
                        .LogHistoryWhenUpdated(roleDescId, oldStatusId, oldStatusId,
                        historyChanges, "EssentialRequirements", CurrentUser.UserName);
            }

            ViewBagWrapper.FormOperations.SetFormType(roleDesc.RoleDescriptionId == 0 ? FormType.Create : FormType.Edit, ViewData);
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.KeyRelationships, ViewData);

            return RedirectToAction("ManageKeyRelationships", new { id = roleDesc.RoleDescriptionId });
        }

        [HasAnyAdminRole]
        public ActionResult ManageCapabilities(int id)
        {
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            Enums.Privilege priv = null;
            RoleDescription model = null;
            if (id == 0)
            {
                priv = task.GetRoleDescriptionPrivilege();
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
            }
            else
            {
                model = Repository.GetRoleDescriptionById(id);
                if (model == null)
                {
                    var msg = MessageHelper.NotFoundMessage("role description");
                    throw new HttpException(msg);
                }
                var wf = SetWorkflowEngine(model);
                priv = wf.GetWorkflowObjectPrivilege();
            }

            if (priv.CanCreate || priv.CanEdit)
            {
                ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
                ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
                ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.CapabilityFramework, ViewData);

                GetErrorsFromTempData();
                CreateLookups();

                GetNumberOfLinkedPositions(id, task);
                if (id == 0)
                {
                    return ReturnMissingRoleDescription();
                }
                else
                {
                    model = Repository.LoadRoleDescWithCapabilityFramework(id);

                    if (model != null)
                    {
                        model.RoleCapabilities = RoleDescriptionExtensions.SortCapabilityGroup(model).ToList();

                        //load matrix
                        var matrix = ServiceRepository.RoleDescCapabilityMatrixRepository()
                            .LoadMatrixByGrade(model.RolePositionDescription.GradeCode);
                        ViewBagWrapper.SetGeneralObject("matrix", matrix, ViewData);
                        SetWorkflowEngine(model);
                        return View("Manage", model);
                    }
                    else
                    {
                        model = Repository.GetEmptyModel();
                        ViewBagWrapper.VariableBag.SetBoolVariable("MissingRoleDesc", true, ViewData);
                        SetWorkflowEngine(model);
                        return View("Manage", model);
                    }
                }
            }
            else
            {
                var msg = MessageHelper.AccessDeniedMessage();
                throw new AccessDeniedException(msg);
            }
        }

        [HasAnyAdminRole]
        [System.Web.Mvc.HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult AddUpdateRoleCapabilities(int id, bool valid=true)
        {
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            Enums.Privilege priv = null;
            RoleDescription model = null;
            if (id == 0)
            {
                priv = task.GetRoleDescriptionPrivilege();
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
            }
            else
            {
                model = Repository.GetRoleDescriptionById(id);
                if (model == null)
                {
                    var msg = MessageHelper.NotFoundMessage("role description");
                    throw new HttpException(msg);
                }
                var wf = SetWorkflowEngine(model);
                priv = wf.GetWorkflowObjectPrivilege();
            }

            if (priv.CanCreate || priv.CanEdit)
            {
                ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
                ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
                ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.UpdateCapabilityFramework, ViewData);
                GetErrorsFromTempData();
                GetNumberOfLinkedPositions(id, task);
                if (id == 0)
                {
                    return ReturnMissingRoleDescription();
                }
                else
                {
                    model = Repository.LoadRoleDescWithCapabilityFramework(id);
                    if (model == null)
                    {
                        var msg = MessageHelper.NotFoundMessage("role description");
                        throw new Exception(msg);
                    }
                    model.RoleCapabilities = RoleDescriptionExtensions.SortCapabilityGroup(model).ToList();
                    //Remove 'Occupation Specific' from the list
                    var capabilityGroupList = ServiceRepository.RoleCapabilityRepository().GenerateCapabilityModel(id);
                    ViewBagWrapper.ListBag.SetCapabilityGroupList(capabilityGroupList, ViewData);

                    var matrix = ServiceRepository.RoleDescCapabilityMatrixRepository()
                            .LoadMatrixByGrade(model.RolePositionDescription.GradeCode);

                    ViewBagWrapper.SetGeneralObject("matrix", matrix, ViewData);

                    LoadSetCapabilityLevels(matrix);
                    SetWorkflowEngine(model);
                    return View("Manage", model);
                }
            }
            else
            {
                var msg = MessageHelper.AccessDeniedMessage();
                throw new AccessDeniedException(msg);
            }
            
        }

        private void LoadSetCapabilityLevels(RoleDescCapabilityMatrix matrix)
        {
            var capabilityLevelItemsFinal = ServiceRepository.CapabilityLevelRepository()
                .LoadCapabilityLevelsForRoleDesc(matrix)
                .OrderBy(l => l.LevelOrder).ToArray()
               .Select(pe => new SelectListItemExtension { Value = pe.CapabilityLevelId.ToString(), Text = pe.LevelName })
               .ToArray();

            ViewBagWrapper.ListBag.SetList("capabilityLevelItems", capabilityLevelItemsFinal, ViewData);
        }


        //Save the capabilities
        [System.Web.Mvc.HttpPost]
        [HasAnyAdminRole]
        public ActionResult SaveCapabilities(FormCollection collection)
        {
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.UpdateCapabilityFramework, ViewData);
            var srv = ServiceRepository;
           
            var roleCapabilityModel = new RoleCapabilityModel();
            var resultRoleCapabilites = roleCapabilityModel.ParseBuildRoleCapabilityList(collection, srv);

            ViewBagWrapper.FormOperations.SetFormType(resultRoleCapabilites.RoleDescriptionId == 0 ? FormType.Create : FormType.Edit, ViewData);
            var model = Repository.LoadRoleDescWithCapabilityFramework(resultRoleCapabilites.RoleDescriptionId);
            
            if (model == null)
            {
                var msg = MessageHelper.NotFoundMessage("role description");
                throw new Exception(msg);
            }

            var matrix = ServiceRepository.RoleDescCapabilityMatrixRepository()
                        .LoadMatrixByGrade(resultRoleCapabilites.GradeCode);
            
            ViewBagWrapper.SetGeneralObject("matrix", matrix, ViewData);
            try
            {
               //Validate capability levels
               var validateResult = RoleCapabilityModel.ValidCapabilityLevels(
                    srv,
                    resultRoleCapabilites.GradeCode, 
                    resultRoleCapabilites.RoleCapabilities,
                    resultRoleCapabilites.IsManager);
               
                if(validateResult.Status == Status.Success)
                {
                    //Save changes - this will update both tables by chaining them into RoleDesc entity
                    var rd = Repository.LoadRoleDescWithRoleCapabilitiesOnly(resultRoleCapabilites.RoleDescriptionId);
                    var oldStatusId = rd.RolePositionDescription.StatusId;
                    rd.RoleCapabilities = resultRoleCapabilites.RoleCapabilities;
                    

                    if (!resultRoleCapabilites.IsManager)
                    {
                          //if it was manager role, now isn't, we need to clear the items in 'People Management'
                            var managementCapNames = srv.CapabilityNameRepository().ListWithGroup()
                            .Where(c => c.CapabilityGroupId == (int)Enums.CapablityGroup.PeopleManagement)
                            .Select(c => c.CapabilityNameId).ToList();
                            rd.RoleCapabilities = rd.RoleCapabilities
                                .Where(x => !managementCapNames.Contains(x.CapabilityNameId)).ToList();
                      
                        
                    }

                    rd.ManagerRole = resultRoleCapabilites.IsManager;

                    Repository.Update(rd);

                    //add to history
                    if (rd.RolePositionDescription.StatusId != (int)Enums.StatusValue.Draft)
                    {
                       StringBuilder sb = new StringBuilder();
                        sb.Append("...");
                        ServiceRepository.RolePositionDescriptionHistoryRepository()
                                .LogHistoryWhenUpdated(resultRoleCapabilites.RoleDescriptionId, oldStatusId, oldStatusId, 
                                sb, "CapabilityFramework", CurrentUser.UserName);
                    }

                    ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.CapabilityFramework,ViewData);
                    //reload to add capability levels properties
                    model = Repository.LoadRoleDescWithCapabilityFramework(resultRoleCapabilites.RoleDescriptionId);
                    SetWorkflowEngine(model);
                    return View("Manage", model);

                }
                else
                {
                    //On validation error
                    //constructing the selected capability list, then pass it to view

                    SetSelectedCapabilities(collection, resultRoleCapabilites);

                    LoadSetCapabilityLevels(matrix);

                      var errors = Repository.GetValidationErrors(ModelState).ToList();
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    errors.Add(new DbValidationError("Form validation ", validateResult.Message));
                   
                    ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);

                    model.ManagerRole = collection["ManagerRole"] == "on";
                    SetWorkflowEngine(model);
                    return View("Manage", model);

                }


            }
            catch (Exception exception)
            {
                LogException(exception);
                var errors = Repository.GetValidationErrors(ModelState).ToList();
                errors.AddRange(Repository.GetBackendValidationErrors());

                var msg = MessageHelper.ErrorOccured();
                errors.Add(new DbValidationError("DB ", msg + exception.Message));
                //errors.Add(new DbValidationError("DB ", "Oops! something went wrong. " + exception.Message));
            }
            
            GetErrorsFromTempData();
            //return View("Manage", model);
            return RedirectToAction("AddUpdateRoleCapabilities", new {id = resultRoleCapabilites.RoleDescriptionId });
            
        }

        //when validation error
        private void SetSelectedCapabilities(FormCollection collection, RoleCapabilityModel resultRoleCapabilites)
        {
            var capabilityGroupList = new List<CapabilityGroupLight>();

            var capGroupItems = ServiceRepository.CapabilityGroupRepository()
                .List().ToList();

            var nameIdList = resultRoleCapabilites.RoleCapabilities.Select(rc => rc.CapabilityNameId);
            var cnameList = new List<CapabilityNameLight>();
            
            
            var cnames = ServiceRepository.CapabilityNameRepository().List().ToList();

            foreach (var cn in cnames)
            {
                var cname = new CapabilityNameLight();

                var selLvlname = "CapabilityLevelId_" + cn.CapabilityNameId;
                var selhigh = "Highlighted_" + cn.CapabilityNameId;
                var highlighted = collection[selhigh].IsOn();

                if (!string.IsNullOrEmpty(collection[selLvlname]) && collection[selLvlname] != "0")
                {
                    if (nameIdList.Contains(cn.CapabilityNameId))
                    {
                        cname.LevelId = Convert.ToInt32(collection[selLvlname]);
                        cname.Highlighted = highlighted;
                    }
                }
                else
                {
                    cname.LevelId = -1;
                    cname.Highlighted = false;
                }

                cname.CapabilityNameId = cn.CapabilityNameId;
                cname.Name = cn.Name;
                cname.Highlighted = highlighted;
                cname.Selected = true;

                var indContext = cn.CapabilityBehaviourIndicators.FirstOrDefault(
                    cb =>
                        cb.CapabilityLevelId == cname.LevelId &&
                        cb.CapabilityNameId == cname.CapabilityNameId);

                cname.IndContext = indContext != null ? indContext.IndicatorContext : "";
                cname.CapabilityGroupId = cn.CapabilityGroupId;
                cname.CapabilityGroupName = cn.CapabilityGroup.GroupName;

                cnameList.Add(cname); //CapabilityName list
            }

            //now add CapabilityNames to capabilityGroupList
            foreach (var g in capGroupItems)
            {
                var capGrp = new CapabilityGroupLight();
                capGrp.CapabilityGroupId = g.CapabilityGroupId;
                capGrp.GroupName = g.GroupName;
                capGrp.GroupDescription = g.GroupDescription;
                capGrp.CapabilityNames = cnameList.Where(c => c.CapabilityGroupId == g.CapabilityGroupId).OrderBy(c => c.Name).ToList();

                capabilityGroupList.Add(capGrp);
            }
            ViewBagWrapper.ListBag.SetCapabilityGroupList(capabilityGroupList, ViewData);
        }


        [System.Web.Mvc.HttpGet]
        [HasAnyAdminRole]
        public ActionResult ManageKeyRelationships(int id)
        {
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            Enums.Privilege priv = null;
            RoleDescription model = null;
            if (id == 0)
            {
                priv = task.GetRoleDescriptionPrivilege();
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
            }
            else
            {
                model = Repository.GetRoleDescriptionById(id);
                if (model == null)
                {
                    var msg = MessageHelper.NotFoundMessage("role description");
                    throw new HttpException(msg);
                }
                var wf = SetWorkflowEngine(model);
                priv = wf.GetWorkflowObjectPrivilege();
            }

            if (priv.CanCreate || priv.CanEdit)
            {
                ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
                ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
                ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.KeyRelationships, ViewData);
                GetErrorsFromTempData();
                CreateLookups();
                GetNumberOfLinkedPositions(id, task);
                if (id == 0)
                {
                    return ReturnMissingRoleDescription();
                }
                else
                {
                    model = Repository.LoadRoleDescWithKeyRelationships(id);
                    if (model == null)
                    {
                        return ReturnMissingRoleDescription();
                    }
                    SetWorkflowEngine(model);
                    return View("Manage", model);
                }
            }
            else
            {
                var msg = MessageHelper.AccessDeniedMessage();
                throw new AccessDeniedException(msg);
            }

        }

        //datatable for key relationships
        [HasAnyAdminRole]
        public ActionResult ListJsonKeyRelationships([FromUri] JQueryDataTableParamModel arg, int id)
        {
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
            ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.KeyRelationships, ViewData);
            GetErrorsFromTempData();
            CreateLookups();
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            GetNumberOfLinkedPositions(id, task);

            if (id == 0)
            {
                var model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
                return View("Manage", model);
            }
            else
            {
                try
                {
                    var model = Repository.LoadRoleDescWithKeyRelationships(id);
                    if (model == null)
                    {
                        return ReturnMissingRoleDescription();
                    }
                    SetWorkflowEngine(model);

                    var keyRelationships = model.KeyRelationships.AsQueryable();
                    IQueryable<KeyRelationship> displayedKeyRelationships;

                    if (!string.IsNullOrWhiteSpace(arg.sSearch))
                    {
                        var searchArgs = new SearchArg { Search = arg.sSearch };
                        displayedKeyRelationships = ServiceRepository.KeyRelationshipRepository()
                            .FilterKeyRelationships(keyRelationships, searchArgs);
                    }
                    else
                    {
                        displayedKeyRelationships = CustomOrderBy.CustomSort(keyRelationships, arg);
                    }

                    var totalRecord = displayedKeyRelationships.Count();
                    var totalDisplayRecord = displayedKeyRelationships.Count();

                    if (arg.iDisplayLength == -1)
                        arg.iDisplayLength = totalRecord;

                    displayedKeyRelationships = displayedKeyRelationships.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);

                    var result = displayedKeyRelationships.AsEnumerable().ToArray().Select(ent => ent.To(new KeyRelationshipLight
                    {
                        ScopeName = ent.RelationshipScope.ScopeTitle,
                        Who = ent.Who,
                        Why = ent.Why,
                        LastUpdated = ent.LastUpdated
                    }));

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
                    dataTableResult.AddError(new DbValidationError("Error",
                        msg + exception.Message));
                    //dataTableResult.AddError(new DbValidationError("Error",
                    //    "Oops! something went wrong. " + exception.Message));
                }

                return Json(dataTableResult, JsonRequestBehavior.AllowGet);


                //var model = Repository.LoadRoleDescWithKeyRelationships(id);
                //if (model == null)
                //{
                //    return ReturnMissingRoleDescription();
                //}
                //SetWorkflowEngine(model);
                //return View("Manage", model);
            }



           
        }

        [HasDoeRole]
        public ActionResult ManageLinkedPositions(int id)
        {
            ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.LinkedPositions, ViewData);
            GetErrorsFromTempData();
            CreateLookups();
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            GetNumberOfLinkedPositions(id,task);
            if (id == 0)
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
                var model = Repository.GetRoleDescriptionById(id);
                if (model == null)
                {
                    return ReturnMissingRoleDescription();
                    }
                
                var linkedPositions = GetLinkedPositions(model.RoleDescriptionId,task);
                  
                ViewBagWrapper.SetGeneralObject("linkedPositions", linkedPositions, ViewData);
                SetWorkflowEngine(model);
                return View("Manage", model);
            }
        }

        

        [HasAnyAdminRole]
        [System.Web.Mvc.HttpGet]
        public ActionResult ManageActions(int id)
        {
            ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.Actions, ViewData);
            GetErrorsFromTempData();
            CreateLookups();
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            GetNumberOfLinkedPositions(id,task);

            if (id == 0)
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
                var model = Repository.GetRoleDescriptionById(id);

                if (model == null)
                {
                    return ReturnMissingRoleDescription();
                    }

                var roleDescTasks = PosRdPdFactory.Create(model).BuildTasks();
                if (roleDescTasks.Any())
                {
                    ViewBagWrapper.ValidationTaskBag.SetTasks(roleDescTasks, ViewData);

                }
                SetWorkflowEngine(model);
                return View("Manage", model);
            }
        }

      
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [HasAnyAdminRole]
        public ActionResult SubmitRoleDesc()
        {
            int id = Convert.ToInt32(Request["RoleDescriptionId"]);
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            GetNumberOfLinkedPositions(id,task);
            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.Actions, ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();

            var roleDescription = Repository.GetRoleDescriptionById(id);
            
            if (roleDescription == null)
            {
                var msg = MessageHelper.NotFoundMessage("role description");
                throw new HttpException(msg);
            }
            var roleDescTasks = PosRdPdFactory.Create(roleDescription).BuildTasks();
            if (!roleDescTasks.Any())
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    if (roleDescription.RolePositionDescription.StatusId == (int) Enums.StatusValue.Submitted)
                    {
                        ServiceRepository.RolePositionDescriptionRepository()
                            .UpdateStatus(id, Enums.StatusValue.Approved);
                        //add to history
                        sb.Clear();
                        sb.Append("Status has changed from Submitted to Approved for this role desc.");
                        ServiceRepository.RolePositionDescriptionHistoryRepository()
                            .LogHistoryWhenUpdated(id, (int) Enums.StatusValue.Submitted,
                                (int) Enums.StatusValue.Approved,
                                sb, "Status", CurrentUser.UserName);

                    }
                    if (roleDescription.RolePositionDescription.StatusId == (int) Enums.StatusValue.Draft)
                    {
                        ServiceRepository.RolePositionDescriptionRepository()
                            .UpdateStatus(id, Enums.StatusValue.Submitted);
                        //add to history
                        sb.Clear();
                        sb.Append("Status has changed from Draft to Submitted for this role desc.");
                        ServiceRepository.RolePositionDescriptionHistoryRepository()
                            .LogHistoryWhenUpdated(id, (int) Enums.StatusValue.Draft, (int) Enums.StatusValue.Submitted,
                                sb, "Status", CurrentUser.UserName);
                    }

                   
                }
                catch (Exception e)
                {
                    LogException(e);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    var msg = MessageHelper.ErrorOccured();
                    errors.Add(new DbValidationError("", msg + e.Message));
                    //errors.Add(new DbValidationError("", "Oops! something went wrong. " + e.Message));
                }


            }

            return RedirectToAction("ManageSummary", new { id });
            

        }

       

        ////PDF download for Role Desc only (without position linked)
        //[HasDoeRole]
        //public ActionResult RoleDescOnlyPdf(int id)
        //{
        //    var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
        //    GetNumberOfLinkedPositions(id,task);
        //    ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.Summary,
        //         ViewData);
        //    ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);


        //    var rolePosDesc = ServiceRepository.RolePositionDescriptionRepository()
        //        .ListForRoleDescriptions().FirstOrDefault(rp => rp.RolePositionDescId == id);

        //    if (rolePosDesc == null)
        //    {
        //        var msg = MessageHelper.NotFoundMessage("role description");
        //        throw new HttpException(msg);
        //    }
            
        //    //Generating PDF
        //    var srv = ServiceRepository.PdfService();

        //    var outputPath = PositionEstablishmentSettings.Site.PdfOutputPath;
        //    var inputFilePath = PositionEstablishmentSettings.Site.PdfTemplatePath;

        //    if (!Directory.Exists(outputPath) || !Directory.Exists(inputFilePath))
        //    {
        //        throw new DirectoryNotFoundException("The folders for PDF templates or output didn't exist. Please check the config file then create the folders.");
        //    }

        //    var templateFile = inputFilePath + "RD_only_Template.html";
        //    var cssFile = inputFilePath + "PDFGenerator.css";

        //    string htmlText = srv.ReadFileToText(templateFile);
        //    string cssText = srv.ReadFileToText(cssFile);

        //    StringBuilder sb = new StringBuilder();
        //    var logo = srv.GetImagePath("dec-pdf.png");
        //    //filling the value in htmlText

        //    //1. top position table
        //    htmlText = htmlText.Replace("[Logo]", logo);
        //    htmlText = htmlText.Replace("[PosTitle]", rolePosDesc.Title);
        //    htmlText = htmlText.Replace("[Agency]", rolePosDesc.RoleDescription.Agency);

        //    htmlText = htmlText.Replace("[GradeCode]", rolePosDesc.GradeCode);
            
        //    htmlText = htmlText.Replace("[ANZSCO]", rolePosDesc.RoleDescription.ANZSCOCode);
        //    htmlText = htmlText.Replace("[PCAT]", rolePosDesc.RoleDescription.PCATCode);
        //    htmlText = htmlText.Replace("[DateApproved]", rolePosDesc.DateOfApproval.ToEasyDateFormat());
        //    htmlText = htmlText.Replace("[AgencyWebsite]", rolePosDesc.RoleDescription.AgencyWebsite);

        //    //2. paragrahs
        //    htmlText = htmlText.Replace("[AgencyOverview]", rolePosDesc.RoleDescription.AgencyOverview);

        //    //htmlText = htmlText.Replace("[DivisionOverview]", rolePosDesc.RoleDescription.DivisionOverview);

        //    htmlText = htmlText.Replace("[BusinessOverview]", "??");

        //    htmlText = htmlText.Replace("[PrimaryPurposse]", rolePosDesc.RoleDescription.RolePrimaryPurpose);
        //    htmlText = htmlText.Replace("[KeyAccountabilities]", rolePosDesc.RoleDescription.KeyAccountabilities);


        //    htmlText = htmlText.Replace("[KeyChallenges]", rolePosDesc.RoleDescription.KeyChallenges);

        //    //build keyrelationships table
        //    sb.Clear();
        //    foreach (var key in rolePosDesc.RoleDescription.KeyRelationships.Where(k => k.ScopeId == 10))
        //    {
        //        sb.Append("<tr class=\"body\">");
        //        sb.Append("<td>" + key.Who + "</td>");
        //        sb.Append("<td>" + key.Why + "</td>");
        //        sb.Append("</tr>");
        //    }
        //    htmlText = htmlText.Replace("[InternalBody]", sb.ToString());

        //    sb.Clear();
        //    foreach (var key in rolePosDesc.RoleDescription.KeyRelationships.Where(k => k.ScopeId == 20))
        //    {
        //        sb.Append("<tr class=\"body\">");
        //        sb.Append("<td>" + key.Who + "</td>");
        //        sb.Append("<td>" + key.Why + "</td>");
        //        sb.Append("</tr>");
        //    }
        //    htmlText = htmlText.Replace("[ExternalBody]", sb.ToString());
        //    htmlText = htmlText.Replace("[ReportingLine]", "Nil");
        //    htmlText = htmlText.Replace("[DirectReports]", "Nil");


        //    sb.Clear();

        //    //[BudgetExp], [BudgetExpValue]
        //    htmlText = htmlText.Replace("[BudgetExp]", !string.IsNullOrEmpty(rolePosDesc.RoleDescription.BudgetExpenditure) 
        //        ? "Nil" : rolePosDesc.RoleDescription.BudgetExpenditure);
        //    htmlText = htmlText.Replace("[BudgetExpValue]", rolePosDesc.RoleDescription.BudgetExpenditureValue);

        //    htmlText = htmlText.Replace("[EssentialReqs]", rolePosDesc.RoleDescription.EssentialRequirements);
        //    htmlText = htmlText.Replace("[CapIntro]", rolePosDesc.RoleDescription.RoleCapabilityItems);
        //    htmlText = htmlText.Replace("[CapSummary]", rolePosDesc.RoleDescription.CapabilitySummary);
        //    htmlText = htmlText.Replace("[FocusCapabilities]", rolePosDesc.RoleDescription.FocusCapabilities);
            
        //    //Build capability framework table
        //    var groupedList = rolePosDesc.RoleDescription.RoleCapabilities.GroupBy(rc => rc.CapabilityName.CapabilityGroup.GroupName)
        //        .ToDictionary(g => g.Key, g => g.ToList());
            
        //    var count = 0;
        //    var bold = "";
        //    foreach (var obj in groupedList)
        //    {
        //        var image = PositionEstablishmentSettings.Site.PdfTemplatePath +
        //                    srv.GetCapabilityGroupImageName(obj.Value);
        //        count = 1;
        //        foreach (var x in obj.Value)
        //        {
        //            bold = "";
        //            sb.Append("<tr class=\"body\">");
        //            if (count == 1)
        //            {
        //                sb.Append("<td rowspan=\"" + obj.Value.Count + "\"><img src=\"" + image + "\" width=\"130\" height=\"130\" alt=\"" + obj.Key + "\" /></td>");
        //             }

        //            if (x.Highlighted)
        //            {
        //                bold = " class=\"bold\"";
        //            }
        //            sb.Append("<td" + bold + ">" + x.CapabilityName + "</td>");
        //            sb.Append("<td" + bold + ">" + x.CapabilityLevel.LevelName + "</td>");
        //            sb.Append("</tr>");
        //            count++;
        //        }
        //    }
        //    count = 0;
        //    //Console.Write(sb.ToString());
        //    htmlText = htmlText.Replace("[CapFrameTable]", sb.ToString());
            
        //    //Build Framework indicator table
        //    sb.Clear();

        //    foreach (var c in rolePosDesc.RoleDescription.RoleCapabilities)
        //    {
        //        if (c.Highlighted)
        //        {
        //            sb.Append("<tr class=\"body\">");
        //            sb.Append("<td><b>" + c.CapabilityName.CapabilityGroup.GroupName + "</b><p>" + c.CapabilityName + "</p></td>");
        //            sb.Append("<td>" + c.CapabilityLevel.LevelName + "</td>");
        //            sb.Append("<td>" + c.CapabilityName.CapabilityBehaviourIndicators.FirstOrDefault(rc => rc.CapabilityLevelId == c.CapabilityLevelId && rc.CapabilityNameId == c.CapabilityNameId) + "</td>");
        //            sb.Append("</tr>");
        //        }
        //    }

        //    //Console.Write(sb.ToString());
        //    htmlText = htmlText.Replace("[FocusIndicators]", sb.ToString());


        //    htmlText = htmlText.Replace("[DOCNumber]", rolePosDesc.DocNumber);
        //    htmlText = htmlText.Replace("[lastUpdated]", rolePosDesc.LastModifiedDate.ToShortDateString());


        //    var outputFileName = srv.GetPdfFileName(rolePosDesc.DocNumber, rolePosDesc.Title, rolePosDesc.GradeCode);
        //    string outputFile = outputPath + outputFileName;

        //    srv.GeneratePdfFromHTML(htmlText, cssText, outputFile);

        //    return File(outputFile, MediaTypeNames.Application.Pdf, outputFileName);
        //}


        [HasDoeRole]
        public ActionResult OpenOldDescFile(string oldDescFileName)
        {
            string path = ProjectTemplateSettings.Site.OldDescriptionFileFolder+ oldDescFileName;

            if (System.IO.File.Exists(path))
            {
                var fileStream = new FileStream(path,
                    FileMode.Open,
                    FileAccess.Read
                    );
                FileInfo info = new FileInfo(oldDescFileName);
                
                FileStreamResult fsResult;

                if (info.Extension == ".pdf")
                    fsResult = new FileStreamResult(fileStream, "application/pdf");
                else if (info.Extension == ".DOCX")
                    fsResult = new FileStreamResult(fileStream,
                        "application/vnd.openxmlfocrmats-officedocument.wordprocessingml.document")
                    { FileDownloadName = oldDescFileName };
                else
                    fsResult = new FileStreamResult(fileStream, "application/vnd.ms-word") { FileDownloadName = oldDescFileName };
                return fsResult;
               
            }
            else
                return null;
        }
        
        [System.Web.Mvc.HttpGet]
        [HasAnyAdminRole]
        public ActionResult WfHistory(int id)
        {
            ViewBagWrapper.RoleDescBag.SetRoleDescWizardBag(Enums.RoleDescWizardStep.History, ViewData);
           
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            GetNumberOfLinkedPositions(id,task); 
            if (id == 0)
            {
                var model = Repository.GetEmptyModel();
                
                return View("Manage", model);
            }
            else
            {
                var model = Repository.GetRoleDescriptionById(id);

                if (model == null)
                {
                    return ReturnMissingRoleDescription();
                    }

                var historyList =
               ServiceRepository.RolePositionDescriptionHistoryRepository().List().Where(r=>r.RolePositionDescId == id);

                ViewBagWrapper.SetGeneralObject("historyList", historyList, ViewData);
                SetWorkflowEngine(model);
                return View("Manage", model);
            }
        }

        public ActionResult GetWorkflowAction(int wfObjectId, WorkflowObjectType objectType, int actionId)
            {
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            RoleDescription roleDescription;
            if(objectType == WorkflowObjectType.RoleDescription)
                {
                roleDescription = Repository.GetRoleDescriptionById(wfObjectId);
                this.Repository.RolePositionDescriptionRepository.LoadNavigationProperty(roleDescription.RolePositionDescription,e=>e.Positions);
                SetWorkflowEngine(roleDescription, task);
                }
            else
                {
                throw new InvalidOperationException("position description workflow should have object type Position description");
                }

            var action = new WorkflowAction { ActionId = actionId };
            WorkflowAction.Populate(this.Repository.RepositoryFactory, action);
            if(action == WorkflowAction.Rename)
                {
                return View("WorkflowAction-Rename-Modal", action);
                }

            if(action == WorkflowAction.MovePositions)
                {

                var positionLights = GetPositionList(roleDescription);

                ViewBagWrapper.PositionBag.SetPositionListModel(positionLights, ViewData);
                return View("WorkflowAction-MovePositions-Modal", action);
                }

            return View("WorkflowAction-Modal", action);
            }

        [System.Web.Mvc.HttpPost]
        public ActionResult ApplyWorkflowAction(WorkflowActionModel model)
            {
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);

            var ajaxResult = new Result();
            try
                {
                if(model.ObjectType == WorkflowObjectType.RoleDescription)
                    {
                    var roleDescription = Repository.GetRoleDescriptionById(model.WfObjectId);
                    var workflowEngine = WorkflowEngineFactory.CreatEngine(roleDescription, task);
                    ajaxResult = workflowEngine.ApplyAction(model, true,Request.Form);
                    }
                else
                    {
                    throw new InvalidOperationException("Object type must be Position description");
                    }


                }
            catch(Exception exception)
                {
                LogException(exception);

                var errors = Repository.GetBackendValidationErrors().ToList();
                var msg = MessageHelper.ErrorOccured();
                errors.Add(new DbValidationError("", msg + exception.Message));
                //errors.Add(new DbValidationError("", "Oops! something went wrong. " + exception.Message));

                if((Request.IsAjaxRequest()))
                    {

                    return Json(ajaxResult);
                    }
                else
                    {
                    ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                    }
                }

            CreateLookups();
            AppCacheHelper.Expire(Enums.CacheRegion.Position);
            if(Request.IsAjaxRequest())
                {
              return Json(ajaxResult);
                }

            return RedirectToAction("Index");
            }
        private List<PositionLight> GetPositionList(RoleDescription rolesDescription)
            {
            var positionLights = new List<PositionLight>();

            var positions =
                this.Repository.ServiceRepository.PositionRepository()
                    .BaseList()
                    .Where(p =>p.StatusId!=-1&&  p.RolePositionDescriptionId == rolesDescription.RoleDescriptionId)
                    .ToList();


            foreach(var p in positions)
                {
                positionLights.Add(new PositionLight
                    {
                    PositionId = p.PositionId,
                    PositionNumber = p.PositionNumber,
                    PositionTitle = p.PositionTitle,
                    StatusName = p.StatusValue.StatusName
                    });
                }
            return positionLights;
            }
        }
}