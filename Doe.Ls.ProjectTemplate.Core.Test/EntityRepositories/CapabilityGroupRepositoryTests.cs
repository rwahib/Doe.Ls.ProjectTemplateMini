

using System;
using System.Linq;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class CapabilityGroupRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.CapabilityGroupRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");

            foreach (var model in rep.List().Take(10))
            {
                Console.WriteLine("{model}");
            }
        }
        [TestMethod]
        public void Create()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityGroupRepository();
            var capGroup = GetCapabilityGroup();
            rep.Insert(capGroup);
            var inserted = rep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken));
            Assert.IsFalse(inserted==null,"Capability group insert failed");
            
        }

   

    [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityGroupRepository();
            var capGroup = GetCapabilityGroup();
            rep.Insert(capGroup);
            var inserted = rep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken));
            inserted.DisplayOrder = 50;
            rep.Update(inserted);
            var updated = rep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken));
            Assert.IsFalse(updated == null, "Capability group update failed");
            Assert.IsFalse(updated.DisplayOrder != 50, "Capability group update failed");

        }
        [TestMethod]
       
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityGroupRepository();
            var capGroup = GetCapabilityGroup("XXX");
            
            rep.Insert(capGroup);
            var inserted = rep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken+ "XXX"));
            rep.Delete(inserted);

            Assert.IsFalse(rep.List().Any(l => l.GroupName.Contains(UnitTestToken + "XXX")), "Delete Failed");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityGroupRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityGroupRepository();
            var capGroup = GetCapabilityGroup();
            rep.Insert(capGroup);
            var arg = new SearchArg()
            {
                Search = capGroup.GroupName
            };
            var result = rep.FilterCapabilityGroups(rep.List(), arg);

            Assert.IsFalse(!rep.List().Any(),"Search Failed");
            Assert.IsFalse(!result.Any(l=>l.GroupName.Contains(UnitTestToken)), "Search Failed");
        }

      [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

