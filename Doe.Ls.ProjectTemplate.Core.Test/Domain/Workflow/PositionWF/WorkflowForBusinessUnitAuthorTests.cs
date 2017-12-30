using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow.PositionWF
    {
    [TestClass]
    //[Ignore]
    public class WorkflowForBusinessUnitAuthorTests : WorkflowBaseTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            this.CurrentUserRole = Enums.UserRole.BusinessUnitAuthor;
            this.BoolIntegrateWithTrim = false;
        }


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }

        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }


        [TestMethod]
        public override void ActionsForDraftTest()
        {
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Draft, true);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Contains(WorkflowAction.Submit), "!actions.Contains(WorkflowAction.Submit)");
            Assert.IsTrue(actions.Contains(WorkflowAction.Delete), "actions.Contains(WorkflowAction.Delete)");
        }



        [TestMethod]
        public override void ActionsForDraftButHasNoAccessTest()
        {

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Draft, false);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Count == 0, "actions.Count==0");

            }

        [TestMethod]
        public override void ActionsForSubmittedTest()
        {

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Submitted, true);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Count == 0, "actions.Count==0");

            }

        [TestMethod]

        public override void ActionsForSubmittedButHasNoAccessTest()
        {
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Submitted, false);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }

            var actions = workflowEngine.GetWorkflowObjectActions();


            Assert.IsTrue(actions.Count == 0, "actions.Count==0");

            }

        [TestMethod]
        public override void ActionsForEndorsedTest()
        {

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Endorsed, true);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }

            var actions = workflowEngine.GetWorkflowObjectActions();


            Assert.IsTrue(actions.Count == 0, "actions.Count==0");

        }

        [TestMethod]

        public override void ActionsForEndorsedButHasNoAccessTest()
        {
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Endorsed, false);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }

            var actions = workflowEngine.GetWorkflowObjectActions();
            Assert.IsTrue(actions.Count == 0, "actions.Count==0");

            }

        [TestMethod]
        public override void ActionsForImportedTest()
        {
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Imported, true);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }

            var actions = workflowEngine.GetWorkflowObjectActions();


            Assert.IsTrue(actions.Contains(WorkflowAction.Clone), "actions.Contains(WorkflowAction.Clone)");
            
        }


        [Ignore]
        public override void ActionsForImportedButHasNoAccessTest()
        {
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Imported, false);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }

            var actions = workflowEngine.GetWorkflowObjectActions();


            Assert.IsTrue(actions.Count == 0, "actions.Count==0");
        }

        [TestMethod]
        public override void ActionsForApprovedTest()
        {
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Approved, true);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }

            var actions = workflowEngine.GetWorkflowObjectActions();
            Assert.IsTrue(actions.Contains(WorkflowAction.Clone), "actions.Contains(WorkflowAction.Clone)");

        }

        [TestMethod]

        public override void ActionsForApprovedButHasNoAccessTest()
        {
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Approved, false);
            if(workflowEngine == null)
            {
                Assert.Inconclusive("There isn't enough data to perform the test");
            }
            var actions = workflowEngine.GetWorkflowObjectActions();
            Assert.IsTrue(actions.Count == 0, "actions.Count==0");


        }


    }
}