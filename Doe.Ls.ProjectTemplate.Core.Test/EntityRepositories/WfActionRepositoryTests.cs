

using System;
using System.Linq;
using System.Web.Mvc;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class WfActionRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.WfActionRepository();
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
            var rep = globalServiceRepository.WfActionRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.WfActionRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.WfActionRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.WfActionRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.WfActionRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void TestCloneSelectedPosition()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gsr = new ServiceRepository(factory);
            var rep = gsr.PositionRepository();
           
            var collection = new FormCollection();

            collection["select"] = "0";
            collection["NewPositionNumber"] = "clone12345";

            var sourcePos = rep.ListForClonePosition()
                .Where(p => p.StatusId == (int)Enums.StatusValue.Approved || p.StatusId == (int)Enums.StatusValue.Imported)
                .OrderByDescending(p => p.PositionId).FirstOrDefault();
            //TODO - clone the whole lot of the selected position (except use the new position#), positionInfo, 
            //costCentre, RPD,RD/PD, PositionHistory

            var newPosition = new Position();

            var newPosInfo = new PositionInformation();
            var newCostCentre = new CostCentreDetail();

            if (sourcePos != null)
            {
                newPosition.PositionNumber = collection["NewPositionNumber"];
                newPosition.RolePositionDescriptionId = sourcePos.RolePositionDescriptionId;
                newPosition.ReportToPositionId = sourcePos.ReportToPositionId;
                newPosition.UnitId = sourcePos.UnitId;
                newPosition.PositionTitle = sourcePos.PositionTitle + UnitTestToken;
                newPosition.Description = sourcePos.Description;
                newPosition.PositionLevelId = sourcePos.PositionLevelId;
                newPosition.StatusId = (int)Enums.StatusValue.Draft;
                newPosition.PositionPath = sourcePos.PositionPath;
                newPosition.LocationId = sourcePos.LocationId;
                newPosition.CreatedDate = DateTime.Now;
                newPosition.LastModifiedDate = DateTime.Now;
                newPosition.CreatedBy = "CloneTest";
                newPosition.LastModifiedBy = "CloneTest";

                //create Position, then retrieve the PositionId to process PositionInfo
               
                newPosInfo.PositionId = newPosition.PositionId;
                newPosInfo.OccupationTypeCode = sourcePos.PositionInformation.OccupationTypeCode;
                newPosInfo.OlderPositionNumber1 = sourcePos.PositionInformation.OlderPositionNumber1;
                newPosInfo.OlderPositionNumber2 = sourcePos.PositionInformation.OlderPositionNumber2;
                newPosInfo.OlderPositionNumber3 = sourcePos.PositionInformation.OlderPositionNumber3;
                newPosInfo.SchNumber = sourcePos.PositionInformation.SchNumber;
                newPosInfo.PositionTypeCode = sourcePos.PositionInformation.PositionTypeCode;
                newPosInfo.EmployeeTypeCode = sourcePos.PositionInformation.EmployeeTypeCode;
                //newPosInfo.PositionNoteId = null;
                newPosInfo.TrimLink = string.Empty;
                newPosInfo.PositionEndDate = sourcePos.PositionInformation.PositionEndDate;
                newPosInfo.PositionFTE = sourcePos.PositionInformation.PositionFTE;
                newPosInfo.PositionStatusCode = sourcePos.PositionInformation.PositionStatusCode;
                newPosInfo.OccupationTypeCode = sourcePos.PositionInformation.OccupationTypeCode;
                

                //CostCentre
                newCostCentre.PositionId = newPosition.PositionId;
                newCostCentre.CostCentre = sourcePos.CostCentreDetail.CostCentre;
                newCostCentre.Fund = sourcePos.CostCentreDetail.Fund;
                newCostCentre.PayrollWBS = sourcePos.CostCentreDetail.PayrollWBS;
                newCostCentre.RCCJDEPayrollCode = sourcePos.CostCentreDetail.RCCJDEPayrollCode;
                newCostCentre.GLAccount = sourcePos.CostCentreDetail.GLAccount;
                newCostCentre.LastUpdatedBy = "CloneTest";
                newCostCentre.LastUpdatedDate = DateTime.Now;

                rep.ClonePosition(newPosition, newPosInfo, newCostCentre, sourcePos.PositionNumber);
            }
        }




        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

