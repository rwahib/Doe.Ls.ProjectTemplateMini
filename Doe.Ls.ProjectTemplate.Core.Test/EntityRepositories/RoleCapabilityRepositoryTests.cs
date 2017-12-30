

using System;
using System.Linq;
using System.Web.Mvc;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories
{
    [TestClass]
    public class RoleCapabilityRepositoryTests : TestBase
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
            var rep = globalServiceRepository.RoleCapabilityRepository();
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
            var rep = globalServiceRepository.RoleCapabilityRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Edit()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleCapabilityRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleCapabilityRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleCapabilityRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleCapabilityRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }


        [TestMethod]
        public void TestParseCapabilities()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);

            var rep = srv.RoleCapabilityRepository();

            var roleDescRep= srv.RoleDescriptionRepository();

            var roleDescriptionId =
                roleDescRep.ActiveList().Where(rd => rd.RoleDescriptionId != -1).FirstOrDefault().RoleDescriptionId;
            var collection = new FormCollection
            {
                ["ManagerRole"] = "on",
                ["RoleDescriptionId"] = roleDescriptionId.ToString(),
                ["CapabilityLevelId_1"] = "4",
                ["CapabilityLevelId_2"] = "4",
                ["CapabilityLevelId_3"] = "4",
                ["CapabilityLevelId_4"] = "4",
                ["CapabilityLevelId_5"] = "4",
                ["CapabilityLevelId_6"] = "4",
                ["CapabilityLevelId_7"] = "4",
                ["CapabilityLevelId_8"] = "4",
                ["CapabilityLevelId_9"] = "2",
                ["CapabilityLevelId_10"] = "2",
                ["CapabilityLevelId_11"] = "2",
                ["CapabilityLevelId_12"] = "2",
                ["CapabilityLevelId_13"] = "2",
                ["Highlighted_1"] = "on",
                ["Highlighted_2"] = "on",
                ["Highlighted_3"] = "on",
                ["Highlighted_4"] = "on"
            };

            
            var model = new RoleCapabilityModel();
            var result = model.ParseBuildRoleCapabilityList(collection, srv);

            Assert.AreEqual(roleDescriptionId, result.RoleDescriptionId);
            Assert.AreEqual(true, result.IsManager);

            Assert.AreEqual(13, result.RoleCapabilities.Count);

            Assert.AreEqual(4, result.RoleCapabilities.Count(h=>h.Highlighted));
        }

        [Ignore]
        [TestMethod]
        public void TestSaveCapabilityList()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleCapabilityRepository();
           
            var collection = new FormCollection();

            collection["ManagerRole"] = "on";
            collection["RoleDescriptionId"] = "709";

            collection["CapabilityLevelId_1"] = "4";
            collection["CapabilityLevelId_2"] = "4";
            collection["CapabilityLevelId_3"] = "4";
            collection["CapabilityLevelId_4"] = "4";
            collection["CapabilityLevelId_5"] = "4";
            collection["CapabilityLevelId_6"] = "4";
            collection["CapabilityLevelId_7"] = "4";
            collection["CapabilityLevelId_8"] = "4";

            collection["CapabilityLevelId_9"] = "3";
            collection["CapabilityLevelId_10"] = "3";
            collection["CapabilityLevelId_11"] = "3";
            collection["CapabilityLevelId_12"] = "3";
            collection["CapabilityLevelId_13"] = "3";

            collection["Highlighted_1"] = "on";
            collection["Highlighted_2"] = "on";
            collection["Highlighted_3"] = "on";
            collection["Highlighted_4"] = "on";

            var model = new RoleCapabilityModel();
            var result = model.ParseBuildRoleCapabilityList(collection, globalServiceRepository);
           
            //Chaining into an entity, then save
          
            var rd =
                rep.RoleDescriptionRepository.ListForCapabilitiesOnly()
                    .FirstOrDefault(r => r.RoleDescriptionId == result.RoleDescriptionId);


            rd.RoleCapabilities = result.RoleCapabilities;
            
            rd.ManagerRole = result.IsManager;
            rep.RoleDescriptionRepository.Update(rd);

         

             var savedList = rep.List().Where(s => s.RoleDescriptionId == result.RoleDescriptionId);

            Assert.AreEqual(result.RoleCapabilities.Count, savedList.Count());

        }

        
        [Ignore]
        [TestMethod]
        public void TestValidateOnMatrix()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleCapabilityRepository();
            var repRd = globalServiceRepository.RolePositionDescriptionRepository();

            var repMatrix = globalServiceRepository.RoleDescCapabilityMatrixRepository();

            var roleDescId = 239;
            var capabilitiesLight = rep.GenerateCapabilityModel(roleDescId);

            var rd = repRd.List().FirstOrDefault(r => r.RolePositionDescId == roleDescId);

            var matrix = repMatrix.List().FirstOrDefault(m => m.GradeCode == rd.GradeCode);
            
            //1   Adept
            //2   Intermediate
            //3   Advanced
            //4   Foundational
            //5   Highly Advanced

            var roleCapabilities = rep.List().Where(c => c.RoleDescriptionId == roleDescId).ToList();

            var foundationalCnt = roleCapabilities.Count(c=>c.CapabilityLevelId == 4);
            var adeptCnt = roleCapabilities.Count(c => c.CapabilityLevelId == 1);
            var interMedCnt = roleCapabilities.Count(c => c.CapabilityLevelId == 2);
            var highAdvCnt = roleCapabilities.Count(c => c.CapabilityLevelId == 5);
            var advCnt = roleCapabilities.Count(c => c.CapabilityLevelId == 3);

            var fCount = 0;
            var adCount = 0;
            var haCount = 0;
            var iCount = 0;
            var aCount = 0;
            foreach (var cp in capabilitiesLight)
            {
                fCount = fCount + cp.CapabilityNames.Count(l => l.LevelId == 4);
                adCount = adCount + cp.CapabilityNames.Count(l => l.LevelId == 3);
                haCount = haCount + cp.CapabilityNames.Count(l => l.LevelId == 5);
                iCount = iCount + cp.CapabilityNames.Count(l => l.LevelId == 2);
                aCount = aCount + cp.CapabilityNames.Count(l => l.LevelId == 1);
            }

            Assert.AreEqual(foundationalCnt, fCount);
            Assert.AreEqual(adeptCnt, aCount);
            Assert.AreEqual(interMedCnt, iCount);
            Assert.AreEqual(highAdvCnt, haCount);
            Assert.AreEqual(advCnt, aCount);
        }





        [ClassCleanup]
        public static void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }


    }

}

