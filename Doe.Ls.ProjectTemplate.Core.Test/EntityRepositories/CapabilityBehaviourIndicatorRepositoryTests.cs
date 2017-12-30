

using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class CapabilityBehaviourIndicatorRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.CapabilityBehaviourIndicatorRepository();
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
            var rep = globalServiceRepository.CapabilityBehaviourIndicatorRepository();
            var capBehaviourInd = GetCapabilityBehaviourIndicator();
            rep.Insert(capBehaviourInd);
            var inserted = rep.List().FirstOrDefault(l => l.IndicatorContext.Contains(UnitTestToken));
            Assert.IsFalse(inserted == null, "Capability name insert failed");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EditLevelId()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityBehaviourIndicatorRepository();
            var capBehaviourInd = GetCapabilityBehaviourIndicator();
            rep.Insert(capBehaviourInd);
            var inserted = rep.List().FirstOrDefault(l => l.IndicatorContext.Contains(UnitTestToken));
            Assert.IsFalse(inserted == null, "Capability name insert failed");
        var clevel = GetCapabilityLevel();
            globalServiceRepository.CapabilityLevelRepository().Insert(clevel);
            inserted.CapabilityLevelId = clevel.CapabilityLevelId;
            globalServiceRepository.CapabilityBehaviourIndicatorRepository().Update(inserted);

        }

        [TestMethod]
        
        public void EditIndContext()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityBehaviourIndicatorRepository();
            var capBehaviourInd = GetCapabilityBehaviourIndicator();
            rep.Insert(capBehaviourInd);
            var inserted = rep.List().FirstOrDefault(l => l.IndicatorContext.Contains(UnitTestToken));
            Assert.IsFalse(inserted == null, "Capability name insert failed");
           
          
            inserted.IndicatorContext = UnitTestToken+" Updated";
           rep.Update(inserted);
            Assert.IsFalse(!rep.List().Any(l=>l.IndicatorContext.Contains("updated")),"Update ind context failed");

        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityBehaviourIndicatorRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityBehaviourIndicatorRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.CapabilityBehaviourIndicatorRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

      [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

