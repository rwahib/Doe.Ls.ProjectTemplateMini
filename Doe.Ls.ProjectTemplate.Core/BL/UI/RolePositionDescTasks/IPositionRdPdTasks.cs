using System;
using System.Collections.Generic;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks
    {
    public interface IPositionRdPdTasks
        {
        IWorkflowObject WorkflowObject { get; }
        bool IsValid();
        List<RolePositionDescTask> BuildTasks();
        }
    }