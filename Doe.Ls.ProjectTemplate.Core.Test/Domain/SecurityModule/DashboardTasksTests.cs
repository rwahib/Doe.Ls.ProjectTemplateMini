using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule
    {
    [TestClass]
    public class DashboardTasksTests : SecurityBase
        {

        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }

        [TestMethod]
        public void SysUserDashboardTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var userList =
                srv.SysUserRepository().GetSysUsersByRole(Enums.UserRole.SystemAdministrator,true).Take(10).ToList();

            foreach(var sysUser in userList)
                {
                var user = UserInfoExtension.MapSysUser(sysUser, factory);
                var task = UserTaskFactory.GetTask(user, factory);
                    if (user.Email == "jorg.knoflack@det.nsw.edu.au")
                    {
                        
                    }
                Assert.IsTrue(
                    task.Dashboard.DashboardSections.Any(
                        sec => sec.Title.ToLower().Contains("Application configuration tasks".ToLower())
                               && sec.Status == UiStatus.Visible),
                    "task.Dashboard.DashboardSections.Any(sec=>sec.Title.ToLower().Contains(Application configuration tasks.ToLower())&& sec.Status == UiStatus.Visible));");

                }


            }


        [TestMethod]
        public void PowerUserDashboardTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var userList =
                srv.SysUserRepository().GetSysUsersByRole(Enums.UserRole.PowerUser,true).Take(10).ToList();

            foreach(var sysUser in userList)
                {
                var user = UserInfoExtension.MapSysUser(sysUser, factory);
                var task = UserTaskFactory.GetTask(user, factory);

                Assert.IsTrue(
                    task.Dashboard.DashboardSections.Any(
                        sec => sec.Title.ToLower().Contains("Application configuration tasks".ToLower())
                               && sec.Status == UiStatus.Visible),
                    "task.Dashboard.DashboardSections.Any(sec=>sec.Title.ToLower().Contains(Application configuration tasks.ToLower())&& sec.Status == UiStatus.Visible));");

                }


            }


        [TestMethod]
        public void DirectorateDataEntryUserDashboardTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var userList =
                srv.SysUserRepository().GetSysUsersByRole(Enums.UserRole.DirectorateDataEntry, true).Take(20).ToList();

            foreach(var sysUser in userList.Where(u=>u.SysUserRoles.Count<=2))
                {
                var user = UserInfoExtension.MapSysUser(sysUser, factory);
                var task = UserTaskFactory.GetTask(user, factory);
                if(user.HasAdminOrPowerRole())continue;
                Assert.IsFalse(
                    task.Dashboard.DashboardSections.Any(
                        sec => sec.Title.ToLower().Contains("Application configuration tasks".ToLower())
                               && sec.Status == UiStatus.Visible),
                    "task.Dashboard.DashboardSections.Any(sec=>sec.Title.ToLower().Contains(Application configuration tasks.ToLower())&& sec.Status == UiStatus.Visible));");

                }


            }


        [TestMethod]
        public void DirectorateEndorserUserDashboardTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var userList =
                srv.SysUserRepository().GetSysUsersByRole(Enums.UserRole.DirectorateEndorser, true).Take(20).ToList();

            foreach(var sysUser in userList.Where(u => u.SysUserRoles.Count <= 2))
                {
                var user = UserInfoExtension.MapSysUser(sysUser, factory);
                var task = UserTaskFactory.GetTask(user, factory);
                if(user.HasAdminOrPowerRole()) continue;
                    var userRoleSection =
                        task.Dashboard.DashboardSections
                            .FirstOrDefault(s => s.Title.ToLower().Contains("User roles".ToLower()));


                    var item=userRoleSection.DashboardItems.FirstOrDefault(i => i.Url.ToLower().Contains("~/User".ToLower()));

                Assert.IsNotNull(item,"Directorate endorser should access user lookup");
                Assert.IsTrue(item.Status==UiStatus.Visible,"item.Status==UiStatus.Visible");

                }


            }

        [TestMethod]
        public void DoEDashboardTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var user = UserInfoExtension.SystemUser;
            user.ActiveRoleOrgLevelList.Add(new UserRoleModel
                {
                RoleId = Enums.UserRole.DoEUser.ToInteger(),
                IsActive = true,
                OrgLevelId = -1,
                });


            var task = UserTaskFactory.GetTask(user, factory);

            Assert.IsTrue(
                task.Dashboard.DashboardSections.Any(sec => sec.Title.ToLower().Contains("Search".ToLower()) && sec.Status == UiStatus.Visible), "task.Dashboard.DashboardSections.Any(sec => sec.Title.ToLower().Contains('Search'.ToLower())&& sec.Status == UiStatus.Visible)");

            Assert.IsFalse(
                    task.Dashboard.DashboardSections.Any(sec => sec.Title.ToLower().Contains("Configuration".ToLower()) && sec.Status == UiStatus.Visible), "task.Dashboard.DashboardSections.Any(sec => sec.Title.ToLower().Contains('Configuration'.ToLower()) && sec.Status == UiStatus.Visible)");


            }

        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();

            }




        }
    }
