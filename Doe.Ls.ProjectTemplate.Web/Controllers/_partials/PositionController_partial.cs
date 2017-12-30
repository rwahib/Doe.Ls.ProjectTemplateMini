using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.UI.CustomAttributes;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Core.Exceptions;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Web.Controllers
{
    public partial class PositionController
    {
        // GET: Position
        public PositionController(){}

        public PositionController(ServiceRepository srv)
        {
            _serviceRepository = srv;
            _repository = srv.PositionRepository();
        }

        //this methid is called on the callback of chartload
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //public ActionResult GetPositions(int NoOfLevels = 0,int DirectorateId = 0, int BusinessUnitId = 0, int UnitId = 0)
        public ActionResult GetPositions(PositionChartFilterParams positionFilterParams)
        {
            try
            {
                var positions=new List<Position>();
                if (!string.IsNullOrWhiteSpace(positionFilterParams.DivisionCode))
                {
                 positions = Repository.LoadPositionListForChart(positionFilterParams);
                }
                var positionsChartModel = Repository.LoadChartJson(positions);
                return Json(positionsChartModel.ToArray(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
              throw new Exception(e.Message);
            }            
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult InitiateChartLoading(string DivisionCode = null,int DirectorateId = 0, int BusinessUnitId = 0, int UnitId = 0, int NoOfLevels = 0)
        {
            //Flag not to include with footer
            ViewBagWrapper.LayoutBag.SetHasFooter(false,ViewData);
            // setting the no of config for first load as 2
            ViewBagWrapper.VariableBag.SetIntVariable("NoOfLevels", NoOfLevels, ViewData);
            var fromChart = true;
            LoadCommonLookups(DivisionCode, DirectorateId, BusinessUnitId, UnitId, fromChart);
            return View("PositionChart", new Position());
        }

        [Authorize]
        public ActionResult LoadChartForPrint(int id)
        {
           // var pos = Repository.CachedPositionListForChart().FirstOrDefault(f => f.PositionId == id);
           // var positionId = Repository.GetTopPositionToPrint(pos.UnitId);
          // ViewBag.UnitId = pos.UnitId;
            var positionsList = Repository.GetChartObjForGeneratePdf(id).ToList();
            ViewBagWrapper.LayoutBag.SetHasFooter(false, ViewData);
            var positions = Repository.LoadChartJson(positionsList, true, id);
            ViewBag.PositionList = positions;
            return View("PrintView");
        }

        private void LoadCommonLookups(string executiveCod, int directorateId = 0, int businessUnitId = 0,
            int unitId = 0,bool fromChart=false)
        {
            if (string.IsNullOrWhiteSpace(executiveCod) &&
                (CurrentUser.DefaultOrganisationalModel.Divisions != null &&
                 CurrentUser.DefaultOrganisationalModel.Divisions.Count == 1))
            {
                executiveCod = CurrentUser.DefaultOrganisationalModel.Divisions.FirstOrDefault();
            }

            if(directorateId==0 && CurrentUser.DefaultOrganisationalModel.Directorates != null && CurrentUser.DefaultOrganisationalModel.Directorates.Count==1)
                {
                directorateId = CurrentUser.DefaultOrganisationalModel.Directorates.FirstOrDefault();
                }

            if(businessUnitId == 0 &&
                (CurrentUser.DefaultOrganisationalModel.BusinessUnites != null &&
                 CurrentUser.DefaultOrganisationalModel.BusinessUnites.Count == 1))
                {
                businessUnitId = CurrentUser.DefaultOrganisationalModel.BusinessUnites.FirstOrDefault();
                }


            IEnumerable<Position> fullPositionList;
            IQueryable<Executive> divisionItems;
            if (fromChart)
            {
                fullPositionList = Repository.CachedPositionListForChart();
                divisionItems = ServiceRepository.ExecutiveRepository().List();
            }
            else
            {
                var task = GetTask();
                 fullPositionList = task.FilterPositions(ServiceRepository.PositionRepository().List());
                divisionItems = task.GetDivisionList();
            }
            var enity = T4Helper.GetEntityType("Position", this.ServiceRepository.GetUnitOfWork().DbContext);
            ViewBagWrapper.EntityInfo.SetEntityType(enity, ViewData);

           var filteredDivisions = divisionItems.ToArray()
                      .Select(pe => new SelectListItemExtension
                      {
                          Value = pe.ExecutiveCod.ToString(),
                          Text = pe.ExecutiveTitle + " ("
                    + fullPositionList.Count(a => a.Unit.BusinessUnit.Directorate.ExecutiveCod == pe.ExecutiveCod)
                    + ")",
                          Selected = pe.ExecutiveCod == executiveCod 
                      })
                      .ToArray();

            ViewBagWrapper.ListBag.SetList("divisionItems", filteredDivisions, ViewData);
            var businessUnitItems = Enumerable.Empty<SelectListItem>();
            var directorateItems = Enumerable.Empty<SelectListItem>();

            if (!string.IsNullOrEmpty(executiveCod))
            {
                 directorateItems = ServiceRepository.DirectorateRepository()
                       .List().Where(l => l.DirectorateId != -1 && l.ExecutiveCod == executiveCod).ToArray()
                       .Select(pe => new SelectListItemExtension
                       {
                           Value = pe.DirectorateId.ToString(),
                           Text = pe.DirectorateName + " ("
                     + fullPositionList.Count(a => a.Unit.BusinessUnit.DirectorateId == pe.DirectorateId)
                     + ")",
                           Selected = pe.DirectorateId == directorateId 
                       })
                       .ToArray();
            }
            ViewBagWrapper.ListBag.SetList("directorateItems", directorateItems, ViewData);

            ViewBagWrapper.ListBag.SetList("businessUnitItems", businessUnitItems, ViewData);
            if (directorateId > 0)
            {
                businessUnitItems = ServiceRepository.BusinessUnitRepository()
                 .List().Where(l => l.BUnitId != -1 && l.DirectorateId == directorateId).ToArray()
                 .Select(pe => new SelectListItemExtension
                 {
                     Value = pe.BUnitId.ToString(),
                     Text = pe.BUnitName + " ("
                + fullPositionList.Count(a => a.Unit.BUnitId == pe.BUnitId)
                + ")",
                     Selected = pe.BUnitId == businessUnitId 
                 })
                 .ToArray();

            }
            ViewBagWrapper.ListBag.SetList("businessUnitItems", businessUnitItems, ViewData);
            var unitItems = Enumerable.Empty<SelectListItem>();
            if (businessUnitId > 0)
            {
                unitItems = ServiceRepository.UnitRepository()
                .List().Where(u => u.UnitId != -1 && u.BUnitId == businessUnitId).ToArray()
                .Select(pe => new SelectListItemExtension
                {
                    Value = pe.UnitId.ToString(),
                    Text = pe.UnitName + " ("
                           + fullPositionList.Count(a => a.Unit.UnitId == pe.UnitId)
                           + ")",
                    Selected = pe.UnitId == unitId
                })
                .ToArray();
            }
            ViewBagWrapper.ListBag.SetList("unitItems", unitItems, ViewData);
            

        }

        private void LoadLookupsForList(string[] posStatus = null, string[] status=null)
        {
            var posStatusCodeItems = ServiceRepository.PositionStatusValueRepository()
                     .List().ToArray()
                     .Select(pe => new SelectListItemExtension
                     {
                         Value = pe.PosStatusCode.ToString(),
                         Text = pe.PosStatusTitle,
                         Selected =posStatus!=null && posStatus.Contains(pe.PosStatusCode) 
                     })
                     .ToArray();
            ViewBagWrapper.ListBag.SetList("posStatusCodeItems", posStatusCodeItems, ViewData);
            var task = GetTask();
            var intArr = status.ToParamList().CastToIntegerList();
            var statusCodeItems = task.GetPositionStatus().ToArray()
                .Select(pe => new SelectListItemExtension
                {
                    Value = ((int)pe).ToString(),
                    Text = pe.ToString(),
                    Selected = intArr!=null && intArr.Contains((int)pe)
                })
                .ToArray();
            ViewBagWrapper.ListBag.SetList("statusCodeItems", statusCodeItems, ViewData);
        }

        [HasAnyAdminRole]
        public ActionResult ManageCostCentre(int id)
        {
            ViewBagWrapper.FormOperations.SetFormType(FormType.Edit, ViewData);
            ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.CostCentre, ViewData);
            var position = Repository.GetPositionById(id);
            if(position == null)
                {
                var msg = MessageHelper.NotFoundMessage("position");
                throw new InvalidOperationException(msg);
                }
            if(position.IsDeleted())
                {
                return RedirectToAction("ManageSummary", new { id });

                }

            if (position.CostCentreDetail == null)
            {
                position.CostCentreDetail = new CostCentreDetail();
            }

            var wf = SetWorkflowEngine(position);
            if(!wf.GetWorkflowObjectPrivilege().CanEdit)
                {
                return RedirectToAction("ManageSummary", new { id });
                }

            GetErrorsFromTempData();
            return View("Manage", position);
        }

        [HasAnyAdminRole]
        public ActionResult SaveCostCentre(FormCollection collection)
        {
            var positionId = Convert.ToInt32(collection["PositionId"]);
            var costCentre = collection["CostCentre"];
            var fund = collection["Fund"];
            var payrollWBS = collection["PayrollWBS"];

            var rccPayrollCode = collection["RCCJDEPayrollCode"];
            var glAccount = collection["GLAccount"];

            var costCentreObj = ServiceRepository.CostCentreDetailRepository()
                .List().FirstOrDefault(r => r.PositionId == positionId);
            
            if (costCentreObj == null)
            {
                costCentreObj = new CostCentreDetail
                {
                    PositionId = positionId,
                    CostCentre = costCentre,
                    Fund = fund,
                    PayrollWBS = payrollWBS,
                    RCCJDEPayrollCode = rccPayrollCode,
                    GLAccount = glAccount
                };

                ServiceRepository.CostCentreDetailRepository().Insert(costCentreObj);
               
            }
            else
            {

               var newCostCentre = new CostCentreDetail
                {
                    CostCentre = costCentre,
                    Fund = fund,
                    PayrollWBS = payrollWBS,
                    RCCJDEPayrollCode = rccPayrollCode,
                    GLAccount = glAccount
                };

                var historyChanges = ServiceRepository.PositionHistoryRepository()
                  .GetCostCentreChanges(costCentreObj, newCostCentre);

                costCentreObj.CostCentre = costCentre;
                costCentreObj.Fund = fund;
                costCentreObj.PayrollWBS = payrollWBS;
                costCentreObj.RCCJDEPayrollCode = rccPayrollCode;
                costCentreObj.GLAccount = glAccount;

                ServiceRepository.CostCentreDetailRepository().Update(costCentreObj, true);

                var position = Repository.GetBasePositionById(positionId);
                
                if (position.StatusId != (int) Enums.StatusValue.Draft)
                {
                    //add to history
                    ServiceRepository.PositionHistoryRepository()
                        .LogHistoryOtherPositionItems(positionId,(int)Enums.ActionType.Update,
                        position.StatusId, position.StatusId,
                        "Updated cost centre info to this position. " + historyChanges.ToString(), CurrentUser.UserName);
               }
                
            }


            ViewBagWrapper.FormOperations.SetFormType(positionId == 0 ? FormType.Create : FormType.Edit, ViewData);
            ViewBagWrapper.FormOperations.SetNullModel(true, ViewData);
            ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.CostCentre, ViewData);
            return RedirectToAction("ManageActions",new {id=positionId});
        }

        //This is the position description, role description
        [HasDoeRole]
        public ActionResult ManageSummary(int id)
        {
            ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.Summary, ViewData);

            var position = Repository.GetPositionForSummary(id);

            if (position == null)
            {
                var msg = MessageHelper.NotFoundMessage("position");
                throw new HttpException(msg);
            }
            var workflowEngine=SetWorkflowEngine(position);
            if(!workflowEngine.GetWorkflowObjectPrivilege().CanRead)
                {
                var msg = MessageHelper.AccessDeniedMessage();
                throw new AccessDeniedException(msg);
                }
           
            
                if (position.RolePositionDescription != null)
                {
                    //Do switch,  whether it is for Position Desc or Role Desc
                    if (position.RolePositionDescription.IsPositionDescription)
                    {
                        var positionDesc =
                            ServiceRepository.PositionDescriptionRepository()
                                .LoadPositionDescById(position.RolePositionDescriptionId);

                        if (positionDesc != null)
                        {
                            var posDescTasks = PosRdPdFactory.Create(positionDesc).BuildTasks();
                            if (posDescTasks.Any())
                            {
                                ViewBagWrapper.ValidationTaskBag.SetTasks(posDescTasks, ViewData);
                            }
                        }
                        ViewBagWrapper.PositionBag.SetDescriptionTypeBag(Enums.DescriptionType.Position, ViewData);
                    }
                    else
                    {
                        //Role Description
                        var roleDesc =
                            ServiceRepository.RoleDescriptionRepository()
                                .LoadRoleDescWithCapabilityFramework(position.RolePositionDescriptionId);
                        roleDesc.RoleCapabilities = RoleDescriptionExtensions.SortCapabilityGroup(roleDesc).ToList();

                        var roleDescTasks = PosRdPdFactory.Create(roleDesc).BuildTasks();
                        if (roleDescTasks.Any())
                        {
                            ViewBagWrapper.ValidationTaskBag.SetTasks(roleDescTasks, ViewData);

                        }

                        var displayObj = SetRoleDescForPositionDisplay(position,
                            roleDesc.RolePositionDescription.GradeCode);

                        ViewBagWrapper.SetGeneralObject("RoleDescForPosition", displayObj, ViewData);
                        ViewBagWrapper.SetGeneralObject("RolePositionDesc", roleDesc.RolePositionDescription, ViewData);
                        ViewBagWrapper.PositionBag.SetDescriptionTypeBag(Enums.DescriptionType.Role, ViewData);
                    }

                }
                else
                {
                    ViewBagWrapper.PositionBag.SetDescriptionTypeBag(Enums.DescriptionType.None, ViewData);

                }

            var childPositions = position.Positions.AsQueryable<Position>();
            var filtered = workflowEngine.Task.FilterPositions(childPositions).Where(pos=>pos.StatusId!=(int)Enums.StatusValue.Deleted);
            position.Positions = filtered.ToList();
            return View("Manage", position);
        }

        //display properties
        private RolePositionDescriptionLight SetRoleDescForPositionDisplay(Position position, string gradeCode)
        {

            //Reporting line is who you report to
            var reportToPosition = Repository.ListForReportTo().FirstOrDefault(p => p.PositionId == position.ReportToPositionId);
            var reportingLine = string.Empty;
            if (position.ReportToPositionId == -1)
            {
                reportingLine = Enums.DirectReportDefault.Nil.ToString();
            }
            else if (reportToPosition != null)
            {
                reportingLine = reportToPosition.PositionNumber + " " + reportToPosition.PositionTitle+" ";
                if (reportToPosition.RolePositionDescription != null)
                {
                    reportingLine = reportingLine + reportToPosition.RolePositionDescription.GradeCode;
                }

            }
           
            else
            {
                reportingLine = Enums.DirectReportDefault.Nil.ToString();
            }
            //DirectReports is the subodinates whom report to you
            //Get direct reports (subordinates) positions
            var subOrdinates = Repository.BaseList().Where(s => s.ReportToPositionId == position.PositionId
                  && (s.StatusId == (int)Enums.StatusValue.Approved || s.StatusId == (int)Enums.StatusValue.Imported));
            
            StringBuilder sb = new StringBuilder();

            if (subOrdinates != null && subOrdinates.Any())
            {
                //Load positions from the list of subordinate positionIds
                var titleGrouped = subOrdinates.GroupBy(n => new { n.PositionTitle, n.RolePositionDescription.GradeCode }).
                    Select(group =>
                        new
                        {
                            PositionTitle = group.Key.PositionTitle,
                            GradeCode = group.Key.GradeCode,
                            Count = group.Count()
                        });

                sb.Append("<ul>");
                foreach (var p in titleGrouped)
                {
                    if (p.Count > 1)
                    {
                        sb.Append("<li>"+p.PositionTitle + " " + p.GradeCode + " (" + p.Count + ")</li>");
                    }
                    else
                    {
                        sb.Append("<li>" + p.PositionTitle + " " + p.GradeCode + "</li>");
                    }
                }
                sb.Append("</ul>");
            }

            var displayObj = new RolePositionDescriptionLight
            {
                IsRoleDescForPosition = true,
                DirectorateOverview = position.Unit.BusinessUnit.Directorate.DirectorateOverview,
                DirectReportsDisplay = string.IsNullOrEmpty(sb.ToString()) ? Enums.DirectReportDefault.Nil.ToString(): sb.ToString(),
                ReportingLineDisplay = reportingLine,
                GradeCode = gradeCode
            };
            return displayObj;
        }



        //Position Description PDF
        public ActionResult PositionDescPdf(int id)
        {
            ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.Summary, ViewData);
            //Generating PDF
            var srv = ServiceRepository.PdfService();

            var position = Repository.ListForPdRoleDesc().FirstOrDefault(p => p.PositionId == id);
            if (position == null)
            {
                var msg = MessageHelper.NotFoundMessage("position");
                throw new HttpException(msg);
            }

            if (position != null && position.RolePositionDescriptionId ==-1)
            {
                var msg = MessageHelper.NotFoundMessage("position description");
                throw new HttpException(msg);
            }

                var result=srv.GeneratePdf(position);
                return File(result.OutputFileFullPath, MediaTypeNames.Application.Pdf, result.OutputFileName);
           
        }


        // Role Description for Position PDF

        public ActionResult RoleDescPdf (int id)
        {
            ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.Summary, ViewData);
            var position = Repository.ListForPdRoleDesc().SingleOrDefault(p => p.PositionId == id);

            if (position == null)
            {
                var msg = MessageHelper.NotFoundMessage("position");
                throw new HttpException(msg);
            }
            if (position != null && position.RolePositionDescriptionId == -1)
            {
                var msg = MessageHelper.NotFoundMessage("role description");
                throw new HttpException(msg);
            }

            //Generating PDF
            var srv = ServiceRepository.PdfService();
                var result = srv.GeneratePdf(position);
                return File(result.OutputFileFullPath, MediaTypeNames.Application.Pdf, result.OutputFileName);
         
        }


        [System.Web.Mvc.HttpGet]
        [HasAnyAdminRoleExceptHr]
        public ActionResult WfHistory(int id)
        {
           ViewBagWrapper.PositionBag.SetPositionWizardBag(Enums.PositionWizardStep.History, ViewData);
            
            if (id == 0)
            {
                return View("Manage", new Position());
            }
           
            var model = Repository.GetPositionById(id);
            SetWorkflowEngine(model);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            var historyList =
            ServiceRepository.PositionHistoryRepository().List().Where(r => r.PositionId == id);

            ViewBagWrapper.SetGeneralObject("historyList", historyList, ViewData);

            return View("Manage", model);
            
        }

    }
}


