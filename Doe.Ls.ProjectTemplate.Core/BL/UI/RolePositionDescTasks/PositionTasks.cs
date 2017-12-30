using System.Linq;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Http;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks
    {
    public class PositionTasks : PositionRdPdTasksBase
        {

        private readonly Position _position;

        internal PositionTasks(Position position):base(position)
            {
            _position = position;
            }

        /// <summary>
        /// The following fields must be filled:
        ///1. Unit Id can't be -1
        ///2. RolePositionDescId can't be -1
        ///3. Its PositionInformation can't be NULL (empty)
        ///4. Location can't be -1
        ///5. if path != '/1/' -- this is the Top position(secretary),then ReportToPositionId can't be -1
        ///6. PositionLevel can't be -1
        ///7. Cost Centre
        /// </summary>
        protected override void BuildTasksCore()
        {
            if(_position.UnitId == -1)
                {
                Tasks.Add(new RolePositionDescTask("Position", "Position Team", "Edit", ""));
                }
            if(_position.RolePositionDescriptionId == -1)
                {
                Tasks.Add(new RolePositionDescTask("RoleDescription", "Role / Position Description", "ManageBasicDetails", ""));
                }

            if(_position.LocationId == -1)
                {
                Tasks.Add(new RolePositionDescTask("Position", "Position Location", "Edit", ""));
                }

            if(_position.PositionLevelId == -1)
                {
                Tasks.Add(new RolePositionDescTask("Position", "Position Level", "Edit", ""));
                }

            if(_position.PositionPath != "/1/" && _position.ReportToPositionId == -1)
                {
                //This is NOT the top position, so needs to fill in
                Tasks.Add(new RolePositionDescTask("Position", "Reports To Position", "Edit", ""));
                }

            if(_position.PositionInformation == null)
                {
                Tasks.Add(new RolePositionDescTask("Position", "Position extra information", "EditMoreInfo", ""));
                }
            else
                {
                //if(_position.PositionInformation.PositionTypeCode == "-1")
                //    {
                //    Tasks.Add(new RolePositionDescTask("Position", "Position Type", "EditMoreInfo", ""));
                //    }

                //if(_position.PositionInformation.EmployeeTypeCode == "-1")
                //    {
                //    Tasks.Add(new RolePositionDescTask("Position", "Employee Type", "EditMoreInfo", ""));
                //    }

                //if(_position.PositionInformation.PositionStatusCode == "-1")
                //    {
                //    Tasks.Add(new RolePositionDescTask("Position", "Position Status code", "EditMoreInfo", ""));
                //    }

                //if(_position.PositionInformation.OccupationTypeCode == "-1")
                //    {
                //    Tasks.Add(new RolePositionDescTask("Position", "Occupation Type code", "EditMoreInfo", ""));
                //    }

                }

            
            if (_position.CostCentreDetail == null)
            {
                Tasks.Add(new RolePositionDescTask("Position", "Cost Centre details", "ManageCostCentre", ""));
            }
            else if (string.IsNullOrEmpty(_position.CostCentreDetail.CostCentre) ||
                string.IsNullOrEmpty(_position.CostCentreDetail.Fund) ||
                string.IsNullOrEmpty(_position.CostCentreDetail.PayrollWBS) ||
                string.IsNullOrEmpty(_position.CostCentreDetail.RCCJDEPayrollCode) ||
                string.IsNullOrEmpty(_position.CostCentreDetail.GLAccount))
            {
                Tasks.Add(new RolePositionDescTask("Position", "Cost Centre details fields", "ManageCostCentre", ""));
            }

            if (!Tasks.Any()) // Check related position or role descriptions
                {
                    if(_position.RolePositionDescription.StatusValue.GetEnum() != Enums.StatusValue.Approved &&
                    _position.RolePositionDescription.StatusValue.GetEnum() != Enums.StatusValue.Imported)
                    {
                        IPositionRdPdTasks relatedTaskFactory;
                    if(_position.RolePositionDescription.IsPositionDescription)
                        {
                            if (_position.RolePositionDescription.PositionDescription == null)
                            {
                            Tasks.Add(new RolePositionDescTask("Related position description", "Related position description tasks.", "", HttpHelper.GetActionUrl("ManageActions", "PositionDescription", new { id = _position.RolePositionDescription.RolePositionDescId})));
                            }
                        else { 
                        relatedTaskFactory = PosRdPdFactory.Create(_position.RolePositionDescription.PositionDescription);
                        if(!relatedTaskFactory.IsValid())
                            {

                            Tasks.Add(new RolePositionDescTask("Related position description", "Related position description tasks.", "", HttpHelper.GetActionUrl("ManageActions", "PositionDescription", new { id = _position.RolePositionDescription.RolePositionDescId})));
                                }
                        }
                        }
                    else
                    {
                        if (_position.RolePositionDescription.RoleDescription == null)
                        {
                            Tasks.Add(new RolePositionDescTask("Related role description", "Related role description tasks.", "", HttpHelper.GetActionUrl("ManageActions", "RoleDescription", new { id = _position.RolePositionDescription.RolePositionDescId})));
                            }
                        else
                        {
                            relatedTaskFactory = PosRdPdFactory.Create(_position.RolePositionDescription.RoleDescription);
                            if (!relatedTaskFactory.IsValid())
                            {

                                Tasks.Add(new RolePositionDescTask("Related role description", "Related role description tasks.", "",HttpHelper.GetActionUrl("ManageActions", "RoleDescription",new {id = _position.RolePositionDescription.RolePositionDescId }) ));
                            }
                        }
                    }
                    }
                }


            }


        }
    }