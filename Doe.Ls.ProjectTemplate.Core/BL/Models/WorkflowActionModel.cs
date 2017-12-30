using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
    {
    public class WorkflowActionModel
        {
        public int ActionId { get; set; }
        public int WfObjectId { get; set; }
        public WorkflowObjectType ObjectType { get; set; }
        public string Comment { get; set; }
        public  DateTime? ApprovalDate { get; set; }
        public Enums.StatusValue? NextStatus { get; set; }

        public Enums.WorkflowActions GetWorkflowAction()
        {

            return (Enums.WorkflowActions) ActionId;
        }
        }

    }
