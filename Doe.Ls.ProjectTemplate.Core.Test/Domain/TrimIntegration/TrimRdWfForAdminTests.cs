using System;
using System.Collections.Specialized;
using System.Linq;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.TrimIntegration
    {
    [TestClass]
    public class TrimRdWfForAdminTests : WorkflowBaseTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
           

            this.CurrentUserRole = Enums.UserRole.Administrator;
            this.BoolIntegrateWithTrim = true;
        }


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
            if(!Settings.ProjectTemplateSettings.Site.TrimIntegration) {
                Assert.Inconclusive("Trim integration is not enabled");
             }
            }

        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }


        [TestMethod]
   
        public override void ActionsForDraftTest()
        {
            var wf = GetTrimtWorkflowEngine(false);
            var srv = new ServiceRepository(wf.Task.RepositoryFactory);
            var trimRecordSrv = srv.TrimRecordRepository();

            WorkflowAction workflowAction;
            WorkflowActionModel model;
          
            if (wf.WorkflowObject.WorkflowObjectStatus.GetEnum() != Enums.StatusValue.Draft)
            {
                workflowAction = WorkflowAction.BringToDraft;
                WorkflowAction.Populate(srv.RepositoryFactory, workflowAction);
                model = new WorkflowActionModel {
                    ActionId = workflowAction.ActionId,
                    WfObjectId = wf.WorkflowObject.WorkflowObjectId,
                    ObjectType = WorkflowObjectType.PositionDescription,
                    Comment = "From unit test for Trim"
                };

                 wf.ApplyAction(model, true);

                var modelInfo = trimRecordSrv.GetRecordInfoModel(model.WfObjectId);
                var record = trimRecordSrv.GetTrimRecordById(model.WfObjectId);

                Assert.IsTrue(modelInfo.OnlineRecordStatus == OnlineRecordStatus.OutOfSync,  "modelInfo.OnlineRecordStatus == OnlineRecordStatus.OutOfSync ");
                if(!string.IsNullOrWhiteSpace(record?.Token)) Assert.AreNotEqual(modelInfo.Token, record.Token, "Assert.AreNotEqual(modelInfo.Token, record.Token)");
                Console.WriteLine(modelInfo);


                }

            if(wf.WorkflowObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Draft) {

                workflowAction = WorkflowAction.MarkAsImported;
                WorkflowAction.Populate(srv.RepositoryFactory, workflowAction);
                model = new WorkflowActionModel {
                    ActionId = workflowAction.ActionId,
                    WfObjectId = wf.WorkflowObject.WorkflowObjectId,
                    ObjectType = WorkflowObjectType.PositionDescription,
                    Comment = "From unit test for Trim"
                    };

                wf.ApplyAction(model, true);

                var modelInfo = trimRecordSrv.GetRecordInfoModel(model.WfObjectId);
                var record = trimRecordSrv.GetTrimRecordById(model.WfObjectId);

                Console.WriteLine("--- Sync Imported record");

                Console.WriteLine(modelInfo);

                Assert.IsTrue(modelInfo.OnlineRecordStatus == OnlineRecordStatus.UpToDate, "modelInfo.OnlineRecordStatus == OnlineRecordStatus.Uptodate");
                Assert.AreEqual(modelInfo.Token, record.Token, "Assert.AreEqual(modelInfo.Token, record.Token");
                }

        }



        [TestMethod]
        public void RenameAction() {
            var wf = GetTrimtWorkflowEngine(false);
            var srv = new ServiceRepository(wf.Task.RepositoryFactory);
            var trimRecordSrv = srv.TrimRecordRepository();

            WorkflowAction workflowAction;
            WorkflowActionModel model;


            workflowAction = WorkflowAction.Rename;
            WorkflowAction.Populate(srv.RepositoryFactory, workflowAction);
            model = new WorkflowActionModel {
                ActionId = workflowAction.ActionId,
                WfObjectId = wf.WorkflowObject.WorkflowObjectId,
                ObjectType = WorkflowObjectType.PositionDescription,
                Comment = "From unit test for Trim"
                };

            var coll = new NameValueCollection();
            var oldTitle = wf.WorkflowObject.WorkflowObjectTitle;
            var title = oldTitle + "_new";

            coll["NewTitle"] = title;

            wf.ApplyAction(model, true, coll);

            var modelInfo = trimRecordSrv.GetRecordInfoModel(model.WfObjectId);
            Assert.AreEqual(modelInfo.Title, title, "Assert.AreEqual(modelInfo.Title, title");

            coll["NewTitle"] = oldTitle;

            wf.ApplyAction(model, true, coll);

            modelInfo = trimRecordSrv.GetRecordInfoModel(model.WfObjectId);
            Assert.AreEqual(modelInfo.Title, oldTitle, "Assert.AreEqual(modelInfo.Title, oldTitle");


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