using Doe.Ls.ProjectTemplate.Data;
using Org.BouncyCastle.Security;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks
{
    public class PosRdPdFactory
    {
        public static IPositionRdPdTasks Create(IWorkflowObject workflowObject)
        {

            if(workflowObject is Position) return new PositionTasks(workflowObject as Position);
            if(workflowObject is PositionDescription) return new PositionDescTasks(workflowObject as PositionDescription);
            if(workflowObject is RoleDescription) return new RoleDescTasks(workflowObject as RoleDescription);

            throw new InvalidParameterException($"invalid parameter type {workflowObject.GetType()}");
        }
        
    }
}