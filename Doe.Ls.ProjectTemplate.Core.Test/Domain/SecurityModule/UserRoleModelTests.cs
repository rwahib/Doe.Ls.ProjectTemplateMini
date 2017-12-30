using System;
using System.Linq;
using Castle.Core.Internal;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule
    {
    [TestClass]
    public class UserRoleModelTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }

        [TestMethod]
        public void DirectorateEndorserDefaultTest()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv=new ServiceRepository(factory);
            var allDirectorates = srv.DirectorateRepository().List().Select(d => d.DirectorateId).ToArray();

            var dirEndorser = GenerateSampleUser(Enums.UserRole.DirectorateEndorser, factory);
            dirEndorser.InitialiseRoles(factory);


            foreach (var userRoleModel in dirEndorser.ActiveRoleOrgLevelList.Where(r=>r.RoleId==(int)Enums.UserRole.DirectorateEndorser))
            {
                var currentDirectorate = userRoleModel.StructureId.ToInteger();
                if (allDirectorates.Contains(currentDirectorate))
                {
                    Assert.IsTrue(
                        dirEndorser.DefaultOrganisationalModel.Directorates.Contains(
                            userRoleModel.StructureId.ToInteger()),
                        "dirEndorser.DefaultOrganisationalModel.Directorates.Contains(userRoleModel.StructureId.ToInteger())");
                }
            }


        }

        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();

            }

        }
    }
