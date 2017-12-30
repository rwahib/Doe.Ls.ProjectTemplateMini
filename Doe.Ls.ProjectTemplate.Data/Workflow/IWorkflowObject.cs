using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.ProjectTemplate.Data
    {
    public interface IWorkflowObject
        {
        int WorkflowObjectId { get; }
        WorkflowObjectType WorkflowObjectType { get; }
        int WorkflowObjectVersion { get; }
        string WorkflowObjectTitle { get; }
        StatusValue WorkflowObjectStatus { get; }
        string WorkflowObjectCreatedBy { get; }
        DateTime? WorkflowObjectCreatedDate { get; }

        string WorkflowObjectLastModifiedBy { get; }
        DateTime? WorkflowObjectLastModifiedDate { get; }
        
        }
    }
