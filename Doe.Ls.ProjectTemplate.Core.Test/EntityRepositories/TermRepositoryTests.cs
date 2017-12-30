

using System;
using System.Linq;
using Doe.Ls.SchoolSportsUnit.Data;
using Doe.Ls.SchoolSportsUnit.Core.BL;
using Doe.Ls.SchoolSportsUnit.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.SchoolSportsUnit.Core.Test.EntityRepositories
{
    [TestClass]
    public class TermRepositoryTests : TestBase
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
            var rep = globalServiceRepository.TermRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var model in rep.List().Take(10))
            {
                Console.WriteLine("{0}", model.ToString());
            }
        }
        [TestMethod]
        public void Create()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.TermRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Edit()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.TermRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.TermRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.TermRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.TermRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void GetByDate()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.TermRepository();
            var term = rep.GetTermByDate(new DateTime(2015, 3, 15));
            Assert.AreEqual(10, term.TermId);
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }


    }

}

