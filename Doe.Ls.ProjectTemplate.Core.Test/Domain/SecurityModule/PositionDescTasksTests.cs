using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule
{
    [TestClass]
    public class PositionDescTasksTests: SecurityBase
    {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }

        [TestMethod]
        public void TestGeneralPrivilegeAdminPowerUser()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var powerUser = GetSampleUsers(Enums.UserRole.PowerUser, factory, true, 1).FirstOrDefault();

            var task = UserTaskFactory.GetTask(powerUser, factory);
            var priv = task.GetPositionDescriptionPrivilege();
            Console.WriteLine(priv);
            Assert.IsTrue(priv.CanCreate, "priv.CanCreate");
            Assert.IsTrue(priv.CanEdit, "priv.CanEdit");
            
        }

        [TestMethod]
        public void TestPowerUserCanEditPd()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var powerUser = GetSampleUsers(Enums.UserRole.PowerUser, factory, true, 1).FirstOrDefault();

            //when PD status is draft, imported
            var pd =
                srv.PositionDescriptionRepository()
                    .List().Where(p=>p.RolePositionDescription.StatusId == (int)Enums.StatusValue.Draft)
                    .OrderByDescending(r => r.RolePositionDescription.RolePositionDescId)
                    .FirstOrDefault();

            if (pd != null)
            {
                var task = UserTaskFactory.GetTask(powerUser, factory);
                var wf=WorkflowEngineFactory.CreatEngine(pd, task);
                var priv = wf.GetWorkflowObjectPrivilege();
                Console.WriteLine(priv);
                Assert.IsTrue(priv.CanEdit, "priv.CanEdit");
            }
            else
            {
                Assert.IsNull(pd);
            }


        }
    }
}
