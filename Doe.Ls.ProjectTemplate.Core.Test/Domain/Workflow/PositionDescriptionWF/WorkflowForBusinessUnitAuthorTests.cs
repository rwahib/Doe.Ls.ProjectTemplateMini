using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow.PositionDescriptionWF
    {
    [TestClass]
    [Ignore]
    public class WorkflowForBusinessUnitAuthorTests : WorkflowBaseTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            this.CurrentUserRole = Enums.UserRole.BusinessUnitAuthor; this.BoolIntegrateWithTrim = false;
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
            var positionDescriptionStatus = Enums.StatusValue.Draft;
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Draft, positionDescriptionStatus);

            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }
            var srv = GetServiceRepository(workflowEngine.Task.User);
            var position= workflowEngine.WorkflowObject as Position;
            var rpd = position.RolePositionDescription;
            
            try
            {

                var actions = workflowEngine.GetWorkflowObjectActions();

                Assert.IsTrue(actions.Contains(WorkflowAction.Submit), "!actions.Contains(WorkflowAction.Submit)");
                Assert.IsTrue(actions.Contains(WorkflowAction.Delete), "actions.Contains(WorkflowAction.Delete)");

                var workflowAction = WorkflowAction.Submit;
                WorkflowAction.Populate(srv.RepositoryFactory,workflowAction);
                var model = new WorkflowActionModel
                {
                    ActionId = workflowAction.ActionId,
                    WfObjectId = position.PositionId,
                    ObjectType = WorkflowObjectType.Position,
                    Comment = "From unit test"
                };


                workflowEngine.ApplyAction(model, true);
                Assert.IsTrue(position.RolePositionDescription.StatusValue.GetEnum() == Enums.StatusValue.Submitted,
                    "pos.RolePositionDescription.StatusValue.GetEnum()==Enums.StatusValue.Submitted");
            }
            finally
            {
              
                rpd = srv.RolePositionDescriptionRepository().GetRolePositionDescById(rpd.RolePositionDescId);
                if (rpd.StatusValue.GetEnum() == Enums.StatusValue.Submitted)
                {
                    srv.RolePositionDescriptionRepository().UpdateStatus(rpd.RolePositionDescId,Enums.StatusValue.Draft);
                }
            }

        }

        [Ignore]
        public override void ActionsForDraftButHasNoAccessTest()
        {
            
        }


        [TestMethod]
        [Ignore]
        public override void ActionsForSubmittedTest()
        {
            
          }
        [Ignore]
        public override void ActionsForSubmittedButHasNoAccessTest()
        {
            
        }
        [Ignore]
        public override void ActionsForEndorsedTest()
        {
            
        }
        [Ignore]
        public override void ActionsForEndorsedButHasNoAccessTest()
        {
            
        }


        [TestMethod]
        [Ignore]
        public override void ActionsForImportedTest()
        {
            
            
        }
        [Ignore]
        public override void ActionsForImportedButHasNoAccessTest()
        {
            
        }

        [Ignore]
        [TestMethod]
        public override void ActionsForApprovedTest()
        {
            

        }
        [Ignore]
        public override void ActionsForApprovedButHasNoAccessTest()
        {
            
        }
    }
}