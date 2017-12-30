using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow.PositionDescriptionWF
    {
    [TestClass]
    [Ignore]
    public class WorkflowForSysAdminTests : WorkflowBaseTest
        {
        [TestInitialize]
        public void TestInitialize()
            {
            this.CurrentUserRole = Enums.UserRole.Administrator; this.BoolIntegrateWithTrim = false;
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
            var positionDescriptionStatus = Enums.StatusValue.Imported;
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Draft, positionDescriptionStatus);

            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }
            var srv = GetServiceRepository(workflowEngine.Task.User);
            var position = workflowEngine.WorkflowObject as Position;
            var rpd = position.RolePositionDescription;
            var oldRpdStatusId = rpd.StatusId;

            try
                {

                var actions = workflowEngine.GetWorkflowObjectActions();

                Assert.IsTrue(actions.Contains(WorkflowAction.MarkAsImported), "actions.Contains(WorkflowAction.MarkAsImported)");
                Assert.IsTrue(actions.Contains(WorkflowAction.Delete), "actions.Contains(WorkflowAction.Delete)");

                var workflowAction = WorkflowAction.MarkAsImported;
                WorkflowAction.Populate(srv.RepositoryFactory, workflowAction);
                var model = new WorkflowActionModel
                    {
                    ActionId = workflowAction.ActionId,
                    WfObjectId = position.PositionId,
                    ObjectType = WorkflowObjectType.Position,
                    Comment = "From unit test"
                    };


                workflowEngine.ApplyAction(model, true);
                rpd = srv.RolePositionDescriptionRepository().GetRolePositionDescById(rpd.RolePositionDescId);
                srv.RolePositionDescriptionRepository().LoadNavigationProperty(rpd, rp => rp.Positions);
                var positions = rpd.Positions.ToList();

                if(true)
                    {
                    Assert.IsTrue(rpd.StatusId == (int)Enums.StatusValue.Imported,
                        "rpd.StatusId==(int)Enums.StatusValue.Imported");
                    }




                }
            finally
                {

                rpd = srv.RolePositionDescriptionRepository().GetRolePositionDescById(rpd.RolePositionDescId);
                if(rpd.StatusValue.GetEnum() == Enums.StatusValue.Imported)
                    {
                    srv.RolePositionDescriptionRepository().UpdateStatus(rpd.RolePositionDescId, (Enums.StatusValue)oldRpdStatusId);
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
        public override void ActionsForImportedTest()
            {
            var positionDescriptionStatus = Enums.StatusValue.Imported;
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Imported, positionDescriptionStatus);

            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }
            var srv = GetServiceRepository(workflowEngine.Task.User);
            var position = workflowEngine.WorkflowObject as Position;
            var rpd = position.RolePositionDescription;
            var oldRpdStatusId = rpd.StatusId;

            try
                {

                var actions = workflowEngine.GetWorkflowObjectActions();

                Assert.IsTrue(actions.Contains(WorkflowAction.BringToDraft), "actions.Contains(WorkflowAction.BringToDraft)");
                Assert.IsTrue(actions.Contains(WorkflowAction.Clone), "actions.Contains(WorkflowAction.Clone)");

                var workflowAction = WorkflowAction.BringToDraft;
                WorkflowAction.Populate(srv.RepositoryFactory, workflowAction);
                var model = new WorkflowActionModel
                    {
                    ActionId = workflowAction.ActionId,
                    WfObjectId = position.PositionId,
                    ObjectType = WorkflowObjectType.Position,
                    Comment = "From unit test"
                    };


                workflowEngine.ApplyAction(model, true);
                rpd = srv.RolePositionDescriptionRepository().GetRolePositionDescById(rpd.RolePositionDescId);
                srv.RolePositionDescriptionRepository().LoadNavigationProperty(rpd, rp => rp.Positions);
                var positions = rpd.Positions.ToList();

                if(positions.HasAnyIncludeLive())
                    {
                    Assert.IsTrue(rpd.StatusId == (int)Enums.StatusValue.Imported,
                        "rpd.StatusId==(int)Enums.StatusValue.Imported");
                    }
                else
                    {
                    Assert.IsTrue(rpd.StatusId == (int)Enums.StatusValue.Draft,
                        "rpd.StatusId == (int)Enums.StatusValue.Draft");
                    }



                }
            finally
                {

                rpd = srv.RolePositionDescriptionRepository().GetRolePositionDescById(rpd.RolePositionDescId);
                if(rpd.StatusValue.GetEnum() == Enums.StatusValue.Draft)
                    {
                    srv.RolePositionDescriptionRepository().UpdateStatus(rpd.RolePositionDescId, (Enums.StatusValue)oldRpdStatusId);
                    }


                }

            }
        [Ignore]
        public override void ActionsForImportedButHasNoAccessTest()
            {

            }

        [Ignore]
        [TestMethod]
        public override void ActionsForApprovedTest()
            {
            var positionDescriptionStatus = Enums.StatusValue.Approved;
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Imported, positionDescriptionStatus);

            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }
            var srv = GetServiceRepository(workflowEngine.Task.User);
            var position = workflowEngine.WorkflowObject as Position;
            var rpd = position.RolePositionDescription;
            var oldRpdStatusId = rpd.StatusId;

            try
                {

                var actions = workflowEngine.GetWorkflowObjectActions();

                Assert.IsTrue(actions.Contains(WorkflowAction.BringToDraft), "actions.Contains(WorkflowAction.BringToDraft)");
                Assert.IsTrue(actions.Contains(WorkflowAction.Clone), "actions.Contains(WorkflowAction.Clone)");

                var workflowAction = WorkflowAction.BringToDraft;
                WorkflowAction.Populate(srv.RepositoryFactory, workflowAction);
                var model = new WorkflowActionModel
                    {
                    ActionId = workflowAction.ActionId,
                    WfObjectId = position.PositionId,
                    ObjectType = WorkflowObjectType.Position,
                    Comment = "From unit test"
                    };


                workflowEngine.ApplyAction(model, true);
                rpd = srv.RolePositionDescriptionRepository().GetRolePositionDescById(rpd.RolePositionDescId);
                srv.RolePositionDescriptionRepository().LoadNavigationProperty(rpd, rp => rp.Positions);
                var positions = rpd.Positions.ToList();

                if(positions.HasAnyIncludeLive())
                    {
                    Assert.IsTrue(rpd.StatusId == (int)Enums.StatusValue.Approved,
                        "rpd.StatusId==(int)Enums.StatusValue.Imported");
                    }
                else
                    {
                    Assert.IsTrue(rpd.StatusId == (int)Enums.StatusValue.Draft,
                        "rpd.StatusId == (int)Enums.StatusValue.Draft");
                    }



                }
            finally
                {

                rpd = srv.RolePositionDescriptionRepository().GetRolePositionDescById(rpd.RolePositionDescId);
                if(rpd.StatusValue.GetEnum() == Enums.StatusValue.Draft)
                    {
                    srv.RolePositionDescriptionRepository().UpdateStatus(rpd.RolePositionDescId, (Enums.StatusValue)oldRpdStatusId);
                    }


                }

            }
        [Ignore]
        public override void ActionsForApprovedButHasNoAccessTest()
            {

            }
        }
    }