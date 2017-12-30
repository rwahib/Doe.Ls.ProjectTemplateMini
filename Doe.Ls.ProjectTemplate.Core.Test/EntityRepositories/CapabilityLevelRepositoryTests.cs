
using System;
using System.Linq;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class CapabilityLevelRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.CapabilityLevelRepository();
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
            var rep = globalServiceRepository.CapabilityLevelRepository();
            var capLevel = GetCapabilityLevel();
            rep.Insert(capLevel);
            var inserted = rep.List().FirstOrDefault(l => l.LevelName.Contains(UnitTestToken));
            Assert.IsFalse(inserted == null, "Capability level insert failed");
        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityLevelRepository();
            var caplevel = GetCapabilityLevel();
            rep.Insert(caplevel);
            var inserted = rep.List().FirstOrDefault(l => l.LevelName.Contains(UnitTestToken));
            inserted.DisplayOrder = 50;
            rep.Update(inserted);
            var updated = rep.List().FirstOrDefault(l => l.LevelName.Contains(UnitTestToken));
            Assert.IsFalse(updated == null, "Capability level update failed");
            Assert.IsFalse(updated.DisplayOrder != 50, "Capability level update failed");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityLevelRepository();
            var caplevel = GetCapabilityLevel("XXX");
            
            rep.Insert(caplevel);
            var inserted = rep.List().FirstOrDefault(l => l.LevelName.Contains(UnitTestToken + "XXX"));

            rep.Delete(inserted);
            Assert.IsFalse(rep.List().Any(l => l.LevelName.Contains(UnitTestToken + "XXX")), "Delete Failed");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityLevelRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityLevelRepository();
            var caplevel = GetCapabilityLevel();
            rep.Insert(caplevel);
            var arg = new SearchArg()
            {
                Search = caplevel.LevelName
            };
            var result = rep.FilterCapabilityLevels(rep.List(), arg);

            Assert.IsFalse(!rep.List().Any(), "Search Failed");
            Assert.IsFalse(!rep.List().Any(l => l.LevelName.Contains(UnitTestToken)), "Search Failed");
        }



    [TestMethod]
    public void TestLoadCapabilityLevels()
    {
        var factory = new MockRepositoryFactory();
        factory.RegisterAllDependencies();

        var globalServiceRepository = new ServiceRepository(factory);
        var rep = globalServiceRepository.CapabilityLevelRepository();
        var repMatrix = globalServiceRepository.RoleDescCapabilityMatrixRepository();

        var gradeCode = "CL3/4";
        var matrix = repMatrix.List().SingleOrDefault(m => m.GradeCode == gradeCode);
        var list = rep.LoadCapabilityLevelsForRoleDesc(matrix);
        
        Assert.AreEqual(2, list.Count);

        gradeCode = "CL9/10";
        matrix = repMatrix.List().SingleOrDefault(m => m.GradeCode == gradeCode);
        list = rep.LoadCapabilityLevelsForRoleDesc(matrix);
        Assert.AreEqual(3, list.Count);
    }


    [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

