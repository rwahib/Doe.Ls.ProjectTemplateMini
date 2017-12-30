﻿using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule
    {
    [TestClass]
    public class DirectoratePrivilegeTests: SecurityBase
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
            var powerUser = GetSampleUsers(Enums.UserRole.PowerUser, factory,true,1).FirstOrDefault();
            
            var task= UserTaskFactory.GetTask(powerUser, factory);
            var priv = task.GetDirectoratePrivilege();
            Console.WriteLine(priv);
            Assert.IsTrue(priv.FullControl, "priv.FullControl");
            

        }

        [TestMethod]
        public void TestPrivilegeAdminPowerUser()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var powerUser = GetSampleUsers(Enums.UserRole.PowerUser, factory, true, 1).FirstOrDefault();
            if (srv.DirectorateRepository().List().Any())
            {
                var directorate = srv.DirectorateRepository().List().OrderByDescending(e=>e.DirectorateId).FirstOrDefault();


                var task = UserTaskFactory.GetTask(powerUser, factory);
                var priv = task.GetDirectoratePrivilege(directorate);
                if (directorate.BusinessUnits.Any()|| directorate.FunctionalAreas.Any())
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


        [TestMethod]
        public void TestGeneralPrivilegeDoe()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);

            var doeUser = GetSampleUsers(Enums.UserRole.DoEUser, factory, true, 1).FirstOrDefault();


            var task = UserTaskFactory.GetTask(doeUser, factory);
            var priv = task.GetDirectoratePrivilege();
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