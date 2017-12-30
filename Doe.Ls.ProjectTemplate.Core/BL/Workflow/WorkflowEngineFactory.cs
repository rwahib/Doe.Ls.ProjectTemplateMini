using System;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Workflow
{
    public static class WorkflowEngineFactory
    {
        public static IWorkflowEngine CreatEngine(IWorkflowObject workflowObject, UserInfoExtension userInfo,
            IRepositoryFactory factory)
        {
            if(workflowObject is Position) return new WorkflowEnginePosition(workflowObject as Position, userInfo, factory);

            if(workflowObject is PositionDescription) return new WorkflowEnginePositionDescription(workflowObject as PositionDescription, userInfo, factory);
            if(workflowObject is RoleDescription) return new WorkflowEngineRoleDescription(workflowObject as RoleDescription, userInfo, factory);

            throw new InvalidOperationException($"{workflowObject.GetType().Name} is not recognised");

        }


        public static IWorkflowEngine CreatEngine(IWorkflowObject workflowObject, IUserTask task)
        {
            if (workflowObject == null)
            {
                throw new ArgumentNullException(nameof(workflowObject));
            }
            if(workflowObject is Position) return new WorkflowEnginePosition(workflowObject as Position, task);

            if(workflowObject is PositionDescription)
                return new WorkflowEnginePositionDescription(workflowObject as PositionDescription, task);
            if(workflowObject is RoleDescription) return new WorkflowEngineRoleDescription(workflowObject as RoleDescription, task);

            throw new InvalidOperationException($"{workflowObject.GetType().Name} is not recognised");

        }

    }
}