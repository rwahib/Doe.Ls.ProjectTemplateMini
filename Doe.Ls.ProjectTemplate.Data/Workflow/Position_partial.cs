using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.ProjectTemplate.Data
    {
    public partial class Position : IWorkflowObject
        {
        int IWorkflowObject.WorkflowObjectId => this.PositionId;
        WorkflowObjectType IWorkflowObject.WorkflowObjectType => WorkflowObjectType.Position;

         int IWorkflowObject.WorkflowObjectVersion => 0;
         string IWorkflowObject.WorkflowObjectTitle => PositionTitle;
         StatusValue IWorkflowObject.WorkflowObjectStatus => StatusValue;
         string IWorkflowObject.WorkflowObjectCreatedBy => CreatedBy;
         DateTime? IWorkflowObject.WorkflowObjectCreatedDate => CreatedDate;
         string IWorkflowObject.WorkflowObjectLastModifiedBy => LastModifiedBy;
         DateTime? IWorkflowObject.WorkflowObjectLastModifiedDate => LastModifiedDate;

        
        }
    }
