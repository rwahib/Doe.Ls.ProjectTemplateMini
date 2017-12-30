

using System;
using System.Linq;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class SelectionCriteriaRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.SelectionCriteriaRepository();
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
            var rep = srv.SelectionCriteriaRepository();
            var criteria = GetCriteria("insert");
            rep.Insert(criteria);
            Assert.IsFalse(!rep.List().Any(l => l.Criteria.Contains(UnitTestToken+"insert")), "insert failed");
        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SelectionCriteriaRepository();
            var criteria = GetCriteria( "insert");
            rep.Insert(criteria);
            Assert.IsFalse(!rep.List().Any(l => l.Criteria.Contains(UnitTestToken + "insert")), "insert failed");
            var insertedObj = rep.List().FirstOrDefault(l => l.Criteria.Contains(UnitTestToken + "insert"));
            insertedObj.Criteria = UnitTestToken+"update";
            rep.Update(insertedObj);
            Assert.IsFalse(!rep.List().Any(l => l.Criteria.Contains(UnitTestToken+"update")), "update failed");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SelectionCriteriaRepository();
            var criteria = GetCriteria("delete");
            rep.Insert(criteria);
            Assert.IsFalse(!rep.List().Any(l => l.Criteria.Contains(UnitTestToken + "delete")), "insert failed");
            var insertedObj = rep.List().FirstOrDefault(l => l.Criteria.Contains(UnitTestToken + "delete"));
            
            rep.Delete(insertedObj);
            Assert.IsFalse(rep.List().Any(l => l.Criteria.Contains(UnitTestToken + "delete")), "delete failed");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SelectionCriteriaRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SelectionCriteriaRepository();
            var criteria = GetCriteria("search");
            rep.Insert(criteria);
            Assert.IsFalse(!rep.List().Any(l => l.Criteria.Contains(UnitTestToken + "search")), "insert failed");
            var insertedObj = rep.List().FirstOrDefault(l => l.Criteria.Contains(UnitTestToken + "search"));
            var arg = new SearchArg()
            {
                Search = UnitTestToken+"search"
            };
            var fList = rep.FilterSelectionCriterias(rep.List(), arg);
        Assert.IsFalse(fList==null|| !fList.Any(),"Search failed");

        }

      [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

