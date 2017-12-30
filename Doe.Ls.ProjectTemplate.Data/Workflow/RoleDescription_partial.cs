using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doe.Ls.ProjectTemplate.Data
    {
    public partial class RoleDescription : IWorkflowObject
        {
        int IWorkflowObject.WorkflowObjectId => this.RoleDescriptionId;
        WorkflowObjectType IWorkflowObject.WorkflowObjectType => WorkflowObjectType.RoleDescription;
        int IWorkflowObject.WorkflowObjectVersion => this.RolePositionDescription.Version;
        string IWorkflowObject.WorkflowObjectTitle => RolePositionDescription.Title;
        StatusValue IWorkflowObject.WorkflowObjectStatus => this.RolePositionDescription.StatusValue;
        string IWorkflowObject.WorkflowObjectCreatedBy => RolePositionDescription.CreatedBy;
        DateTime? IWorkflowObject.WorkflowObjectCreatedDate => RolePositionDescription.CreatedDate;
        string IWorkflowObject.WorkflowObjectLastModifiedBy => RolePositionDescription.LastModifiedBy;
        DateTime? IWorkflowObject.WorkflowObjectLastModifiedDate => RolePositionDescription.LastModifiedDate;
        }
    }
