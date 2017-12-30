using System;
using System.IO;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Doe.Ls.ProjectTemplate.Core.BL.UI.Dashboards;
using Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using System.Linq;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
{
    [TestClass]
    public class DashboardItemTests : SecurityBase
        {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }

        [TestMethod]
        public void GetClassName()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var powerUser = GetSampleUsers(Enums.UserRole.PowerUser, factory, true, 1).FirstOrDefault();

            var task = UserTaskFactory.GetTask(powerUser, factory);

            var item = DashboardItem.ManagePositionDescriptions(task);
            Console.WriteLine(item.Url);
            Console.WriteLine(item.GetClassName());
            var className = item.GetClassName();

            Assert.IsFalse(className.Contains("?"), "className.Contains(? )");

            item = DashboardItem.ManageDivisionApproverRoleUsersTask(task);
            Console.WriteLine(item.Url);
            Console.WriteLine(item.GetClassName());
            

            item = DashboardItem.ManageDirectorateDataEntryRoleUsersTask(task);
            Console.WriteLine(item.Url);
            Console.WriteLine(item.GetClassName());


            }

        [TestCleanup]
        public void CleanUp()
        {
            

        }


    }

}

