

using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class KeyRelationshipRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.KeyRelationshipRepository();
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

            var srv = new ServiceRepository(factory);
            var rep = srv.KeyRelationshipRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");

            var roleDescId = srv.RoleDescriptionRepository().List().Skip(10).First().RoleDescriptionId;

            var countBefore = rep.List().Count(kr => kr.RoleDescriptionId == roleDescId);


            var keyrelationship = new KeyRelationship
            {
                ScopeId = 10,
                RoleDescriptionId = roleDescId,
                Who = "Test_" + UnitTestToken,
                Why = "Test again",
                DateCreated = DateTime.Now,
                LastUpdated = DateTime.Now,
                ModifiedUserName = "mw",
                OrderNumber = 1
            };
            
            rep.Insert(keyrelationship);
            var countAfter = rep.List().Count(kr => kr.RoleDescriptionId == roleDescId);
            Assert.IsTrue(countAfter==countBefore+1,"countAfter==countBefore+1");

            rep.Delete(keyrelationship);
            countAfter = rep.List().Count(kr => kr.RoleDescriptionId == roleDescId);
            Assert.IsTrue(countAfter == countBefore , "countAfter == countBefore");

        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.KeyRelationshipRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.KeyRelationshipRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.KeyRelationshipRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.KeyRelationshipRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

    [TestMethod]
    public void TestScopeNames()
    {
        var scopeType = Enums.ScopeType.Ministerial;

        var name = Enum.GetName(typeof(Enums.ScopeType), Enums.ScopeType.Ministerial);
        Console.WriteLine(name);

        var name2 = Enum.GetName(typeof(Enums.ScopeType), Enums.ScopeType.Internal);
        Console.WriteLine(name2);

        var name3 = Enum.GetName(typeof(Enums.ScopeType), Enums.ScopeType.External);
        Console.WriteLine(name3);
    }

    [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

