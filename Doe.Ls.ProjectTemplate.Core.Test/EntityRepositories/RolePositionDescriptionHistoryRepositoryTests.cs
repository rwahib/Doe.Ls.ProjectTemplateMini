

using System;
using System.Linq;
using System.Text;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class RolePositionDescriptionHistoryRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.RolePositionDescriptionHistoryRepository();
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
            var rep = globalServiceRepository.RolePositionDescriptionHistoryRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionHistoryRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionHistoryRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionHistoryRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.RolePositionDescriptionHistoryRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }


    [TestMethod]
    public void TestCreateRolePosDesc()
    {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var svr = new ServiceRepository(factory);
            var rep = svr.RolePositionDescriptionHistoryRepository();
            var repRd = svr.RolePositionDescriptionRepository();

        var lastRpd = repRd.BaseList().Where(rp => !rp.IsPositionDescription)
                .OrderByDescending(rp => rp.RolePositionDescId).FirstOrDefault();

            
            var rpd = new RolePositionDescription
            {
                RolePositionDescId = lastRpd.RolePositionDescId + 10,
                StatusId = (int)Enums.StatusValue.Draft,
                Version = 1,
                Title = "Test-" + UnitTestToken,
                DocNumber = "DOC99/12345678",
                GradeCode = "CL2/3",
                IsPositionDescription = false,
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = "UnitTest"
            };

            repRd.Insert(rpd);

            //add to history
            var history = new RolePositionDescriptionHistory
            {
                RolePositionDescId = rpd.RolePositionDescId,
                Action = Enum.GetName(typeof(Enums.ActionType), Enums.ActionType.Create),
                StatusFrom = "NA",
                StatusTo = Enum.GetName(typeof(Enums.StatusValue), Enums.StatusValue.Draft),
                AdditionalInfo = "Test-" + UnitTestToken,
                CreatedBy = "Test-" + UnitTestToken,
                CreatedDate = DateTime.Now
            };
            rep.Insert(history);

        }

    [TestMethod]
    public void TestLogBudgetExpeditureChange()
    {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var svr = new ServiceRepository(factory);
            var rep = svr.RolePositionDescriptionHistoryRepository();
            var repRd = svr.RoleDescriptionRepository();

        var rd = repRd.ListForPrimitiveItems().OrderByDescending(r => r.RoleDescriptionId).FirstOrDefault();

        var rdNew = new RoleDescription
        {
            RoleDescriptionId = rd.RoleDescriptionId,
            BudgetExpenditure = "Change of the Budget / Expenditure",
            BudgetExpenditureValue = "There are $5 billions to spend this year",
        };
            rdNew.RolePositionDescription = new RolePositionDescription
            {
                RolePositionDescId = rd.RoleDescriptionId,
                StatusId = (int)Enums.StatusValue.Draft,
                Version = 1,
                Title = "Test",
                DocNumber = "DOC99/124563",
                GradeCode = "CL1/2",
                IsPositionDescription = false,
                CreatedBy = "Test-" + UnitTestToken,
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Test-" + UnitTestToken,
                LastModifiedDate = DateTime.Now
            };

        
       StringBuilder sb = new StringBuilder();
            sb.Append("Old Budget/Expenditure = '");
            sb.Append(rd.BudgetExpenditure);
            sb.Append("'. Old Budget/Expenditure value = '");
            sb.Append(rd.BudgetExpenditureValue);
            sb.Append("'. ");
            sb.Append("New Budget/Expenditure = '");
            sb.Append(rdNew.BudgetExpenditure);
            sb.Append("'. New Budget/Expenditure value = '");
            sb.Append(rdNew.BudgetExpenditureValue);
            sb.Append("'");

            rep.LogHistoryWhenUpdated(rd.RoleDescriptionId, 30, 20, 
                sb, "Budget/Expenditure", "Test-" +UnitTestToken);
        }

    [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

