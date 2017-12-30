

using System;
using System.Collections.Generic;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Core.VleWsUserInformation;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Location = Doe.Ls.ProjectTemplate.Data.Location;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class LocationRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.LocationRepository();
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
            var rep = globalServiceRepository.LocationRepository();
            
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");

            
            var directorate = globalServiceRepository.DirectorateRepository().BasicList()
                .OrderByDescending(d => d.DirectorateId).FirstOrDefault();
            var directorateId = directorate.DirectorateId;
            var location = new Data.Location
            {
                Name = "LocationTest_" + UnitTestToken,
                DirectorateId = directorateId,
                CreatedBy = UnitTestToken,
                CreatedDate = DateTime.Now,
                LastModifiedBy = UnitTestToken,
                LastModifiedDate = DateTime.Now
            };

            rep.Insert(location);
        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LocationRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LocationRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LocationRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LocationRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }


    [TestMethod]
    public void TestLocationDistinct()
    {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LocationRepository();

            var distinctList = rep.DistinctList();

        Console.WriteLine($"Total count = {distinctList.Count()}.");
    }

    [TestMethod]
    public void TestSplitString()
    {
        string inputString = "24|Bridget st.";
        string[] delimiter = {"|"};

        string[] words = inputString.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
        
        Assert.AreEqual("24", words[0]);
        Assert.AreEqual("Bridget st.", words[1]);
     }

    [TestMethod]
    public void TestMultipleLocations()
    {
        var factory = new MockRepositoryFactory();
        factory.RegisterAllDependencies();

        var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LocationRepository();
            var repDir = globalServiceRepository.DirectorateRepository();

        string selectedLocations = "Albury,Armidale,Bankstown";
        string[] delimiter1 = {","};
       
        
        string[] sectors= selectedLocations.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries);
          
        foreach (var s in sectors)
        {
                
            Console.WriteLine($"name={s}");
        }
    }

    

    [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

