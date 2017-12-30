using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks
    {
    public class PositionDescTasks : PositionRdPdTasksBase
        {

        private readonly PositionDescription _positionDesc;
        
        internal PositionDescTasks(PositionDescription positionDescription):base(positionDescription)
            {
            _positionDesc = positionDescription;
            }

        protected override void BuildTasksCore()
            {
            if(string.IsNullOrEmpty(_positionDesc.RolePositionDescription.DocNumber))
                {
                Tasks.Add(new RolePositionDescTask("Overview", "Document number", "ManageOverview", ""));
                }
            if(string.IsNullOrEmpty(_positionDesc.RolePositionDescription.GradeCode))
                {
                Tasks.Add(new RolePositionDescTask("Overview", "Grade", "ManageOverview", ""));
                }
            if(string.IsNullOrEmpty(_positionDesc.RolePositionDescription.Title))
                {
                Tasks.Add(new RolePositionDescTask("Overview", "Title", "ManageOverview", ""));
                }

            if(string.IsNullOrEmpty(_positionDesc.BriefRoleStatement))
                {
                Tasks.Add(new RolePositionDescTask("Overview", "Brief role statement", "ManageOverview", ""));
                }
            if(string.IsNullOrEmpty(_positionDesc.StatementOfDuties))
                {
                Tasks.Add(new RolePositionDescTask("Overview", "Statement of duties", "ManageOverview", ""));
                }
            else
            {
                var count = CommonHelper.GetUIListCount(_positionDesc.StatementOfDuties);
                if(count < 6)
                    {
                    Tasks.Add(new RolePositionDescTask("Overview", "The Statement of duties must consist of 6-8 bullet points", "ManageOverview", ""));
                    }
                else if(count > 8)
                    {
                        {
                        Tasks.Add(new RolePositionDescTask("Overview", "The Statement of duties must consist of 6-8 bullet points", "ManageOverview", ""));
                        }
                    }
                }

            if(!_positionDesc.PositionFocusCriterias.Any())
                {
                Tasks.Add(new RolePositionDescTask("Selection Criteria", "Selection Criteria", "ListSelectionCriteria", ""));
                }
            else if(_positionDesc.PositionFocusCriterias.Count < 8 || _positionDesc.PositionFocusCriterias.Count > 10)
                {
                Tasks.Add(new RolePositionDescTask("Selection Criteria", "The selection criteria must consist of 8-10 bullet points.", "ListSelectionCriteria", ""));
                }
            
            }

        

        }
    }
