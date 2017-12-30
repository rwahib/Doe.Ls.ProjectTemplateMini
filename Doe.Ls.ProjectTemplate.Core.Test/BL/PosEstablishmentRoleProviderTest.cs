using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.BL
    {
    [TestClass]
    public class PosEstablishmentRoleProviderTest : TestBase
        {

        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }

        [TestMethod]
        public void GetAllRoles()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
                var srv = new ServiceRepository(factory);
                var db = srv.GetUnitOfWork().DbContext.Database;

            var roleProvider = new ProjectTemplateProvider(factory);
            var roles = roleProvider.GetAllRoles();

            Assert.IsTrue(roles.Contains(Enums.UserRole.SystemAdministrator.ToString()), "roles.Contains(Enums.UserRole.SystemAdministrator.ToString())");
            Assert.IsTrue(roles.Contains(Enums.UserRole.Administrator.ToString()), "roles.Contains(Enums.UserRole.Administrator.ToString())");
            Assert.IsTrue(roles.Contains(Enums.UserRole.PowerUser.ToString()), "roles.Contains(Enums.UserRole.PowerUser.ToString())");
            Assert.IsTrue(roles.Contains(Enums.UserRole.DivisionEditor.ToString()), "roles.Contains(Enums.UserRole.DivisionEditor.ToString())");


            }


        [TestMethod]
        public void UserInRole()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var userRepo = srv.SysUserRepository();
            var roleProvider = new ProjectTemplateProvider(factory);

            var users =
               userRepo.GetSysUsersByRole(Enums.UserRole.SystemAdministrator, true)
                    .Where(u => !u.UserId.Contains(UnitTestToken)).ToArray();

            if(!users.Any()) Console.WriteLine("There are no sys admin to test");


            foreach(var sysUser in users.Take(10).ToList())
                {
                Assert.IsTrue(roleProvider.IsUserInRole(sysUser.UserId, Enums.UserRoleValues.SystemAdministrator));
                }


            users =
               userRepo.GetSysUsersByRole(Enums.UserRole.Administrator, true)
                    .Where(u => !u.UserId.Contains(UnitTestToken)).ToArray();



            if(!users.Any()) Console.WriteLine("There are no administrator to test");

            foreach(var sysUser in users.Take(10).ToList())
                {
                Assert.IsTrue(roleProvider.IsUserInRole(sysUser.UserId, Enums.UserRoleValues.Administrator));
                }


            users =
             userRepo.GetSysUsersByRole(Enums.UserRole.DivisionEditor, true)
                   .Where(u => !u.UserId.Contains(UnitTestToken)).ToArray();



            if(!users.Any()) Console.WriteLine("There are no administrator to test");

            foreach(var sysUser in users.Take(10))
                {
                Assert.IsTrue(roleProvider.IsUserInRole(sysUser.UserId, Enums.UserRoleValues.DivisionEditor));
                }

            }

        [TestMethod]
        public void RolesForUser()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var userRepo = srv.SysUserRepository();
            var roleProvider = new ProjectTemplateProvider(factory);

            var sysAdminUserList = userRepo.GetSysUsersByRole(Enums.UserRole.SystemAdministrator,true)
                
                    .Where(u => !u.UserId.Contains(UnitTestToken)).Take(10).ToList();

            if(!sysAdminUserList.Any()) Assert.Inconclusive("There are no sys admin to test");



            foreach(var sysUser in sysAdminUserList)
                {
                var roles = roleProvider.GetRolesForUser(sysUser.UserId);
                Assert.IsTrue(roles.Contains(Enums.UserRoleValues.SystemAdministrator), "roles.Contains(Enums.UserRoleValues.SystemAdministrator)");
                }


            var adminUserList =
             userRepo.GetSysUsersByRole(Enums.UserRole.Administrator, true)
                  .Where(u => !u.UserId.Contains(UnitTestToken)).ToList().Take(10);

            if(!adminUserList.Any()) Assert.Inconclusive("There are no  admin to test");



            foreach(var sysUser in adminUserList)
                {
                var roles = roleProvider.GetRolesForUser(sysUser.UserId);
                Assert.IsTrue(roles.Contains(Enums.UserRoleValues.Administrator), "roles.Contains(Enums.UserRoleValues.Administrator)");
                }



            var powerUserlist =
              userRepo.GetSysUsersByRole(Enums.UserRole.PowerUser, true)
                 .Where(u => !u.UserId.Contains(UnitTestToken)).ToList().Take(10);

            if(!powerUserlist.Any()) Assert.Inconclusive("There are no  powerUserlist to test");



            foreach(var sysUser in powerUserlist)
                {
                var roles = roleProvider.GetRolesForUser(sysUser.UserId);
                Assert.IsTrue(roles.Contains(Enums.UserRoleValues.PowerUser), "roles.Contains(Enums.UserRoleValues.PowerUser)");
                }



            var directorateUserlist =
          userRepo.GetSysUsersByRole(Enums.UserRole.DirectorateDataEntry, true)
             .Where(u => !u.UserId.Contains(UnitTestToken)).ToList().Take(10);

            if(!directorateUserlist.Any()) Assert.Inconclusive("There are no DirectorateDataEntry to test");



            foreach(var sysUser in directorateUserlist)
                {
                var roles = roleProvider.GetRolesForUser(sysUser.UserId);
                Assert.IsTrue(roles.Contains(Enums.UserRoleValues.DirectorateDataEntry), "roles.Contains(Enums.UserRoleValues.DirectorateDataEntry)");
                }

            }

        [TestCleanup]
        public void CleanUp()
            {
            CleanUnitTestData();
            }


        }

    }

