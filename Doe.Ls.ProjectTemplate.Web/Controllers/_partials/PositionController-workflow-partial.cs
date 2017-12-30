using System;

using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Http;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
    {
    public partial class PositionController
    {
        [HasAnyAdminRole]
        public ActionResult ManageActions(int id)
            {
            ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.Actions,
                ViewData);
            var position = Repository.GetPositionById(id);
          
            if(position == null)
                {
                var msg = MessageHelper.NotFoundMessage($"position ({id})");
                throw new HttpException(msg);
                }
            if(position.IsDeleted())
                {
                return RedirectToAction("ManageSummary", new { id });

                }

            if (position.RolePositionDescription.IsPositionDescription)
            {
                position.RolePositionDescription.PositionDescription =
                    Repository.PositionDescriptionRepository.ListWithSelectionCriteria()
                        .SingleOrDefault(r => r.PositionDescriptionId == position.RolePositionDescriptionId);
            }
            else
            {
                position.RolePositionDescription.RoleDescription =
                    Repository.RoleDescriptionRepository.GetRoleDescriptionById	(position.RolePositionDescriptionId);
                }

           
            var wf=SetWorkflowEngine(position);
           
            return View("Manage", position);         
            }

        [HasAnyAdminRole]
        public ActionResult PositionApprovalList()
            {
            ViewBagWrapper.SetGeneralObject("PositionListType",PositionListType.ApprovalList, ViewData);
            var arg = new JQueryDatatableParamPositionExtension();
            
            ReadFromQueryString(arg);
            LoadCommonLookups(arg.DivisionCode, arg.DirectorateId, arg.BusinessUnitId, arg.UnitId);
            LoadLookupsForList(arg.PosStatusCode, arg.StatusCode);
            var task = GetTask();
            var test= task.GetApprovalPositions().Take(1).ToList();
            if (test.Count < 0)
            {
                // do nothing it is just to invoke Access denied exception 
            }
            ViewBagWrapper.VariableBag.SetBoolVariable("hasSession", arg.HasSession, ViewData);
            return View("Index");
            }
        public ActionResult ListApprovalJson([FromUri] JQueryDatatableParamPositionExtension arg)
            {
            ViewBagWrapper.SetGeneralObject("PositionListType", PositionListType.ApprovalList, ViewData);
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
            var task = GetTask();

            try
                {
                var positions = task.GetApprovalPositions();

                    var displayedPositions = Repository.FilterDisplayedPositions(arg, positions);

                var totalRecord = displayedPositions.Count();
                var totalDisplayRecord = displayedPositions.Count();

                if(arg.iDisplayLength == -1)
                    arg.iDisplayLength = totalRecord;

                displayedPositions = displayedPositions.Skip(arg.iDisplayStart).Take(arg.iDisplayLength);
                var currTask = UserTaskFactory.GetTask(CurrentUser, ServiceRepository.RepositoryFactory);
                var canView = currTask.CanViewRolePositiondesc();
                var result = displayedPositions.AsEnumerable().ToArray().Select(ent => ent.To(new PositionLight()
                    {
                    UnitName = ent.Unit.UnitName,
                    ParentPositionTitle = $"({ent.ParentPosition.PositionNumber})-{ent.ParentPosition.PositionTitle }",
                    PositionLevel = ent.PositionLevel.PositionLevelName,
                    StatusName = ent.StatusValue.StatusName,
                    DOCNumber = ent.RolePositionDescription.DocNumber,
                    CanViewRolePosDesc = canView,
                    Grade = ent.RolePositionDescription.GradeCode,
                    PositionType = ent.PositionInformation != null ? ent.PositionInformation.PositionTypeCode : "",
                    OccupationType = ent.PositionInformation != null ? ent.PositionInformation.OccupationTypeCode : "",
                    EmployeeType = ent.PositionInformation != null ? ent.PositionInformation.EmployeeTypeCode : "",
                    Privilege = GetPositionPrivilege(ent)

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
                dataTableResult.AddError(new DbValidationError("Error",
                    msg + exception.Message));
                //dataTableResult.AddError(new DbValidationError("Error",
                //    "Oops! something went wrong. " + exception.Message));
                }

            return Json(dataTableResult, JsonRequestBehavior.AllowGet);
            }

        public ActionResult GetWorkflowAction(int wfObjectId,WorkflowObjectType objectType,int actionId)
        {
            var task = UserTaskFactory.GetTask(CurrentUser, this.Repository.RepositoryFactory);
            var position = new Position();
            if (objectType == WorkflowObjectType.Position)
            {
                position = Repository.GetPositionById(wfObjectId);
                SetWorkflowEngine(position, task);
               }
            else
            {
                throw new InvalidOperationException("Position workflow should have object type = position");
            }
            
            var action = new WorkflowAction {ActionId = actionId};
            
            if (action.ActionId == (int)Enums.WorkflowActions.Clone)
            {
                var clone = new CloneActionModel();
                if (position != null)
                {
                    clone.SourcePosition = position;
                    clone.SourcePositionId = position.PositionId;
                    clone.NewPositionNumber = Request["PositionNumber"];
                    return View("ClonePosition", clone);
                }
                else
                {
                    var msg = MessageHelper.NotFoundMessage("source position");
                    throw new InvalidOperationException(msg);
                }
            }

            WorkflowAction.Populate(this.Repository.RepositoryFactory, action);


            if(action.ActionId == (int)Enums.WorkflowActions.UpdatePositionNumber)
                {
                
                    return View("WorkflowAction-UpdatePosNumber-Modal", action);
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
                if (model.ObjectType == WorkflowObjectType.Position)
                {
                    var position = Repository.GetPositionById(model.WfObjectId);
                    var workflowEngine = WorkflowEngineFactory.CreatEngine(position, task);
                    ajaxResult=workflowEngine.ApplyAction(model, true,Request.Form);
                }
                else
                {
                        throw new InvalidOperationException("Object type must be Position");

                    }


                }
            catch (Exception exception)
            {
                LogException(exception);

                var errors = Repository.GetBackendValidationErrors().ToList();

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
              
                return Json(ajaxResult);
            }

            return RedirectToAction("Index");
            }


        private Enums.Privilege GetPositionPrivilege(Position ent)
        {
            var wf = SetWorkflowEngine(ent);
            return wf.GetWorkflowObjectPrivilege();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ApplyCloneAction(FormCollection collection)
        {
            var selectionOption = collection["select"];
            var newPositionNum = collection["NewPositionNumber"];
            var sourcePositionId = Convert.ToInt32(collection["SourcePositionId"]);
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            var sourcePos = Repository.ListForClonePosition()
                .Where(
                    p =>
                        p.StatusId == (int) Enums.StatusValue.Approved || p.StatusId == (int) Enums.StatusValue.Imported)
                .SingleOrDefault(p => p.PositionId == sourcePositionId);

            int newPositionId = 0;

            if (selectionOption == "0")
            {
                //clone from selected position =>
                //includes position (with new position), positionInfo, cost centre, RD/PD
                if (sourcePos != null)
                {
                    newPositionId = Repository.ProcessClonePosition(sourcePos, newPositionNum,
                        sourcePos.RolePositionDescriptionId, CurrentUser.UserName);

                }
            }
            else if (selectionOption == "1")
            {
                //use an exisiting DOC# =>
                //clone position, positioninfo, cost centre, but link with the selected existing RD/PD
            }
            else
            {
                //TODO
                //use new DOC# =>
                //Clone position, position info, cost centre, but with an empty RD/PD
            }

            var ajaxResult = new Result();

            if (newPositionId > 0)
            {
                if (Request.IsAjaxRequest())
                {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";
                    ajaxResult.Data = HttpHelper.GetActionUrl("Edit", "Position", new {id = newPositionId});

                    return Json(ajaxResult);
                }
                
                return RedirectToAction("Edit", "Position", new { id = newPositionId });
                
            }
            else
            {
                errors.AddRange(Repository.GetBackendValidationErrors());
                var msg = MessageHelper.ErrorOccured();
                errors.Add(new DbValidationError("Error", msg + "The clone wasn't successful"));
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
    }
    }


