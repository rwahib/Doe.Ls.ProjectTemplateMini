

using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories
{
    [TestClass]
    public class AppEntityTypeRepositoryTests : TestBase
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
            var rep = globalServiceRepository.AppEntityTypeRepository();
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
            var rep = globalServiceRepository.AppEntityTypeRepository();

            var number = GetRandom(10000);
            CreateEntity(number);

            var name = UnitTestToken + number;

            var justCreated = rep.List().FirstOrDefault(a => a.EntityTypeId == number);

            Assert.AreEqual(name, justCreated.EntityTitle);

        }
        [TestMethod]
        public void Edit()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.AppEntityTypeRepository();

            var number = GetRandom(20000);

            CreateEntity(number);

            var justCreated = rep.List().FirstOrDefault(a => a.EntityTypeId == number);

            justCreated.EntityApiName = "abc";
            rep.Update(justCreated);
            Assert.AreEqual("abc", justCreated.EntityApiName);

        }
        [TestMethod]
        public void Delete()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.AppEntityTypeRepository();

            var number = GetRandom(30000);
            CreateEntity(number);

            var justCreated = rep.List().FirstOrDefault(a => a.EntityTypeId == number);

            rep.Delete(justCreated);

            var exists = rep.List().Any(a => a.EntityTypeId == justCreated.EntityTypeId);
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.AppEntityTypeRepository();

            var number = GetRandom(13000);
            var name = UnitTestToken + number;
            CreateEntity(number);
            var justCreated = rep.List().FirstOrDefault(a => a.EntityTypeId == number);

            Assert.AreEqual(name, justCreated.EntityTitle);
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.AppEntityTypeRepository();

            var number = GetRandom(16000);

            CreateEntity(number);
            var justCreated = rep.List().FirstOrDefault(a => a.EntityTypeId == number);

            Assert.AreEqual(number, justCreated.EntityTypeId);

        }


        private AppEntityType CreateEntity(int ramNum)
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.AppEntityTypeRepository();

            var number = ramNum;
            var name = UnitTestToken + number;
            var thisEntry = new AppEntityType
            {
                EntityTypeId = number,
                EntityApiName = name,
                EntityTitle = name,
                SysAdminDashboard = false,
                PowerUserDashboard = false,
                HighPriority = false

            };
            rep.Insert(thisEntry);

            return thisEntry;

        }


        [TestCleanup]
        public void CleanUp()
        {
            CleanUnitTestData();
        }


    }

}

