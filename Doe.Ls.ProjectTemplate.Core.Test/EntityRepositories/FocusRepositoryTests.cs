

using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class FocusRepositoryTests : TestBase {

        
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
            var rep = globalServiceRepository.FocusRepository();
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
            var rep = globalServiceRepository.FocusRepository();
            var focus = GetFocus();
            rep.Insert(focus);
            Assert.IsFalse(!rep.List().Any(l=>l.FocusName.Contains(UnitTestToken)),"insert fails");
        }
        [TestMethod]
        public void Edit()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.FocusRepository();
           
            var focus = GetFocus("insert");
            rep.Insert(focus);
            Assert.IsFalse(!rep.List().Any(l => l.FocusName.Contains(UnitTestToken+ "insert")), "insert failed");
            var insertedObj = rep.List().FirstOrDefault(l => l.FocusName.Contains(UnitTestToken));
            insertedObj.FocusName = UnitTestToken + "Edit";
            rep.Update(insertedObj);
            Assert.IsFalse(!rep.List().Any(l => l.FocusName.Contains(UnitTestToken + "Edit")), "update failed");


        }
        [TestMethod]
        public void Delete()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.FocusRepository();
            var focus = GetFocus("delete");
            rep.Insert(focus);
            Assert.IsFalse(!rep.List().Any(l => l.FocusName.Contains(UnitTestToken + "delete")), "insert failed");
            var insertedObj = rep.List().FirstOrDefault(l => l.FocusName.Contains(UnitTestToken + "delete"));
            rep.Delete(insertedObj);
            Assert.IsFalse(rep.List().Any(l => l.FocusName.Contains(UnitTestToken + "delete")), "delete failed");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.FocusRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.FocusRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

      [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

