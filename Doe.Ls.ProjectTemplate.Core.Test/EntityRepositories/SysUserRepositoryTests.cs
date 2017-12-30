

using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories
    {
    [TestClass]
    public class SysUserRepositoryTests : TestBase
        {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }

        [TestMethod]
        public void List()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach(var model in rep.List().Take(10))
                {
                Console.WriteLine("{0}", model.ToString());
                }
            }

        [TestMethod]
        [Ignore]
        public void Samplpleusers()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.SysUserRepository();

            var user = rep.GetSysUserByUserName("jonathan.russell7");
            var userInfo = UserInfoExtension.MapSysUser(user, factory);
            foreach (var activeRole in userInfo.ActiveRoleOrgLevelList)
            {
                Console.WriteLine(userInfo.CurrentRole);
                Console.WriteLine(activeRole);
            }


       var result=     rep.FilterSysUsers(rep.List(), new JQueryDataTableParamModel
            {
                sSearch = "jonathan.russell7"
                });

            foreach (var sysUser in result)
            {
                 userInfo = UserInfoExtension.MapSysUser(user, factory);
                Console.WriteLine(userInfo.CurrentRole);
                foreach(var activeRole in userInfo.ActiveRoleOrgLevelList)
                    {
                    Console.WriteLine(activeRole);
                    }
                }


        }


        [TestMethod]
        public void Edit()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }
        [TestMethod]
        public void Delete()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }

        [TestMethod]
        public void Detail()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }

        [TestMethod]
        public void Search()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }



        [TestMethod]
        public void TestEnumsOrgLevel()
        {
            var orgLevelId = Enums.OrgLevel.NA;

            var businessUnit = "BusinessUnit";

            Console.WriteLine("NA-->" + orgLevelId);

            foreach (var value in Enum.GetValues(typeof(Enums.OrgLevel)))
            {
                Console.WriteLine("{0} - {1}", (int) value, (Enums.OrgLevel) value);

                if (businessUnit == ((Enums.OrgLevel) value).ToString())
                {
                    Console.WriteLine("Found the key:" + (int)value);
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

