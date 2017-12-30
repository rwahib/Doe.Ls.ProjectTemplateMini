using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Http;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Doe.Ls.ProjectTemplate.Data;
using ActionNameAttribute = System.Web.Http.ActionNameAttribute;
using CommonHelper = Doe.Ls.ProjectTemplate.Core.BL.UI.CommonHelper;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {
    public partial class PositionDescriptionController
        {
        public PositionDescriptionController()
            {
            }

        public PositionDescriptionController(ServiceRepository srv)
            {
            _serviceRepository = srv;
            _repository = srv.PositionDescriptionRepository();
            }

        // GET: PositionDescriptionController_partial
        [HasAnyAdminRole]
        [HttpGet]
        public ActionResult ManageOverview(int id)
        {
            var model = Repository.GetPositionDescriptionById(id);
            InitMessagesList();
            ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.Overview, ViewData);
            GetErrorsFromTempData();
            CreateLookups();

            if(model!=null)
            {
                var wf = SetWorkflowEngine(model);
                if (!wf.GetWorkflowObjectPrivilege().CanEdit)
                    {
                        return RedirectToAction("ManageSummary", new {id = id});
                    }
                SetNumberOfLinkedPositions(id);
                ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);

                }
            
         
            if(id == 0 || model==null) // new
            {
                model = Repository.GetEmptyModel();
                
                ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);

                SetWorkflowEngine(model);
                return View("Manage", model);
                }
            SetWorkflowEngine(model);
            return View("Manage", model);

            }
        

        [HasAnyAdminRole]
        [HttpGet]
        public ActionResult ListSelectionCriteria(int id)
            {
            var model = Repository.GetPositionDescriptionById(id);
            var wf = SetWorkflowEngine(model);


            if(!wf.GetWorkflowObjectPrivilege().CanEdit)
                {
                return RedirectToAction("ManageSummary", new { id = id });

                }
            SetNumberOfLinkedPositions(id);
            ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.SelectionCriteria,
                ViewData);
            ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
            model = Repository.LoadPositionDescById(id);
            
            if (model == null)
            {
                return ReturnMissingPositionDescription();
            }
                
           SetWorkflowEngine(model);
           return View("Manage", model);
            }


        [HasAnyAdminRole]
        [HttpGet]
        public ActionResult ManageSelectionCriteria(int id)
            {
            var model = Repository.GetPositionDescriptionById(id);
            var wf = SetWorkflowEngine(model);

            if(!wf.GetWorkflowObjectPrivilege().CanEdit)
                {
                return RedirectToAction("ManageSummary", new { id = id });

                }
            ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.SelectionCriteria,
                ViewData);
            SetNumberOfLinkedPositions(id);
            ViewBagWrapper.FormOperations.SetFormType(id == 0 ? FormType.Create : FormType.Edit, ViewData);
             
            model = Repository.LoadPositionDescById(id);

            if(model == null)
                {
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
                return View("Manage", model);
                }
            

            ViewBagWrapper.VariableBag.SetBoolVariable("IsEditCriteria", true, ViewData);

            var selectionCriteriaItems =
                ServiceRepository.LookupFocusGradeCriteriaRepository()
                    .List()
                    .Where(l => l.GradeCode == model.RolePositionDescription.GradeCode)
                    .ToArray();
            ViewBagWrapper.PositionDescBag.SetSelectionCriteria("selectionCriteriaItems", selectionCriteriaItems,
                ViewData);

            SetWorkflowEngine(model);
            return View("Manage", model);
            }

        [HasAnyAdminRole]
        [HttpPost]
        public ActionResult SaveSelectionCriteria(FormCollection collection)
            {
            var errors = new List<DbValidationError>();
            var posDescriptionId = Convert.ToInt32(collection["PositionDescriptionId"]);
            var positionDescription = Repository.List().FirstOrDefault(l => l.PositionDescriptionId == posDescriptionId);
            var selectionCriteriaItems =
                ServiceRepository.LookupFocusGradeCriteriaRepository()
                    .List()
                    .Where(l => l.GradeCode == positionDescription.RolePositionDescription.GradeCode)
                    .ToArray();
            try
                {
                var selFocusList = new List<PositionFocusCriteria>();
                foreach(var criteria in selectionCriteriaItems)
                    {
                    var selected = Request["item_" + criteria.LookupId].IsOn();
                    if((criteria.SelectionCriteria.Criteria != Enums.Cnt.Custom) &&(selected || criteria.IsMandatory))
                        {
                        var positionFocusCriteria = new PositionFocusCriteria
                            {
                            LookupId = criteria.LookupId,
                            PositionDescriptionId = posDescriptionId,
                            LastModifiedDate = DateTime.Now,
                            LastModifiedBy = CurrentUser.UserName
                            };
                        selFocusList.Add(positionFocusCriteria);
                        }

                    }

                var customContent = collection["customCriteria"];
                
                var item_custom_chk = collection["CustomChkBoxVal"]=="true"?true:false;
                if(item_custom_chk && !string.IsNullOrEmpty(customContent))
                    {
                    var lukupId =
                        selectionCriteriaItems.FirstOrDefault(s => s.SelectionCriteria.Criteria == Enums.Cnt.Custom).LookupId;
                    var positionFocusCriteria = new PositionFocusCriteria
                        {
                        LookupId = lukupId,
                        PositionDescriptionId = posDescriptionId,
                        LookupCustomContent = customContent,
                        LastModifiedDate = DateTime.Now,
                        LastModifiedBy = CurrentUser.UserName
                        };
                    selFocusList.Add(positionFocusCriteria);

                    }

                ServiceRepository.PositionFocusCriteriaRepository()
                  .UpdatePositionFocusCriteria(posDescriptionId, selFocusList);

                if(selFocusList.Count < 8 || selFocusList.Count > 10)
                    {
                    throw new InvalidOperationException("Please select minimum 8 and maximum 10 selection criteria.");

                    }
              

                //add to history
                    var rpd =
                        ServiceRepository.RolePositionDescriptionRepository()
                            .BaseList()
                            .SingleOrDefault(r => r.RolePositionDescId == posDescriptionId);

                if (rpd.StatusId != (int)Enums.StatusValue.Draft)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("...");
                    ServiceRepository.RolePositionDescriptionHistoryRepository()
                            .LogHistoryWhenUpdated(posDescriptionId, rpd.StatusId, rpd.StatusId,
                            sb, "SelectionCriterias", CurrentUser.UserName);
                }


                return RedirectToAction("ListSelectionCriteria", new { id = posDescriptionId });
                }
            catch(Exception exception)
                {
                LogException(exception);
                errors.AddRange(Repository.GetBackendValidationErrors());
                errors.Add(new DbValidationError("", "Validation error  " + exception.Message));

                }

            ViewData["Errors"] = errors;
            TempData["Errors"] = errors;
            return RedirectToAction("ManageSelectionCriteria", new { id = posDescriptionId });
            }


        
        [HttpGet]
        public ActionResult ManageLinkedPositions(int id)
            {
            

            ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.LinkedPositions,
                 ViewData);
            SetNumberOfLinkedPositions(id);
            var model = Repository.LoadPositionDescById(id);

            if(model == null)
                {
                return ReturnMissingPositionDescription();
                }

            var task = UserTaskFactory.GetTask(CurrentUser, Repository.RepositoryFactory);

            var linkedPositions =GetLinkedPositions(id,task);

            ViewBagWrapper.SetGeneralObject("linkedPositions", linkedPositions, ViewData);
            SetWorkflowEngine(model);
            return View("Manage", model);
            }

        private ActionResult ReturnMissingPositionDescription()
        {
            var model = Repository.GetEmptyModel();
            SetWorkflowEngine(model);
            return View("Manage", model);
        }

        private RolePositionDescription ValidateAndSetRolePosDescObject(
            ref RolePositionDescription rolePositionDescription)
            {
            InitialiseTask();
            var task = ViewBagWrapper.TaskBag.GetCurrentTask(ViewData);

            if(!task.CanEditPositionDescription(rolePositionDescription.RolePositionDescId))
                {
                return rolePositionDescription;
                }

            var title = Request["RolePositionDescription.Title"];
            var grade = Request["RolePositionDescription.GradeCode"];
           
           var modelState = task.GetRolePosModelState(rolePositionDescription.RolePositionDescId);
            if(string.IsNullOrEmpty(title))
                {
                var msg = MessageHelper.NullPleaseEnterMessage("Position title");
                throw new InvalidOperationException(msg);
                }
            
            else if(modelState.GradeEnabled && string.IsNullOrEmpty(grade))
                {
                var msg = MessageHelper.NullPleaseSelectMessage("Grade");
                throw new InvalidOperationException(msg);
                }



            rolePositionDescription.Title = title;
            rolePositionDescription.DocNumber = CommonHelper.ConstructDocNumber(Request);//docNum;
            if(modelState.GradeEnabled) rolePositionDescription.GradeCode = grade;
        
            return rolePositionDescription;

            }
        


        private void SetNumberOfLinkedPositions(int id)
        {
            var task = UserTaskFactory.GetTask(CurrentUser, ServiceRepository.RepositoryFactory);

            var positions = GetLinkedPositions(id,task);
            var count = positions.Count();          
            ViewBagWrapper.VariableBag.SetIntVariable("LinkedPositionsCount", count, ViewData);
            }


        [HttpGet]
        public ActionResult ManageSummary(int id)
            {
            SetNumberOfLinkedPositions(id);
            ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.Summary,
                 ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var model = Repository.LoadPositionDescById(id);
            if(model == null)
                {
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
                return View("Manage", model);
                }

            SetWorkflowEngine(model);
            return View("Manage", model);

            }

        [HttpGet]
        [MustHaveAdminOrPowerRole]
        public ActionResult Trim(int id) {
            
            ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.Trim,
                 ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var model = Repository.LoadPositionDescById(id);
            if(model == null) {
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
                return View("Manage", model);
                }

            var recordInfo=ServiceRepository.TrimRecordRepository().GetRecordInfoModel(id, true);
            ViewBagWrapper.SetGeneralObject("TrimRecordInfo",recordInfo,ViewData);
            SetWorkflowEngine(model);
            return View("Manage", model);

            }

        [MustHaveAdminOrPowerRole]
        [HttpPost, System.Web.Mvc.ActionName("Trim")]
        public ActionResult TrimConfirm(int id)
        {

         var comment = $"Re synced from {CurrentUser.DisplayName} at {DateTime.Now}";
         ServiceRepository.TrimRecordRepository().SynchRolePosDescription(id, comment);

            return RedirectToAction("Trim",new { id});

        }

        
        [HttpGet]
        public ActionResult ManageActions(int id)
            {
            ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.Actions,
              ViewData);            
            var model = Repository.LoadPositionDescById(id);

            if(model == null)
                {
                model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
                return View("Manage", model);
                }

            if(model.IsDeleted())
                {
                return RedirectToAction("ManageSummary", new { id });

                }
            
            var wf = SetWorkflowEngine(model);
            SetNumberOfLinkedPositions(id);
            return View("Manage", model);
            
            }

        public ActionResult GetWorkflowAction(int wfObjectId, WorkflowObjectType objectType, int actionId)
            {
            var task = UserTaskFactory.GetTask(CurrentUser, Repository.RepositoryFactory);
            var positionDescription = new PositionDescription();
            if (objectType == WorkflowObjectType.PositionDescription)
            {
                 positionDescription = Repository.GetPositionDescriptionById(wfObjectId);
                Repository.RolePositionDescriptionRepository.LoadNavigationProperty(positionDescription.RolePositionDescription, e => e.Positions);
                SetWorkflowEngine(positionDescription, task);
            }
            else
            {
                throw new InvalidOperationException("position description workflow should have object type Position description");
            }

            var action = new WorkflowAction { ActionId = actionId };
            
            if (action.ActionId == (int)Enums.WorkflowActions.Clone)
            {
                var clone = new CloneActionModel();
                if (positionDescription.PositionDescriptionId > 0)
                {
                    clone.SourcePositionDesc = positionDescription;
                    clone.SourceRolePositionDescId = positionDescription.PositionDescriptionId;
                    return View("ClonePositionDesc", clone);
                }
                else
                {
                    var msg = MessageHelper.NotFoundMessage("source position description");
                    throw new InvalidOperationException(msg);
                }
            }

            WorkflowAction.Populate(Repository.RepositoryFactory, action);

            if (action == WorkflowAction.Rename)
            {
                return View("WorkflowAction-Rename-Modal", action);
           }

            if(action == WorkflowAction.MovePositions)
                {

                var positionLights = GetPositionList(positionDescription);

                ViewBagWrapper.PositionBag.SetPositionListModel(positionLights, ViewData);
                return View("WorkflowAction-MovePositions-Modal", action);
                }

            return View("WorkflowAction-Modal", action);
            }

      

        [HttpPost]
        public ActionResult ApplyWorkflowAction(WorkflowActionModel model)
            {
            var task = UserTaskFactory.GetTask(CurrentUser, Repository.RepositoryFactory);

            var ajaxResult = new Result();
            try
                {
                    if (model.ObjectType == WorkflowObjectType.PositionDescription)
                    {
                        var positionDescription = Repository.GetPositionDescriptionById(model.WfObjectId);
                        var workflowEngine = WorkflowEngineFactory.CreatEngine(positionDescription, task);
                      ajaxResult=  workflowEngine.ApplyAction(model, true, Request.Form);
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
            AppCacheHelper.Expire(Enums.CacheRegion.Position);
            if(Request.IsAjaxRequest())
                {
                    return Json(ajaxResult);
                }

            return RedirectToAction("Index");
            }


        //[System.Web.Mvc.HttpGet]
        ////This is PositionDesc PDF only (without position linked)
        //public ActionResult PositionDescOnlyPdf(int id)
        //    {
        //    SetNumberOfLinkedPositions(id);
        //    ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.Summary,
        //         ViewData);
        //    ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);


        //    var rolePosDesc = ServiceRepository.RolePositionDescriptionRepository()
        //        .ListForPositionDescriptions().FirstOrDefault(rp => rp.RolePositionDescId == id);

        //    if(rolePosDesc == null)
        //    {
        //        var msg = MessageHelper.NotFoundMessage("position description");
        //        throw new HttpException(msg);
        //    }

        //    //Generating PDF
        //    var srv = ServiceRepository.PdfService();

        //    var outputPath = PositionEstablishmentSettings.Site.PdfOutputPath;
        //    var inputFilePath = PositionEstablishmentSettings.Site.PdfTemplatePath;

        //    if(!Directory.Exists(outputPath) || !Directory.Exists(inputFilePath))
        //        {
        //        throw new DirectoryNotFoundException("The folders for PDF templates or output didn't exist. Please check the config file then create the folders.");
        //        }

        //    var templateFile = inputFilePath + "PD_only_Template.html";
        //    var cssFile = inputFilePath + "PDFGenerator.css";

        //    string htmlText = srv.ReadFileToText(templateFile);
        //    string cssText = srv.ReadFileToText(cssFile);

        //    StringBuilder selectionCriteria = new StringBuilder();

        //    var focusCriteria = rolePosDesc.PositionDescription.PositionFocusCriterias;

        //    selectionCriteria.Append("<ul>");

        //    foreach(var fc in focusCriteria)
        //        {
        //        selectionCriteria.Append("<li>" + fc.LookupFocusGradeCriteria.SelectionCriteria + "</li>");
        //        }

        //    selectionCriteria.Append("</ul>");

        //    var logo = srv.GetImagePath("dec-pdf.png");
        //    //filling the value in htmlText
        //    htmlText = htmlText.Replace("[Logo]", logo);

        //    htmlText = htmlText.Replace("[Title]", rolePosDesc.Title);
        //    htmlText = htmlText.Replace("[GradeCode]", rolePosDesc.GradeCode);
        //    htmlText = htmlText.Replace("[BriefRoleStatement]", rolePosDesc.PositionDescription.BriefRoleStatement);
        //    htmlText = htmlText.Replace("[StatementOfDuties]", rolePosDesc.PositionDescription.StatementOfDuties);
        //    htmlText = htmlText.Replace("[Criteria]", selectionCriteria.ToString());
        //    htmlText = htmlText.Replace("[DOCNumber]", rolePosDesc.DocNumber);
        //    htmlText = htmlText.Replace("[lastUpdated]", rolePosDesc.LastModifiedDate.ToShortDateString());

        //    var outputFileName = srv.GetPdfFileName(rolePosDesc.DocNumber, rolePosDesc.Title, rolePosDesc.GradeCode);
        //    string outputFile = outputPath + outputFileName;

        //    srv.GeneratePdfFromHTML(htmlText, cssText, outputFile);

        //    return File(outputFile, MediaTypeNames.Application.Pdf, outputFileName);

        //    }

        [HttpGet]
        [HasAnyAdminRole]
        public ActionResult WfHistory(int id)
        {
           
            ViewBagWrapper.PositionDescBag.SetPositionDescWizardBag(Enums.PositionDescWizardStep.History, ViewData);
           
            SetNumberOfLinkedPositions(id);
            if (id == 0)
            {
                var model = Repository.GetEmptyModel();
                SetWorkflowEngine(model);
                return View("Manage", model);
            }
            else
            {
                var model = Repository.LoadPositionDescById(id);

                if(model == null)
                    {
                    model = Repository.GetEmptyModel();
                    return View("Manage", model);
                    }

                var historyList =
               ServiceRepository.RolePositionDescriptionHistoryRepository().List().Where(r => r.RolePositionDescId == id);

                ViewBagWrapper.SetGeneralObject("historyList", historyList, ViewData);
                var wf = SetWorkflowEngine(model);
                return View("Manage", model);
            }
        }

        /// <summary>
        /// Clone position description
        /// only use the new DOC#, new 'statement of duties', new 'brief role statement'
        ///use the old 'title', old 'grade'
        ///Need to fill in 'RolePositionDescription', 'PositionDescription -> only PositionDescId'
        /// Copy the selectionCriteria from the source
        /// RolePositionDescriptionHistory
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApplyCloneAction(FormCollection collection)
        {
            var newDocNum = CommonHelper.ConstructDocNumber(Request);
            var sourceRdPdDescId = Convert.ToInt32(collection["SourceRolePositionDescId"]);
            var userName = CurrentUser.UserName;

            var sourcePositionDesc = Repository.GetPositionDescriptionById(sourceRdPdDescId);
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            var ajaxResult = new Result();

            //check the new Doc# not already exists.
            var newDocNumExists = Repository.RolePositionDescriptionRepository.Exists(newDocNum);
            
            if ( !string.IsNullOrEmpty(newDocNum) && !newDocNumExists && sourcePositionDesc!= null)
            {
                //create a new RolePositionDescription, a new row of position description with (new positionDescId)
                var newRolePosDesc = new RolePositionDescription
                {
                    DocNumber = newDocNum,
                    Title = sourcePositionDesc.RolePositionDescription.Title,
                    GradeCode = sourcePositionDesc.RolePositionDescription.GradeCode
                };
                var newPd = new PositionDescription();

                Repository.ClonePositionDescrition(newPd,
                    newRolePosDesc, sourcePositionDesc.RolePositionDescription.DocNumber, 
                    sourcePositionDesc.PositionDescriptionId, userName);
                
                if (Request.IsAjaxRequest())
                {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";
                    ajaxResult.Data = HttpHelper.GetActionUrl("Edit", "PositionDescription", 
                        new { id = newRolePosDesc.RolePositionDescId });

                    return Json(ajaxResult);
                }

                return RedirectToAction("Edit", "PositionDescription", 
                    new { id = newRolePosDesc.RolePositionDescId });

            }

            if (newDocNumExists)
            {
                errors.AddRange(Repository.GetBackendValidationErrors());
                var msg = MessageHelper.DocNumExistsMessage();
                errors.Add(new DbValidationError("Error", msg));
              
                if (Request.IsAjaxRequest())
                {
                    ajaxResult.Status = Status.Error;
                    ajaxResult.Message = "Error";
                    ajaxResult.AddErrors(errors);
                    return Json(ajaxResult);
                }
                else
                {
                    ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                }
            }
            else
            {
                errors.AddRange(Repository.GetBackendValidationErrors());
                var msg2 = MessageHelper.ErrorOccured();
                errors.Add(new DbValidationError("Error", msg2 + "The clone wasn't successful"));
                //errors.Add(new DbValidationError("Error", "Sorry something has gone wrong. The clone wasn't successful"));

                if (Request.IsAjaxRequest())
                {
                    ajaxResult.Status = Status.Error;
                    ajaxResult.Message = "Error";
                    ajaxResult.AddErrors(errors);
                    return Json(ajaxResult);
                }
                else
                {
                    ViewBagWrapper.ErrorBag.SetErrors(errors, ViewData);
                }
            }
            return RedirectToAction("Index");
        }
        private List<PositionLight> GetPositionList(PositionDescription positionDescription)
            {
            var positionLights = new List<PositionLight>();
            
            var positions =
               Repository.ServiceRepository.PositionRepository()
                   .BaseList()
                   .Where(p => p.StatusId != -1 && p.RolePositionDescriptionId == positionDescription.PositionDescriptionId)
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