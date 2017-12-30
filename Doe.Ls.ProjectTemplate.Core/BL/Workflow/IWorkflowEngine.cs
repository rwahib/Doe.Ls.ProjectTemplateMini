using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Data;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace Doe.Ls.ProjectTemplate.Core.BL.Workflow
    {
    public interface IWorkflowEngine
        {
        IWorkflowObject WorkflowObject { get; }
        T GetWorkflowObject<T>() where T: class,IWorkflowObject;
        IPositionRdPdTasks PositionRdPdTasks { get; }
        IUserTask Task { get; }
        List<WorkflowAction> GetWorkflowObjectActions();
        Result ApplyAction(WorkflowActionModel model,bool relatedWorkflowObjects=false,NameValueCollection formCollection=null);
        Enums.Privilege GetWorkflowObjectPrivilege(PrivilegeOptions options=null);
        }

        public class PrivilegeOptions
        {
            public bool GetDownloadPrivilege { get; set; }
        
        }
    }



