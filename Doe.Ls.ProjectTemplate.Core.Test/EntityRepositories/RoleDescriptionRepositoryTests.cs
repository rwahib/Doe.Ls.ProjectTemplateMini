

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories
{
    [TestClass]
    public class RoleDescriptionRepositoryTests : TestBase
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
            var rep = globalServiceRepository.RoleDescriptionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var model in rep.List().Take(10))
            {
                Console.WriteLine("{0}", model.ToString());
            }
        }

        [TestMethod]
        [Ignore]
        public void GetById()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleDescriptionRepository();
            var result = rep.GetRoleDescriptionById(697);
            Assert.IsNotNull(result,"result != null");

            }


        [TestMethod]
        public void Create()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleDescriptionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Edit()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleDescriptionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleDescriptionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleDescriptionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleDescriptionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void TestCreateRoleDesc()
        {

            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleDescriptionRepository();
            var repContent = globalServiceRepository.GlobalItemRepository();
            var rolePosRep = globalServiceRepository.RolePositionDescriptionRepository();

            var rolepositionDesc = new RolePositionDescription();
            var roleDescription = new RoleDescription();


            try
            {
                rolepositionDesc.RolePositionDescId = 700;
                rolepositionDesc.StatusId = 0;
                rolepositionDesc.Version = 1; // must be 1 or more
                rolepositionDesc.Title = "RoleDesc_Test_" + UnitTestToken;
                rolepositionDesc.DocNumber = "DOC17/1111111";
                rolepositionDesc.GradeCode = "CL1/2";
                rolepositionDesc.IsPositionDescription = false;
                rolepositionDesc.CreatedDate = DateTime.Now;
                rolepositionDesc.LastModifiedDate = DateTime.Now;
                rolepositionDesc.CreatedBy = "mary test";
                rolepositionDesc.LastModifiedBy = "mary test";
                rolepositionDesc.DateOfApproval = null;
                rolepositionDesc.StatusId = (int)Enums.StatusValue.Draft;

                rolePosRep.Insert(rolepositionDesc);
            }
            catch (Exception ex)
            {
                var msg = ex.Message + ex.StackTrace;
            }
            //Role Desc
            roleDescription.RoleDescriptionId = rolepositionDesc.RolePositionDescId;
            roleDescription.Agency = UnitTestToken;

            var globalContent = repContent.List().ToList();
            var agencyOverview = globalContent.FirstOrDefault(i => i.ItemCode == Enums.GlobalItem.AgencyOverview);
            roleDescription.AgencyOverview = agencyOverview.ItemContent;
            roleDescription.Cluster = globalContent.FirstOrDefault(i => i.ItemCode == Enums.GlobalItem.Cluster).ItemContent;
            roleDescription.AgencyWebsite = globalContent.FirstOrDefault(i => i.ItemCode == Enums.GlobalItem.AgencyWebsite).ItemContent;
            //roleDescription.DivisionOverview =globalContent.FirstOrDefault(i => i.ItemCode == Enums.GlobalItem.DivisionOverview).ItemContent;

            roleDescription.EssentialRequirements =
                globalContent.FirstOrDefault(i => i.ItemCode == Enums.GlobalItem.GeneralEssentialRequirement).ItemContent;
            roleDescription.RoleCapabilityItems =
                globalContent.FirstOrDefault(i => i.ItemCode == Enums.GlobalItem.CapabilitiesForTheRole).ItemContent;
            roleDescription.CapabilitySummary =
                globalContent.FirstOrDefault(i => i.ItemCode == Enums.GlobalItem.CapabilitySummary).ItemContent;
            roleDescription.FocusCapabilities =
                globalContent.FirstOrDefault(i => i.ItemCode == Enums.GlobalItem.FocusCapabilitiesForTheRole).ItemContent;

            roleDescription.VersionStatus = "Current";


            rep.Insert(roleDescription);

        }


        [TestMethod]
        public void TestCapabilitiesByRoldDescId()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RoleDescriptionRepository();
            var repCap = globalServiceRepository.RoleCapabilityRepository();


            var roleDescId = rep.ListForCapabilityFramework().Where(r =>
                    r.RoleCapabilities.Any() && r.RolePositionDescription.StatusId == (int)Enums.StatusValue.Imported).OrderBy(r => r.RoleDescriptionId)
                .Skip(10).First().RoleDescriptionId;


            var rdWithCapabilities = rep.LoadRoleDescWithCapabilityFramework(roleDescId);

            var listOfCapabilities = repCap.List().Where(c => c.RoleDescriptionId == roleDescId);

            Assert.AreEqual(listOfCapabilities.Count(), rdWithCapabilities.RoleCapabilities.Count);

        }

        [TestMethod]
        public void TestLoadRoleDescWithKeyRelationships()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gSrv = new ServiceRepository(factory);
            var rep = gSrv.RoleDescriptionRepository();
            var repKey = gSrv.KeyRelationshipRepository();

            var roleDescId = rep.ListForkeyRelationships().Where(r =>
                    r.KeyRelationships.Any() && r.RolePositionDescription.StatusId == (int)Enums.StatusValue.Imported).OrderBy(r=>r.RoleDescriptionId)
                .Skip(10).First().RoleDescriptionId;

            var rd = rep.LoadRoleDescWithKeyRelationships(roleDescId);

            var keyR = repKey.List().Where(r => r.RoleDescriptionId == roleDescId).ToList();

            Assert.AreEqual(keyR.Count, rd.KeyRelationships.Count);

        }

        [TestMethod]
        public void TestInternalExternalKeyRelationships()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.RoleDescriptionRepository();
            var list = rep.List().Where(rd => rd.KeyRelationships.Any()).Take(5).ToList();
            if (!list.Any()) Assert.Inconclusive("Not enough data to test");

            foreach (var roleDescription in list)
            {
                Console.WriteLine("---Started----");
                Console.WriteLine(roleDescription);
                Console.WriteLine("-------");

                foreach (var internalKey in roleDescription.InternalKeyRelationships())
                {
                    
                    Console.WriteLine(internalKey);
                }
                Console.WriteLine("===================");
                foreach (var externalKey in roleDescription.ExternalKeyRelationships())
                {
                    Console.WriteLine(externalKey);
                }
            }



        }

        [Ignore]
        [TestMethod]
        public void TestCapabilityGroupOrder()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.RoleDescriptionRepository();

            var roleDescId = 508;
            var rd = rep.LoadRoleDescWithCapabilityFramework(roleDescId);

            var cg = RoleDescriptionExtensions.SortCapabilityGroup(rd);

            foreach (var g in cg)
            {
                
                Console.WriteLine(g.CapabilityName.CapabilityGroup + 
                    "-" + 
                    g.CapabilityName.CapabilityGroup.DisplayOrder +
                    "-" +
                    g.CapabilityName.Name);

            }
        }

        [TestMethod]
        public void TestBulletPointsCount()
        {
            string data = "<li>List 1</li><li>List 2</li><li>List 3</li><li>List 4</li><li>List 5</li>";

            int count = Regex.Matches(data, "<li>").Cast<Match>().Count();

            Console.WriteLine(count);
           
        }

        [TestMethod]
        public void TestBulletPointsValidation()
        {
            string data = "<ul><li>List 1</li><li>List 2</li><li>List 3</li><li>List 4</li><li>List 5</li><li>List 5</li></ul>";

            var result = new Result();
            int min = 6;
            int max = 8;

           result = CommonHelper.ValidBulletPoints(data, min, max, "Key Accountabilities");
            Console.WriteLine(result.Message);
        }

        [TestMethod]
        public void TestNoBulletPointsFound()
        {
            string data = "<p>List 1</p><p>List 2</p><p>List 3</p><p>List 4</p><p>List 5</p><p>List 5</p>";
            var result = new Result();
            int min = 6;
            int max = 8;

            result = CommonHelper.ValidBulletPoints(data, min, max, "Key Accountabilities");
            Console.WriteLine(result.Message);
        }



        [TestMethod]
        public void TestEmptyString()
        {
            string data = "";
            var result = new Result();
            int min = 6;
            int max = 8;
            //empty string is OK
            result = CommonHelper.ValidBulletPoints(data, min, max, "Key Accountabilities");
            Console.WriteLine(result.Message);
        }


        [Ignore]
        [TestMethod]
        public void TestUpdateCapabitiltyNotManager()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.RoleDescriptionRepository();
            var repCapName = srv.CapabilityNameRepository();
           

            var rdId = 709;
            var isManagerRole = false;
            var model = rep.LoadRoleDescWithCapabilityFramework(rdId);
            model.RoleCapabilities = RoleDescriptionExtensions.SortCapabilityGroup(model).ToList();

            //If not manager role, People Management capabilities need to be removed
            var managementCap = repCapName.ListWithGroup()
                .Where(c=>c.CapabilityGroupId == (int)Enums.CapablityGroup.PeopleManagement)
                .Select(c=>c.CapabilityNameId).ToList();

            var tempList = model.RoleCapabilities;

            //Console.WriteLine("Original count of role capabilities = " + tempList.Count);
            
            var finalList = tempList.Where(x => !managementCap.Contains(x.CapabilityNameId)).ToList();
            
           // Console.WriteLine("Final count of role capabilities = " + finalList.Count);

            //do update
            model.RoleCapabilities = finalList;
            model.ManagerRole = isManagerRole;
            rep.Update(model);

            var updated = rep.LoadRoleDescWithCapabilityFramework(rdId);
            
            Assert.AreEqual(finalList.Count, updated.RoleCapabilities.Count);

        }


        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }


    }

}

