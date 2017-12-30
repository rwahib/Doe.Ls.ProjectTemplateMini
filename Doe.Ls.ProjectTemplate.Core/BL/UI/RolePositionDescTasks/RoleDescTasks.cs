using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks
{
    public class RoleDescTasks : PositionRdPdTasksBase
        {
        
        private readonly RoleDescription _roleDesc;

        internal RoleDescTasks(RoleDescription roleDescription):base(roleDescription)
        {
            _roleDesc = roleDescription;
            }

        protected override void BuildTasksCore()
        {
            
            if (string.IsNullOrEmpty(_roleDesc.RolePositionDescription.DocNumber))
            {
                Tasks.Add(new RolePositionDescTask("DocNumber", "Document number", "ManageBasicDetails", ""));
               
            }
            if (string.IsNullOrEmpty(_roleDesc.RolePositionDescription.GradeCode))
            {
                Tasks.Add(new RolePositionDescTask("GradeCode", "Grade", "ManageBasicDetails", ""));
            }
            if (string.IsNullOrEmpty(_roleDesc.RolePositionDescription.Title))
            {
                Tasks.Add(new RolePositionDescTask("Title", "Title", "ManageBasicDetail", ""));
            }

            //if (string.IsNullOrEmpty(_roleDesc.ANZSCOCode))
            //{
            //    Tasks.Add(new RolePositionDescTask("ANZSCOCode", "ANZSCO code", "ManageBasicDetails", ""));
            //}

            //if (string.IsNullOrEmpty(_roleDesc.PCATCode))
            //{
            //    Tasks.Add(new RolePositionDescTask("PCATCode", "PCAT code", "ManageBasicDetails", ""));
            //}


            if (string.IsNullOrEmpty(_roleDesc.RolePrimaryPurpose))
            {
                Tasks.Add(new RolePositionDescTask("RolePrimaryPurpose", "Role Primary Purpose", "ManageBasicDetails", ""));
            }

            
            if (string.IsNullOrEmpty(_roleDesc.DecisionMaking))
            {
                Tasks.Add(new RolePositionDescTask("DecisionMaking", "Decision Making", "ManageBasicDetails", ""));
            }


            if (string.IsNullOrEmpty(_roleDesc.KeyAccountabilities))
            {
                Tasks.Add(new RolePositionDescTask("KeyAccountabilities", "Key Accountabilities", "ManageKeyAccountabilities", ""));
            }

            if (string.IsNullOrEmpty(_roleDesc.KeyChallenges))
            {
                Tasks.Add(new RolePositionDescTask("KeyChallenges", "Key Challenges", "ManageKeyAccountabilities", ""));
            }


            if(_roleDesc.ManagerRole.HasValue && _roleDesc.ManagerRole ==true && string.IsNullOrEmpty(_roleDesc.BudgetExpenditureValue))
            { 
                Tasks.Add(new RolePositionDescTask("BudgetExpenditureValue", "Budget Expenditure values", "ManageBudget", ""));
            }

            if (string.IsNullOrEmpty(_roleDesc.EssentialRequirements))
            {
                Tasks.Add(new RolePositionDescTask("EssentialRequirements", "Essential Requirements","ManageEssentialReq", ""));
            }

            if (!_roleDesc.KeyRelationships.Any())
            {
                Tasks.Add(new RolePositionDescTask("KeyRelationships", "Key Relationships", "ManageKeyRelationships", ""));
            }
            if (!_roleDesc.RoleCapabilities.Any())
            {
                Tasks.Add(new RolePositionDescTask("Capabilities", "Capability Framework", "ManageCapabilities", ""));
            }

           

        }

       
        }
}
