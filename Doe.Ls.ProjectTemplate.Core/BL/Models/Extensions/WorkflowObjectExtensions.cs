using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions
    {
    public static class WorkflowObjectExtensions
        {
        public static bool IsValid(this IWorkflowObject wfObject)
            {
            return (PosRdPdFactory.Create(wfObject)).IsValid();
            }
        public static bool IsLive(this IWorkflowObject wfObject)
            {
            return (wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Imported ||
                wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Approved);
            }
        public static bool IsDeleted(this IWorkflowObject wfObject)
        {
            return wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Deleted;
        }

        public static string Info(this IWorkflowObject workflowObject)
            {
            if(workflowObject is Position)
                {
                var unit = (workflowObject as Position).Unit;
                return
                    $"{unit.BusinessUnit.Directorate.Executive.ExecutiveTitle}/{unit.BusinessUnit.Directorate.DirectorateName}/{unit.BusinessUnit.BUnitName}/{unit.UnitName}";
                }

            RolePositionDescription rpd = null;
            if(workflowObject is Position) rpd = (workflowObject as Position).RolePositionDescription;
            if(workflowObject is PositionDescription) rpd = (workflowObject as PositionDescription).RolePositionDescription;
            if(workflowObject is RoleDescription) rpd = (workflowObject as RoleDescription).RolePositionDescription;

            return $"{rpd.Positions.Count} linked positions";

            }
        public static string TitleGrade(this IWorkflowObject workflowObject)
        {
            RolePositionDescription rpd = null;
            if (workflowObject is Position) rpd = (workflowObject as Position).RolePositionDescription;
            if(workflowObject is PositionDescription) rpd = (workflowObject as PositionDescription).RolePositionDescription;
            if(workflowObject is RoleDescription) rpd = (workflowObject as RoleDescription).RolePositionDescription;

            return $"{rpd.DocNumber}-{rpd.Title}-{rpd.GradeCode}";

            }
        }
    }