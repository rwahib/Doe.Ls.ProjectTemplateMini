

using System;
using System.Data.Entity;
using System.Linq;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class PositionInformationRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.PositionInformationRepository();
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
            var rep = globalServiceRepository.PositionInformationRepository();
            var position = GetPositionWithRpDesc(globalServiceRepository);
            var positionInfo = new PositionInformation()
            {
                PositionId = position.PositionId,
                PositionTypeCode = globalServiceRepository.PositionTypeRepository().List().FirstOrDefault().PositionTypeCode,
                EmployeeTypeCode = globalServiceRepository.EmployeeTypeRepository().List().FirstOrDefault().EmployeeTypeCode,
                PositionFTE = 1,
                PositionStatusCode = globalServiceRepository.PositionStatusValueRepository().List().FirstOrDefault().PosStatusCode,
                OccupationTypeCode = globalServiceRepository.OccupationTypeRepository().List().FirstOrDefault().OccupationTypeCode
                
            };
            rep.Insert(positionInfo);
            var insObj = rep.List().FirstOrDefault(l => l.Position.PositionTitle.Contains( UnitTestToken));
            Assert.IsFalse(insObj == null, "Position information insert failed");
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionInformationRepository();
            var position = GetPositionWithRpDesc(globalServiceRepository);
            var positionInfo = new PositionInformation()
            {
                PositionId = position.PositionId,
                PositionTypeCode = globalServiceRepository.PositionTypeRepository().List().FirstOrDefault().PositionTypeCode,
                EmployeeTypeCode = globalServiceRepository.EmployeeTypeRepository().List().FirstOrDefault().EmployeeTypeCode,
                PositionFTE = 1,
                PositionStatusCode = globalServiceRepository.PositionStatusValueRepository().List().FirstOrDefault().PosStatusCode,
                OccupationTypeCode = globalServiceRepository.OccupationTypeRepository().List().FirstOrDefault().OccupationTypeCode

            };
            rep.UpdateOrInsertPositionInformation(positionInfo,null);
            var insObj = rep.List().FirstOrDefault(l => l.Position.PositionTitle.Contains(UnitTestToken));
            Assert.IsFalse(insObj == null, "Position information insert failed");
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void EditPositionInfoWithNotes()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionInformationRepository();
            var position = GetPositionWithRpDesc(globalServiceRepository);
            var positionInfo = new PositionInformation()
            {
                PositionId = position.PositionId,
                PositionTypeCode = globalServiceRepository.PositionTypeRepository().List().FirstOrDefault().PositionTypeCode,
                EmployeeTypeCode = globalServiceRepository.EmployeeTypeRepository().List().FirstOrDefault().EmployeeTypeCode,
                PositionFTE = 1,
                PositionStatusCode = globalServiceRepository.PositionStatusValueRepository().List().FirstOrDefault().PosStatusCode,
                OccupationTypeCode = globalServiceRepository.OccupationTypeRepository().List().FirstOrDefault().OccupationTypeCode

            };
           
            rep.UpdateOrInsertPositionInformation(positionInfo, UnitTestToken);
            var insObj = rep.List().Include(l=>l.PositionNotes).FirstOrDefault(l => l.Position.PositionTitle.Contains(UnitTestToken));
            Assert.IsFalse(insObj == null, "Position information insert failed");
            Assert.IsFalse(!insObj.PositionNotes.Any(n=>n.Notes.Contains(UnitTestToken)), "Position notes insert failed");
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        [ExpectedException(typeof(Exception), "Position end date is mandatory for temporary position type")]
        public void TestEndDateValidation()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionInformationRepository();
            var position = GetPositionWithRpDesc(globalServiceRepository);
            var positionInfo = new PositionInformation()
            {
                PositionId = position.PositionId,
                PositionTypeCode = Enums.PositionType.T.ToString(),
                EmployeeTypeCode = globalServiceRepository.EmployeeTypeRepository().List().FirstOrDefault().EmployeeTypeCode,
                PositionFTE = 1,
                PositionStatusCode = globalServiceRepository.PositionStatusValueRepository().List().FirstOrDefault().PosStatusCode,
                OccupationTypeCode = globalServiceRepository.OccupationTypeRepository().List().FirstOrDefault().OccupationTypeCode

            };
            rep.UpdateOrInsertPositionInformation(positionInfo, null);
            var insObj = rep.List().FirstOrDefault(l => l.Position.PositionTitle.Contains(UnitTestToken));
            Assert.IsFalse(insObj == null, "Position information insert failed");
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionInformationRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionInformationRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionInformationRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

      [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

