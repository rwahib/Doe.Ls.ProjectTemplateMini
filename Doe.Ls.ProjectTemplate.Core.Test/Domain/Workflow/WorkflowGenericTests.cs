using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow
    {
    [TestClass]
    public class WorkflowGenericTests : SecurityBase
        {

        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }

        [TestMethod]
       [Ignore]
        public void TestScenarios()
            {

            //    TraceWorkflowScenario(653, "alison.lochrin");
              //  TraceWorkflowScenario(9261, "michaela.clark2");
            }


        [TestMethod]
        [Ignore]
        public void PositionInformationTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var posInfoRepo = srv.PositionInformationRepository();

            var posInfoOld = posInfoRepo.GetPositionInformationById(145);

            var updatedPosInfo = new PositionInformation
            {
                PositionTypeCode = "O",
                PositionStatusCode = "F",
                PositionFTE = 1,
                EmployeeTypeCode = "OP",
                OccupationTypeCode = "4",                
            };

            posInfoRepo.UpdateOrInsertPositionInformation(updatedPosInfo,"Hello");

            }


        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();
            }

        }


    }

