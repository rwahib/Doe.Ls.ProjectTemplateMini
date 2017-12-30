

using System;
using System.Linq;
using Doe.Ls.SchoolSportsUnit.Data;
using Doe.Ls.SchoolSportsUnit.Core.BL;
using Doe.Ls.SchoolSportsUnit.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.SchoolSportsUnit.Core.Test.EntityRepositories 
{
[TestClass]
    public class VenueRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.VenueRepository();
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
            var rep = srv.VenueRepository();

            var venue = new Venue
            {
                AdditionalInformation = UnitTestToken,
                State = "NSW",
                Suburb = UnitTestToken,
                VenueName = $"venue name {UnitTestToken}"

            };

            rep.Insert(venue);
            Assert.IsTrue(venue.VenueId>0,"venue.VenueId>0");
            Assert.IsTrue(venue.VenueName== $"venue name {UnitTestToken}","venue.VenueName== $'venue name {UnitTestToken}'");
            

        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.VenueRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.VenueRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.VenueRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.VenueRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

      [ClassCleanup]
        public static void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

