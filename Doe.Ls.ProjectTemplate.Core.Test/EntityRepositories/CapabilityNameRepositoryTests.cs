

using System;
using System.Linq;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class CapabilityNameRepositoryTests : TestBase {
	
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
            var rep = globalServiceRepository.CapabilityNameRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");

            foreach (var model in rep.List().Take(10)) {
                Console.WriteLine("{0}", model.ToString());
            }
        }
        [TestMethod]
        public void Create()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityNameRepository();
            var grprep = globalServiceRepository.CapabilityGroupRepository();
            var cname = GetCapabilityName();
            var cgrp = GetCapabilityGroup();
            grprep.Insert(cgrp);
            cname.CapabilityGroupId = cgrp.CapabilityGroupId;
            var insertedGrp = grprep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken));
            Assert.IsFalse(insertedGrp == null, "Capability group insert failed");
            rep.Insert(cname);
            var inserted = rep.List().FirstOrDefault(l => l.Name.Contains(UnitTestToken));
            Assert.IsFalse(inserted == null, "Capability name insert failed");
           
        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityNameRepository();
            var grprep = globalServiceRepository.CapabilityGroupRepository();

            var cname = GetCapabilityName();
            var cgrp = GetCapabilityGroup();
            grprep.Insert(cgrp);
            cname.CapabilityGroupId = cgrp.CapabilityGroupId;
            var insertedGrp = grprep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken));
            Assert.IsFalse(insertedGrp == null, "Capability group insert failed");
            rep.Insert(cname);
            var inserted = rep.List().FirstOrDefault(l => l.Name.Contains(UnitTestToken));
            Assert.IsFalse(inserted == null, "Capability name insert failed");

            var cgrp1 = GetCapabilityGroup();
            grprep.Insert(cgrp1);
            inserted.CapabilityGroupId = cgrp1.CapabilityGroupId;
            rep.Update(inserted);
            var updated = rep.List().FirstOrDefault(l => l.Name.Contains(UnitTestToken));
            Assert.IsFalse(updated == null, "Capability name update failed");
            Assert.IsFalse(updated.CapabilityGroupId != cgrp1.CapabilityGroupId, "Capability name update failed");

        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityNameRepository();
            var grprep = globalServiceRepository.CapabilityGroupRepository();

            var cname = GetCapabilityName("XXX");
            var cgrp = GetCapabilityGroup();
            grprep.Insert(cgrp);
            cname.CapabilityGroupId = cgrp.CapabilityGroupId;
            var insertedGrp = grprep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken));
            Assert.IsFalse(insertedGrp == null, "Capability group insert failed");
            
            rep.Insert(cname);
            var inserted = rep.List().FirstOrDefault(l => l.Name.Contains(UnitTestToken + "XXX"));
            Assert.IsFalse(inserted == null, "Capability name insert failed");
            rep.Delete(inserted);
            Assert.IsFalse(rep.List().Any(l => l.Name.Contains(UnitTestToken + "XXX")), "Delete Capability name failed");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityNameRepository();
            var grprep = globalServiceRepository.CapabilityGroupRepository();
            var cname = GetCapabilityName();
            var cgrp = GetCapabilityGroup();
            grprep.Insert(cgrp);
            cname.CapabilityGroupId = cgrp.CapabilityGroupId;
            var insertedGrp = grprep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken));
            Assert.IsFalse(insertedGrp == null, "Capability group insert failed");
            rep.Insert(cname);
            var inserted = rep.List().FirstOrDefault(l => l.Name.Contains(UnitTestToken));
            Assert.IsFalse(inserted == null, "Capability name insert failed");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityNameRepository();
            var grprep = globalServiceRepository.CapabilityGroupRepository();
            var cname = GetCapabilityName();
            var cgrp = GetCapabilityGroup();
            grprep.Insert(cgrp);
            cname.CapabilityGroupId = cgrp.CapabilityGroupId;
            var insertedGrp = grprep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken));
            Assert.IsFalse(insertedGrp == null, "Capability group insert failed");
            rep.Insert(cname);
            var inserted = rep.List().FirstOrDefault(l => l.Name.Contains(UnitTestToken));
            Assert.IsFalse(inserted == null, "Capability name insert failed");
            //search on capability name
            var arg = new SearchArg()
            {
                Search = cname.Name
            };
            var result = rep.FilterCapabilityNames(rep.List(), arg);

            Assert.IsFalse(!rep.List().Any(), "Search Failed");
            Assert.IsFalse(!result.Any(l => l.Name.Contains(UnitTestToken)), "Capability name Search failed");

            //search on capability group name
            var cname2 = GetCapabilityName();
            var cgrp2 = GetCapabilityGroup();
            grprep.Insert(cgrp2);
            cname2.CapabilityGroupId = cgrp2.CapabilityGroupId;
            var insertedGrp2 = grprep.List().FirstOrDefault(l => l.GroupName.Contains(UnitTestToken));
            Assert.IsFalse(insertedGrp2 == null, "Capability group insert failed");
            rep.Insert(cname2);
            var inserted2 = rep.List().FirstOrDefault(l => l.Name.Contains(UnitTestToken));
            Assert.IsFalse(inserted2 == null, "Capability name insert failed");
            var arg2 = new SearchArg()
            {
                Search = cgrp2.GroupName
            };
            var result2 = rep.FilterCapabilityNames(rep.List(), arg);

            Assert.IsFalse(!rep.List().Any(), "Search Failed");
            Assert.IsFalse(!result2.Any(l => l.CapabilityGroup.GroupName.Contains(cgrp2.GroupName)), "Capability name, group name Search failed");
        }


    [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

