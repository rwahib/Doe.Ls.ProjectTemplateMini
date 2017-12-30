using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule
    {
    [TestClass]
    public class LocationPrivilegeTests : SecurityBase
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
            var priv = task.GetLocationPrivilege();

            Console.WriteLine(priv);

            Assert.IsTrue(priv.CanRead && priv.CanApprove && priv.CanSubmit,"priv.CanRead && priv.CanApprove && priv.CanSubmit");
            
            }

        [TestMethod]
        public void TestPrivilegeAdminPowerUser()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var ctx = srv.GetUnitOfWork().DbContext as SampleProjectTemplateEntities;
            var powerUser = GetSampleUsers(Enums.UserRole.PowerUser, factory, true, 1).FirstOrDefault();
            if(srv.LocationRepository().List().Any())
                {
                foreach(var location in srv.LocationRepository().List().Take(5).ToArray())
                    {
                    var task = UserTaskFactory.GetTask(powerUser, factory);
                    var priv = task.GetLocationPrivilege(location);
                    var counts = srv.PositionRepository().List().Count(p => p.LocationId == location.LocationId);
                    if(counts > 0)
                        {
                        Assert.IsFalse(priv.CanDelete, "priv.CanDelete");
                        }
                    else
                        {
                        Assert.IsTrue(priv.CanDelete, "priv.CanDelete");
                        }

                    Console.WriteLine(priv);

                    }
                }

            }


        [TestMethod]
        public void TestGeneralPrivilegeDoe()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);

            var doeUser = GetSampleUsers(Enums.UserRole.DoEUser, factory, true, 1).FirstOrDefault();


            var task = UserTaskFactory.GetTask(doeUser, factory);
            var priv = task.GetLocationPrivilege();
            Console.WriteLine(priv);
            Assert.IsFalse(priv.CanCreate, "priv.CanCreate");


            }

        [TestMethod]
        public void TestPrivilege()
            {


            }

        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();

            }

        }
    }
