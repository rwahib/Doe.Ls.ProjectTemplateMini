using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Core.Configuration;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow.PositionDescriptionWF
    {
    [TestClass]
    [Ignore]
    public class WorkflowForAdminTests : WorkflowBaseTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            this.CurrentUserRole = Enums.UserRole.Administrator;
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


                var result=workflowEngine.ApplyAction(model, true);
                Console.WriteLine(result);

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




        [TestMethod]
        public void RenameAction()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            
            var srv = new ServiceRepository(factory);
            var rep = srv.PositionDescriptionRepository();
            var rpdRep = srv.RolePositionDescriptionRepository();
            var pd = rep.ActiveList().Skip(10).FirstOrDefault();

            var admin = GenerateSampleUser(this.CurrentUserRole, factory);

            var workflowEngine = WorkflowEngineFactory.CreatEngine(pd, UserTaskFactory.GetTask(admin, factory));
            
            
                var workflowAction = WorkflowAction.Rename;
                WorkflowAction.Populate(srv.RepositoryFactory, workflowAction);
                var model = new WorkflowActionModel
                    {
                    ActionId = workflowAction.ActionId,
                    WfObjectId = pd.PositionDescriptionId,
                    ObjectType = WorkflowObjectType.Position,
                    Comment = "From unit test"
                    };

            var coll=new NameValueCollection();
            var oldTitle = pd.RolePositionDescription.Title;
            var title = pd.RolePositionDescription.Title+"_new";

            coll.Add("NewTitle", title);

                var result = workflowEngine.ApplyAction(model, true, coll);
                Console.WriteLine(result);

                var rpd = rpdRep.GetRolePositionDescById(pd.PositionDescriptionId);

            Console.WriteLine(rpd);


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

                
                var result=workflowEngine.ApplyAction(model, true);
                Console.WriteLine(result);
                rpd = srv.RolePositionDescriptionRepository().GetRolePositionDescById(rpd.RolePositionDescId);
                srv.RolePositionDescriptionRepository().LoadNavigationProperty(rpd, rp=> rp.Positions);
                var positions = rpd.Positions.ToList();

                if (positions.HasAnyIncludeLive())
                    {
                        Assert.IsTrue(rpd.StatusId == (int) Enums.StatusValue.Imported,
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


                var result=workflowEngine.ApplyAction(model, true);
                Console.WriteLine(result);
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