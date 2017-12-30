using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Http;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Exceptions;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Web.Controllers._partials;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{

    [System.Web.Mvc.Authorize(Roles = Enums.UserRoleValues.DoEUser)]
    public partial class PositionController : WorkflowController
        {
        private PositionRepository _repository = null;
        public PositionRepository Repository
        {
            get
            {
                return _repository = _repository ?? ServiceRepository.PositionRepository();
            }
        }

        public ActionResult Index()
        {
			ViewBagWrapper.SetGeneralObject("PositionListType", PositionListType.AdvancedSearch, ViewData);
            var arg = new JQueryDatatableParamPositionExtension();

            //check query string from bookmark

            ReadFromQueryString(arg);
            LoadCommonLookups(arg.DivisionCode, arg.DirectorateId, arg.BusinessUnitId, arg.UnitId);
            LoadLookupsForList(arg.PosStatusCode,arg.StatusCode);
            
            ViewBagWrapper.VariableBag.SetBoolVariable("hasSession", arg.HasSession, ViewData);

            return View();
        }

      // GET: /Position/Details/5
        public ActionResult Details(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Details, ViewData);
            var position = Repository.GetPositionById(id);
            if (position == null || position.StatusId==(int)Enums.StatusValue.Deleted)
            {
                var msg = MessageHelper.NotFoundMessage($"position ({id})");
                throw new HttpException(msg);
            }
            SetWorkflowEngine(position);
            if (Request.IsAjaxRequest())
            {
                return View("Details-modal", position);
            }
            else
            {
                return View(position);
            }
        }

        public ActionResult ListJson([FromUri] JQueryDatatableParamPositionExtension arg)
        {
			ViewBagWrapper.SetGeneralObject("PositionListType", PositionListType.AdvancedSearch, ViewData);
            InitialiseArgument(arg);
            var dataTableResult = new DataTableResult();
            var task = GetTask();

            try
            {
                var positions = task.FilterPositions(ServiceRepository.PositionRepository().List()).AsNoTracking();

                var displayedPositions = Repository.FilterDisplayedPositions(arg, positions);

                var totalRecord = displayedPositions.Count();
                var totalDisplayRecord = displayedPositions.Count();

                if (arg.iDisplayLength == -1)
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
                    PositionType= ent.PositionInformation!=null?ent.PositionInformation.PositionTypeCode:"",
                    OccupationType= ent.PositionInformation != null ? ent.PositionInformation.OccupationTypeCode : "",
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

       

        // GET: /Position/Details/5
        public ActionResult DetailsJson(int id = 0)
        {
            var ajaxResult = new Result();

            try
            {
                var positionLight = Repository.GetEntityByKey(id).To(new PositionLight());


                ajaxResult.Status = Status.Success;
                ajaxResult.Message = "Success";
                ajaxResult.Data = positionLight;

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



        //
        // GET: /Position/Create
        public ActionResult Create(int rolePositionDescId)
        {
            ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.BasicDetails, ViewData);
            var rolePositionDesc =
                Repository.RolePositionDescriptionRepository.GetPositionDescriptionById(rolePositionDescId);

            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            var position = new Position()
            {
                PositionNumber = Enums.Cnt.DefaultPositionNo,
                RolePositionDescription = rolePositionDesc,
                RolePositionDescriptionId = rolePositionDescId,
                PositionTitle = rolePositionDesc.Title,
                PositionLevelId = (int)Enums.PositionLevel.Position
            };
            EditLookups(position);
            SetWorkflowEngine(position);
            GetErrorsFromTempData();
            return View("Manage" , position);
           
        }

        //
        // POST: /Position/Create

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Position position)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);

            var ajaxResult = new Result();
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Repository.Exists(position.PositionNumber))
                    {
                        var msg = MessageHelper.PositionNumberExists(position.PositionNumber);
                        throw new InvalidOperationException(msg);
                        //throw new InvalidOperationException($"position number {position.PositionNumber} already exists");
                    }

                    Repository.Insert(position);
                    
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
                return RedirectToAction("EditMoreInfo");

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
                    return View(position);
                }

            }

        }


        //
        // GET: /Position/Edit/5
        public ActionResult Edit(int id = 0)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            var position = Repository.GetPositionById(id);
            if (position == null)
            {
                var msg = MessageHelper.NotFoundMessage($"position ({id})");
                throw new HttpException(msg);
            }

            if (position.PositionLevelId == Enums.Cnt.Na)
            {
                position.PositionLevelId = (int) Enums.PositionLevel.Position;
            }
            
            if (position.IsDeleted())
            {
                return RedirectToAction("ManageSummary", new {id});

                }
            EditLookups(position);
            ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.BasicDetails, ViewData);
            
            if (!Repository.GetRootPositionsIds().Contains(id) && position.ReportToPositionId == Enums.Cnt.Na) //not RootEdsServices position
            {
                position.ReportToPositionId = 0;
            }
            var workflowEngine=SetWorkflowEngine(position);
            GetErrorsFromTempData();
            if (workflowEngine.GetWorkflowObjectPrivilege().CanEdit)
            {
                return View("Manage", position);
            }
            else
            {
                return RedirectToAction("ManageSummary", new {id});
            }
        }

       

        //
        // POST: /Basic Details form/
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit(Position position)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            ModelState.Remove("CreatedDate");
            ModelState.Remove("LastModifiedDate");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("LastModifiedBy");
            ModelState.Remove("PositionPath");
            ModelState.Remove("StatusId");
            ModelState.Remove("PositionTitle");
           
            var errors = Repository.GetValidationErrors(ModelState).ToList();            
            if (ModelState.IsValid)
            {
                try
                {
                    var oldPosition = Repository.GetPositionById(position.PositionId);
                    var mockPos = position.Clone();
                    if (oldPosition == null || oldPosition.UnitId == Enums.Cnt.Na) // new position
                    {

                        if(Repository.Exists(position.PositionNumber))
                        {
                            var msg = MessageHelper.PositionNumberExists(position.PositionNumber);
                            throw new InvalidOperationException(msg);
                            //throw new InvalidOperationException($"position number {position.PositionNumber} already exists");
                        }

                        mockPos.Unit = ServiceRepository.UnitRepository().GetUnitById(position.UnitId);
                        mockPos.StatusId = (int) Enums.StatusValue.Draft;
                        mockPos.StatusValue = new StatusValue
                        {
                            StatusId = (int) Enums.StatusValue.Draft,
                            StatusName = Enums.StatusValue.Draft.ToString()
                        };
                           
                        var wf = WorkflowEngineFactory.CreatEngine(mockPos, CurrentUser,
                            this.Repository.RepositoryFactory);
                        if (!wf.GetWorkflowObjectPrivilege().CanEdit)
                        {
                            if (mockPos.Unit != null)
                            {
                                var msg = MessageHelper.NoModifyPositionPermission(mockPos.Unit.UnitName);
                                throw new AccessDeniedException(msg);
                                //throw new AccessDeniedException($"This user has no modify access over {mockPos.Unit} team positions");
                            }
                            else
                            {
                                var msg = MessageHelper.NotFoundMessage("team");
                                throw new Exception(msg);
                            }
                        }
                    }
                    else
                    {
                        mockPos.Unit = ServiceRepository.UnitRepository().GetUnitById(position.UnitId);
                        mockPos.StatusValue = oldPosition.StatusValue;

                        var wf = WorkflowEngineFactory.CreatEngine(oldPosition, CurrentUser,
                            this.Repository.RepositoryFactory);
                        var wf2 = WorkflowEngineFactory.CreatEngine(mockPos, CurrentUser,
                          this.Repository.RepositoryFactory);
                        if(!wf.GetWorkflowObjectPrivilege().CanEdit)
                        {
                            if (mockPos.Unit != null)
                            {
                                var msg = MessageHelper.NoModifyPositionPermission(oldPosition.Unit.UnitName);
                                throw new AccessDeniedException(msg);
                                //throw new AccessDeniedException($"This user has no modify access over {oldPosition.Unit} team positions");
                            }
                            else
                            {
                                var msg = MessageHelper.NotFoundMessage("team");
                                throw new Exception(msg);
                            }
                           
                        }

                        if (!wf2.GetWorkflowObjectPrivilege().CanEdit)
                        {
                            if (mockPos.Unit != null)
                            {
                                var msg = MessageHelper.NoModifyPositionPermission(mockPos.Unit.UnitName);
                                throw new AccessDeniedException(msg);
                                //throw new AccessDeniedException($"This user has no modify access over {mockPos.Unit} team positions");
                            }
                            else
                            {
                                var msg = MessageHelper.NotFoundMessage("team");
                                throw new Exception(msg);
                            }
                            
                        }
                        }

                    //construct for history logging

                    if(string.IsNullOrWhiteSpace(position.DivisionOverview))
                        {
                        
                        if(position.UnitId != Enums.Cnt.Na)
                            {
                            try
                                {
                                var div = ServiceRepository.UnitRepository().GetUnitById(position.UnitId).BusinessUnit.Directorate.Executive;
                                position.DivisionOverview = div.DefaultExecutiveOverview;
                                }
                            catch(Exception exception)
                                {
                                LoggerService.Log(exception);
                                }
                            }


                        }

                    Repository.CreateOrUpdatePositionWithHistory(position, CurrentUser.UserName);
                    }
                catch (Exception exception)
                {
                    LogException(exception);
                    errors.AddRange(Repository.GetBackendValidationErrors());
                    errors.Add(new DbValidationError("There", exception.Message));
                    CreateLookups();
                    SetErrorsInTempData(errors);
                    if (position.PositionId == 0)
                    {
                        return RedirectToAction("Create", new { rolePositionDescId = position.RolePositionDescriptionId });
                    }
                    return RedirectToAction("Edit", new {id= position.PositionId});
                }
                CreateLookups();
                return RedirectToAction("EditMoreInfo",new {id=position.PositionId});
            }
            else
            {
                ViewBagWrapper.ErrorBag.SetErrors(Repository.GetValidationErrors(ModelState), ViewData);
                CreateLookups();
                SetErrorsInTempData(errors);
                return RedirectToAction("Edit", new { id = position.PositionId });
            }
        }

        public ActionResult LoadPositions(string term)
        {
            var list = Repository.List().Where(l => l.PositionTitle.Contains(term) || l.PositionNumber.Contains(term)).AsEnumerable().ToArray()

                 .Select(pe => new SelectListItemExtension { Value = pe.PositionId.ToString(), Text = pe.PositionNumber + "|" + pe.PositionTitle })
                      .ToArray();


            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateRolePositionDesc()
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Create, ViewData);
            
            return View("CreateRolePositionDescription-modal");

        }
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("CreateRolePositionDescConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRolePositionDescConfirmed()
        {
            var ajaxResult = new Result();
            var neworold = Request["select"];
            var errors = Repository.GetValidationErrors(ModelState).ToList();
            try
            {
                int posId;
                if (neworold == "1")
                {
                    var docNumber = Request["RolePositionDescId"];
                    int id;
                    int.TryParse(docNumber, out id);
                    var rolePosition = ServiceRepository.RolePositionDescriptionRepository()
                        .List()
                        .FirstOrDefault(l => l.RolePositionDescId == id);
                    if (rolePosition == null)
                    {
                        throw new Exception("Selected document doesn't exist");
                    }
                   
                    var position = Repository.CreateNewPosition(rolePosition.RolePositionDescId, rolePosition.Title);
                    posId = position.PositionId;
                    //return RedirectToAction("edit", new { id = position.PositionId });

                    //add to history
                    var msg = "Position was created from an existing DOC# (" + rolePosition.DocNumber + ")";
                    ServiceRepository.PositionHistoryRepository()
                        .LogHistoryOnCreatePositioin(position, CurrentUser.UserName, msg);
                }
                else
                {
                    var docNumber = CommonHelper.ConstructDocNumber(Request);
                    var gradeCode = Request["GradeCode"];
                    var title = Request["Title"];

                    var rolePos = new RolePositionDescription()
                    {
                        Title = title,
                        DocNumber = docNumber,
                        GradeCode = gradeCode,
                        IsPositionDescription = Request["DescType"] == Enums.DescriptionType.Position.ToString()
                    };

                    if(Repository.RolePositionDescriptionRepository.Exists(rolePos.DocNumber))
                        {
                        throw new InvalidOperationException($"Document number {rolePos.DocNumber} already exists");
                        }
                    var position = Repository.CreatePositionAndRolePositionDescription(rolePos);
                    posId = position.PositionId;
                    // return RedirectToAction("edit", new { id = position.PositionId });

                    //Add to history
                    var msg = "Position was created from a new DOC# = " + docNumber + ", title = " + rolePos.Title +
                              ", grade = " + rolePos.GradeCode;
                    ServiceRepository.PositionHistoryRepository()
                        .LogHistoryOnCreatePositioin(position, CurrentUser.UserName, msg);
                    ServiceRepository.RolePositionDescriptionHistoryRepository()
                        .LogHistoryOnCreateRolePositioinDesc(position.RolePositionDescription, CurrentUser.UserName);

                }
                if(Request.IsAjaxRequest())
                    {
                    ajaxResult.Status = Status.Success;
                    ajaxResult.Message = "Success";

                        ajaxResult.Data = HttpHelper.GetActionUrl("Edit", "Position", new {id = posId});

                    return Json(ajaxResult);
                    }
                }
            catch (Exception exception)
            {
                LogException(exception);
                errors.AddRange(Repository.GetBackendValidationErrors());
                errors.Add(new DbValidationError("", "Something went wrong. " + exception.Message));

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
            
            return RedirectToAction("Index");

        }
        public ActionResult EditMoreInfo(int id)
        {
            var position = Repository.GetPositionById(id);

            if (position == null)
            {
                var msg = MessageHelper.NotFoundMessage($"position ({id})");
                throw new InvalidOperationException(msg);
            }
            if(position.IsDeleted())
                {
                return RedirectToAction("ManageSummary", new { id });

                }
            ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.MoreInfo, ViewData);
            
            if (position.PositionInformation == null)
            {
                position.PositionInformation = new PositionInformation()
                {
                    PositionId = position.PositionId
                };
            }

            
            LookUpMoreInfo();
            var wf=SetWorkflowEngine(position);
            if (!wf.GetWorkflowObjectPrivilege().CanEdit)
            {
                return RedirectToAction("ManageSummary", new { id });
                }
            GetErrorsFromTempData();
            return View("Manage", position);
        }

     

        private void LookUpMoreInfo()
        {
            var positionTypeItems = ServiceRepository.PositionTypeRepository()
                .List().ToArray()
                .Select(pe => new SelectListItemExtension {Value = pe.PositionTypeCode.ToString(), Text = pe.PositionTypeName})
                .ToArray();

            ViewBagWrapper.ListBag.SetList("positionTypeItems", positionTypeItems, ViewData);


            var employeeTypeItems = ServiceRepository.EmployeeTypeRepository()
                .List().ToArray()
                .Select(pe => new SelectListItemExtension {Value = pe.EmployeeTypeCode.ToString(), Text = pe.EmployeeTypeName})
                .ToArray();

            ViewBagWrapper.ListBag.SetList("employeeTypeItems", employeeTypeItems, ViewData);
            var positionStatusItems = ServiceRepository.PositionStatusValueRepository()
                .List().ToArray()
                .Select(pe => new SelectListItemExtension { Value = pe.PosStatusCode.ToString(), Text = pe.PosStatusTitle })
                .ToArray();

            ViewBagWrapper.ListBag.SetList("positionStatusItems", positionStatusItems, ViewData);

            var occupationTypeItems = ServiceRepository.OccupationTypeRepository()
                  .List().Where(i=>i.OccupationTypeCode != "-1").ToArray()
                  .Select(pe => new SelectListItemExtension { Value = pe.OccupationTypeCode.ToString(), Text = pe.OccupationTypeName })
                  .ToArray();

            ViewBagWrapper.ListBag.SetList("occupationTypeItems", occupationTypeItems, ViewData);

        }

        [System.Web.Mvc.AllowAnonymous]
        public ActionResult GetPositionsAjax(string term, string context = null)
            {
            var positionId = 0;
            if(!string.IsNullOrWhiteSpace(context))
                {
                int.TryParse(context, out positionId);
                }

            var positions = ServiceRepository.PositionRepository()
                        .List().Where(p => p.PositionTitle.Contains(term) || p.PositionNumber.Contains(term));
            if(positionId > 0) positions = positions.Where(p => p.PositionId != positionId);
            var positionItems = positions
                         .ToArray()
                        .Select(pe => new SelectListItemExtension { Value = pe.PositionId.ToString(), Text = pe.PositionNumber + " | " + pe.PositionTitle })
                        .ToArray();
            return Json(positionItems, JsonRequestBehavior.AllowGet);
            }


        private void ReadFromQueryString(JQueryDatatableParamPositionExtension arg)
        {
            var divisionCode = Request.QueryString["DivisionCode"];
            var directorateId = (!String.IsNullOrEmpty(Request.QueryString["DirectorateId"])) ? int.Parse(Request.QueryString["DirectorateId"]) : 0;
            var businessUnitId = (!String.IsNullOrEmpty(Request.QueryString["BusinessUnitId"])) ? int.Parse(Request.QueryString["BusinessUnitId"]) : 0;
            var unitId = (!String.IsNullOrEmpty(Request.QueryString["UnitId"])) ? int.Parse(Request.QueryString["UnitId"]) : 0;
            var posStatusCode = Request.QueryString["PosStatusCode"];
            var statusCode = Request.QueryString["StatusCode"];


            if (!string.IsNullOrEmpty(divisionCode) || directorateId > 0 || businessUnitId > 0 || unitId > 0 || !string.IsNullOrEmpty(statusCode) || !string.IsNullOrWhiteSpace(posStatusCode))
            {
                arg.DirectorateId = directorateId;
                arg.BusinessUnitId = businessUnitId;
                arg.DivisionCode = divisionCode;
                arg.UnitId = unitId;
                if (!string.IsNullOrWhiteSpace(posStatusCode))
                {

                    arg.PosStatusCode = posStatusCode.Split(',');
                }

                if (!string.IsNullOrEmpty(statusCode))
                {
                    arg.StatusCode = statusCode.Split(',');
                }
                    //arg.StatusCode = !string.IsNullOrEmpty(statusCode) ? Convert.ToInt32(statusCode) : 0;

                // SetFilterSession(arg);
            }
        }
        private void CreateLookups()
        {

            var enity = T4Helper.GetEntityType("Position", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);
            
            var positionLevelItems = ServiceRepository.PositionLevelRepository()
                      .List().ToArray()
                      .Select(pe => new SelectListItemExtension { Value = pe.PositionLevelId.ToString(), Text = pe.PositionLevelName })
                      .ToArray();

            ViewBagWrapper.ListBag.SetList("positionLevelItems", positionLevelItems, ViewData);
            /*
                        var directorateItems = ServiceRepository.DirectorateRepository()
                                  .List().ToArray()
                                  .Select(pe => new SelectListItemExtension { Value = pe.DirectorateId.ToString(), Text = pe.DirectorateName })
                                  .ToArray();

                        ViewBagWrapper.ListBag.SetList("directorateItems", directorateItems, ViewData);*/

            var divisionItems = ServiceRepository.ExecutiveRepository()
                      .List().ToArray()
                      .Select(pe => new SelectListItemExtension { Value = pe.ExecutiveCod.ToString(), Text = pe.ExecutiveTitle })
                      .ToArray();

            ViewBagWrapper.ListBag.SetList("divisionItems", divisionItems, ViewData);

        }
        private void EditLookups(Position position)
        {
            var enity = T4Helper.GetEntityType("Position", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);

            var positionLevelItems = ServiceRepository.PositionLevelRepository()
                      .List().ToArray()
                      .Select(pe => new SelectListItemExtension { Value = pe.PositionLevelId.ToString(), Text = pe.PositionLevelName })
                      .ToArray();

            ViewBagWrapper.ListBag.SetList("positionLevelItems", positionLevelItems, ViewData);
            int dirId = (position != null && position.Unit != null ? position.Unit.BusinessUnit.DirectorateId : 0);

            var divisionItems = ServiceRepository.ExecutiveRepository()
                     .List().FilterDefaults(CurrentUser).ToArray()
                     .Select(pe => new SelectListItemExtension
                     {
                         Value = pe.ExecutiveCod.ToString(),
                         Text = pe.ExecutiveTitle,
                         Selected = pe.Directorates.Any(dir => dir.DirectorateId == dirId)

                     }).ToArray();
            ViewBagWrapper.ListBag.SetList("divisionItems", divisionItems, ViewData);
            var unitItems = Enumerable.Empty<SelectListItem>();
            var locationItems = Enumerable.Empty<SelectListItem>();
            var businessUnitItems = Enumerable.Empty<SelectListItem>();
            var directorateItems = Enumerable.Empty<SelectListItem>();
            //  var unitPositions = Enumerable.Empty<SelectListItem>();

            if (position!=null && position.UnitId > 0)
            {
                 directorateItems = ServiceRepository.DirectorateRepository()
                   .List().FilterDefaults(CurrentUser)
                   .Select(pe => new SelectListItemExtension
                   {
                       Value = pe.DirectorateId.ToString(),
                       Text = pe.DirectorateName,
                       Selected = pe.BusinessUnits.Any(bu => bu.BUnitId == position.Unit.BUnitId)
                   })
                   .ToArray();

           
                locationItems = ServiceRepository.LocationRepository().List().Where(l => l.DirectorateId == position.Unit.BusinessUnit.DirectorateId)
                 .ToArray().Select(l => new SelectListItemExtension
                 {
                     Value = l.LocationId.ToString(),
                     Text = l.Name
                 }).ToArray();

                businessUnitItems = ServiceRepository.BusinessUnitRepository()
                     .List().Where(bu => bu.DirectorateId == position.Unit.BusinessUnit.DirectorateId).FilterDefaults(CurrentUser)
                     .Select(pe => new SelectListItemExtension
                     {
                         Value = pe.BUnitId.ToString(),
                         Text = pe.BUnitName,
                         Selected = pe.Units.Any(u => u.UnitId == position.UnitId)
                     })
                     .ToArray();
               

                unitItems = ServiceRepository.UnitRepository()
                                   .List().Where(u => u.BUnitId == position.Unit.BUnitId)
                                   .Select(pe => new SelectListItemExtension
                                   {
                                       Value = pe.UnitId.ToString(),
                                       Text = pe.UnitName,

                                   });
               // unitPositions = Repository.GetTeamMembersOrderByMaxRate(position.UnitId);


            }
            ViewBagWrapper.ListBag.SetList("directorateItems", directorateItems, ViewData);
            ViewBagWrapper.ListBag.SetList("businessUnitItems", businessUnitItems, ViewData);
            ViewBagWrapper.ListBag.SetList("locationItems", locationItems, ViewData);
            ViewBagWrapper.ListBag.SetList("unitItems", unitItems, ViewData);
        }

        //public ActionResult PositionNumberExists(string positionNumber)
        //{
        //    var result = new { valid = true };

        //    var positionNumExist = Repository.HasPositionNumber(positionNumber);

        //    result = new {valid = !positionNumExist};

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

            /// <summary>
            /// Changed by RW as the old version does not exclude the delete positions
            /// </summary>
            /// <param name="positionNumber"></param>
            /// <returns></returns>
        public ActionResult PositionNumberExists(string positionNumber)
            {
            var result = new { valid = true };

            var positionNumExist = Repository.Exists(positionNumber);//only check in non deleted positions 

            result = new { valid = !positionNumExist };

            return Json(result, JsonRequestBehavior.AllowGet);
            }
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }


        
    }
}