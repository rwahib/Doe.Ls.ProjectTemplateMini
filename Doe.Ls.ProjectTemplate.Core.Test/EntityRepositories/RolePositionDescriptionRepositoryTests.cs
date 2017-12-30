

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories
    {
    [TestClass]
    public class RolePositionDescriptionRepositoryTests : TestBase
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
            var rep = globalServiceRepository.RolePositionDescriptionRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach(var model in rep.List().Take(10))
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
            var rep = globalServiceRepository.RolePositionDescriptionRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }


        [TestMethod]
        public void MovePositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rpdService = srv.RolePositionDescriptionRepository();

            var sourceRpd = rpdService.ListForPositionDescriptions().Include(rpd=> rpd.Positions).FirstOrDefault(rpd => rpd.Positions.Any());
            if (sourceRpd == null) Assert.Inconclusive("No data to run the test");

            var sourceCount = sourceRpd.Positions.Count;

            var targetRpd =
                rpdService.ListForPositionDescriptions().Include(rpd => rpd.Positions)
                    .FirstOrDefault(rpd => rpd.RolePositionDescId != sourceRpd.RolePositionDescId &&
                                           rpd.Positions.Any());
            
            if (targetRpd == null) Assert.Inconclusive("No data to run the test");
            var targetCount1 = targetRpd.Positions.Count;
            var model = new RpdPositionsModel
            {
                RolePositionDescId = targetRpd.RolePositionDescId,
                PositionIds = sourceRpd.Positions.Take(2).Select(rpd => rpd.PositionId).ToList()
            };

            rpdService.MovePositions(model);

            var sourceRpd2 = rpdService.GetRolePositionDescById(sourceRpd.RolePositionDescId);

            rpdService.LoadNavigationProperty(sourceRpd2,rpd=>rpd.Positions);

            var targetRpd2 = rpdService.GetRolePositionDescById(targetRpd.RolePositionDescId);
            rpdService.LoadNavigationProperty(targetRpd2, rpd => rpd.Positions);

            Assert.IsTrue(targetCount1+model.PositionIds.Count==targetRpd2.Positions.Count,"targetCount1+model.PositionIds.Count==targetRpd2.Positions.Count");
            Assert.IsTrue(sourceCount-model.PositionIds.Count==sourceRpd2.Positions.Count,"sourceCount-model.PositionIds.Count==sourceRpd2.Positions.Count");



            model.RolePositionDescId = sourceRpd.RolePositionDescId;
            rpdService.MovePositions(model);

            }




        [TestMethod]
        public void Edit()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }

        [TestMethod]
        public void Delete()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }

        [TestMethod]
        public void Detail()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }


        [TestMethod]
        public void UpdateRolePosDescriptionTitleCascade()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.RolePositionDescriptionRepository();
            var repPos = srv.PositionRepository();

            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            var samples = rep.ListForRoleDescriptions()
                .Include(rd => rd.Positions)
                .Where(rd => rd.Positions.Any(p => p.StatusId != (int)Enums.StatusValue.Deleted))
                .Skip(5)
                .Take(2)
                .ToList();
            foreach(var rolePositionDescription in samples)
                {
                var oldTitle = rolePositionDescription.Title;
                var newTitle = rolePositionDescription.Title+ "_new";
                 var  rpd = rep.GetRolePositionDescById(rolePositionDescription.RolePositionDescId);

                rep.UpdateRolePosDescriptionTitleCascade(rpd.DocNumber, newTitle);
                    
                rep.Refresh(rpd);
                repPos.Refresh(rpd.Positions);

                Assert.IsTrue(rpd.Title == newTitle, "rp.Title==newTitle");
                foreach(var position in rpd.Positions)
                    {
                    Assert.IsTrue(position.PositionTitle == newTitle, "position.PositionTitle == newTitle");
                    }


                rep.UpdateRolePosDescriptionTitleCascade(rpd.DocNumber, oldTitle);
                    rpd = rep.GetRolePositionDescById(rolePositionDescription.RolePositionDescId);
                    rep.Refresh(rpd);
                    repPos.Refresh(rpd.Positions);

                Assert.IsTrue(rpd.Title == oldTitle, "rp.Title == oldTitle");
                }




            }
        [TestMethod]
        public void Search()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionRepository();
            //if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }

        [TestMethod]
        public void TestListForPositionDesc()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionRepository();

            var list = rep.ListForPositionDescriptions();

            foreach(var rd in list.Where(r => r.IsPositionDescription).Take(5).ToArray())
                {
                Console.WriteLine(rd.PositionDescription.PositionFocusCriterias.Count);
                }
            }

        [TestMethod]
        public void TestListForRoleDesc()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionRepository();

            var list = rep.FilterForLiveRolePositionDescriptions(rep.ListForRoleDescriptions());

            foreach(var rd in list.Take(5).ToArray())
                {

                Console.WriteLine(rd.RoleDescription.RoleCapabilities.Count);
                }
            }

        [TestMethod]
        public void TestEnumGetName()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var svr = new ServiceRepository(factory);
            var rep = svr.RolePositionDescriptionRepository();

            var rpd = rep.BaseList().OrderByDescending(r => r.RolePositionDescId).FirstOrDefault();

            var statusName = Enum.GetName(typeof(Enums.StatusValue), rpd.StatusId);

            Console.WriteLine("Status is: " + statusName);
            }

        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();
            }


        }

    }

