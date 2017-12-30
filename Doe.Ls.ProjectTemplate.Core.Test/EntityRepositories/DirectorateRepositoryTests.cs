

using System;
using System.Collections.Generic;
using System.Linq;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories
{
    [TestClass]
    public class DirectorateRepositoryTests : TestBase
    {


        [ClassInitialize]
        public static void Initialise(TestContext testContext) {
        }

        [TestMethod]
        public void List() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.DirectorateRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var model in rep.List().Take(10)) {
                Console.WriteLine("{0}", model.ToString());
            }
        }
        [TestMethod]
        public void Create() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.DirectorateRepository();

            var number = GetRandom(100);
            CreateEntity(number);

            var name = UnitTestToken + number;

            var justCreated = rep.List().FirstOrDefault(a => a.DirectorateId == number);

            Assert.AreEqual(name, justCreated.DirectorateName);
        }
        [TestMethod]
        public void Edit() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.DirectorateRepository();
            var number = GetRandom(100);

            CreateEntity(number);

            var justCreated = rep.List().FirstOrDefault(a => a.DirectorateId == number);

            justCreated.DirectorateDescription = "My test directorate";
            rep.Update(justCreated);
            Assert.AreEqual("My test directorate", justCreated.DirectorateDescription);


        }
        [TestMethod]
        public void Delete() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.DirectorateRepository();

            var number = GetRandom(100);
            var name = UnitTestToken + number;
            CreateEntity(number);


            var justCreated = rep.List().FirstOrDefault(a => a.DirectorateId == number);

            rep.Delete(justCreated);

            var exists = rep.List().Any(a => a.DirectorateId == number);
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void DeleteWithLocation() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.DirectorateRepository();

            var number = GetRandom(100);

            CreateEntity(number);

            var justCreated = rep.List().First(a => a.DirectorateId == number);

            var max = srv.LocationRepository().GetMaxKeyValue();


            justCreated.Locations.Add(new Location { Name = "AAAA", LocationId = max + 1, DirectorateId = justCreated.DirectorateId, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now, CreatedBy = "System", LastModifiedBy = "System" });
            justCreated.Locations.Add(new Location { Name = "BBB", LocationId = max + 2, DirectorateId = justCreated.DirectorateId, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now, CreatedBy = "System", LastModifiedBy = "System" });

            rep.SaveChanges();

            Assert.IsTrue(justCreated.Locations.Count == 2, "justCreated.Locations.Count==2");

            justCreated = rep.List().FirstOrDefault(a => a.DirectorateId == number);


            rep.Delete(justCreated);

            var exists = rep.List().Any(a => a.DirectorateId == number);
            Assert.IsFalse(exists);
        }



        [TestMethod]
        [Ignore]
        public void DeleteWithLocationToIgnore() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.DirectorateRepository();

            var justCreated = rep.List().FirstOrDefault(a => a.DirectorateId == 25342);



            rep.Delete(justCreated);

            var exists = rep.List().Any(a => a.DirectorateId == 25342);
            Assert.IsFalse(exists, "exists");

        }

        [TestMethod]
        public void Detail() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.DirectorateRepository();

            var number = GetRandom(100);
            var name = UnitTestToken + number;
            CreateEntity(number);
            var justCreated = rep.List().FirstOrDefault(a => a.DirectorateId == number);

            //Assert.AreEqual(name, justCreated.DirectorateName);
        }

        [TestMethod]
        public void Search() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.DirectorateRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        private Directorate CreateEntity(int ramNum) {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.DirectorateRepository();

            var number = ramNum; //use a number >5 digits
            var name = UnitTestToken + number;
            var thisEntry = new Directorate {
                DirectorateId = number,
                ExecutiveCod = "TL",
                DirectorateName = name,
                DirectorateDescription = name,
                DirectorateOverview = "For RD only",
                DirectorateCustomClass = "#fff",
                StatusId = (int)Enums.StatusValue.Draft,
                CreatedBy = name,
                CreatedDate = DateTime.Now,
                LastModifiedBy = name,
                LastModifiedDate = DateTime.Now

            };
            rep.Insert(thisEntry);

            return thisEntry;

        }



        [TestMethod]
        public void TestCreateDirectorateWithmultipleLocations() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LocationRepository();
            var repDir = globalServiceRepository.DirectorateRepository();

            string selectedLocations = "Albury,Armidale,Bankstown";
            string[] delimiter1 = { "," };


            string[] sectors = selectedLocations.Split(delimiter1, StringSplitOptions.RemoveEmptyEntries);

            var directorateId = repDir.GetMaxKeyValue() + 10;

            var directorate = new Directorate {
                DirectorateId = directorateId,
                ExecutiveCod = "TL",
                DirectorateName = UnitTestToken,
                StatusId = (int)Enums.StatusValue.Draft,
                CreatedBy = UnitTestToken,
                LastModifiedBy = UnitTestToken,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            if (directorate != null) {
                List<Location> locationList = new List<Location>();

                foreach (var s in sectors) {
                    //string[] words = s.Split(delimiter2, StringSplitOptions.RemoveEmptyEntries);
                    Console.WriteLine($"name={s}");

                    var location = new Data.Location {
                        LocationId = rep.GetDbNewId("Location"),
                        Name = s,
                        DirectorateId = directorateId,
                        CreatedBy = UnitTestToken,
                        LastModifiedBy = UnitTestToken,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now
                    };

                    locationList.Add(location);
                }

                directorate.Locations = locationList;
                //add Directorate and its locations

                repDir.Insert(directorate);
                Console.WriteLine($"# of locations = " + directorate.Locations.Count);
            }

        }


        [TestCleanup]
        public void CleanUp() {
            TestBase.CleanUnitTestData();
        }


    }

}

