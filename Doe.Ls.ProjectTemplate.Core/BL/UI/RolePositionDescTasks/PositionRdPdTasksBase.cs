using System.Collections.Generic;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks
{
    public abstract class PositionRdPdTasksBase : IPositionRdPdTasks
    {
        protected List<RolePositionDescTask> Tasks;

        protected PositionRdPdTasksBase(IWorkflowObject workflowObject)
        {
            WorkflowObject = workflowObject;
        }

        public IWorkflowObject WorkflowObject { get; }

        public virtual bool IsValid()
        {
            BuildTasks();
            if (Tasks == null || !Tasks.Any()) return true;
            return Tasks.All(task => task.Status == Enums.ValidationTasksStatus.Completed);
        }
        public virtual List<RolePositionDescTask> BuildTasks()
        {
            if (Tasks != null) return Tasks;
            Tasks=new List<RolePositionDescTask>();
            if (this.WorkflowObject.WorkflowObjectStatus.GetEnum() != Enums.StatusValue.Approved &&
                this.WorkflowObject.WorkflowObjectStatus.GetEnum() != Enums.StatusValue.Imported)
            {
                BuildTasksCore();
            }
           
            return Tasks;
        }

        protected abstract void BuildTasksCore();

    }
}