using System;
using System.Linq;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow
    {
    [TestClass]
    public class WorkflowEngineTests : SecurityBase
        {

       
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }

        [TestMethod]
        public void CreateEngine()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);            
            var posSrv = srv.PositionRepository();

            var sysAdmin = GetSampleSysUsers(Enums.UserRole.SystemAdministrator, factory, true, 1).FirstOrDefault();

            var posList = posSrv.BaseList().Where(p => p.StatusId == (int)Enums.StatusValue.Draft).Take(3).ToArray();

            foreach(var position in posList)
                {
                var engine = WorkflowEngineFactory.CreatEngine(position,
                    UserTaskFactory.GetTask(UserInfoExtension.MapSysUser(sysAdmin, factory), factory));

                Assert.IsTrue(
                    engine.Task.User.UserName.Equals(sysAdmin.UserId, StringComparison.InvariantCultureIgnoreCase),
                    "engine.Task.User.UserName.Equals(sysAdmin.UserId,StringComparison.InvariantCultureIgnoreCase)");


                Assert.IsTrue(engine.PositionRdPdTasks is PositionTasks, "engine.PositionRdPdTasks is PositionTasks");

                Assert.IsTrue((engine.GetWorkflowObject<Position>()).PositionId == position.PositionId, "(engine.WorkflowObject as Position).PositionId== position.PositionId");

                }


            }

       
        
        [TestMethod]
        public  void ApplySubmitActionOnDraft()
        {
            this.CurrentUserRole = Enums.UserRole.BusinessUnitAuthor;
            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Draft, true);
            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }

            var srv = new ServiceRepository(workflowEngine.Task.RepositoryFactory);

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Contains(WorkflowAction.Submit), "actions.Contains(WorkflowAction.Submit)");


            var wfObject = workflowEngine.WorkflowObject;
            Console.Write($"Workflow object {wfObject} with status {wfObject.WorkflowObjectStatus}: ");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Draft,"wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Draft");

            var action = WorkflowAction.Submit;

           var result= workflowEngine.ApplyAction(new WorkflowActionModel
            {
               ActionId= action.ActionId,
               WfObjectId = wfObject.WorkflowObjectId,
               ObjectType = wfObject.WorkflowObjectType,
               ApprovalDate = null,
               Comment = "A test performed from the unit test"
               
            });

            
            Console.WriteLine($"Has been {action.ActionStatus} and the new Status is {wfObject.WorkflowObjectStatus.GetEnum()}");

            Assert.IsTrue(result.Status==Status.Success,"result.Status==Status.Success");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Submitted,"wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Submitted");

            var positionHistorySrv=srv.PositionRepository().PositionHistoryRepository;
            var entries = positionHistorySrv.GetHistoryEntriesByPositionId(wfObject.WorkflowObjectId);

            Assert.IsTrue(entries.Any(his=>his.StatusFrom.ToLower().Contains(Enums.StatusValue.Draft.ToString().ToLower())),"entries.Any(his=>his.StatusFrom.ToLower().Contains(Enums.StatusValue.Draft.ToString().ToLower()))");
            Assert.IsTrue(entries.Any(his=>his.StatusTo.ToLower().Contains(action.ActionStatus.ToLower())),"entries.Any(his=>his.StatusTo.ToLower().Contains(action.ActionStatus.ToLower()))");


        }
        
        
        [TestMethod]
        public  void ApplyEndorseActionOnSubmitted()
            {

            var action = WorkflowAction.Endorse;

            this.CurrentUserRole = Enums.UserRole.DirectorateEndorser;

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Submitted, true);
            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }

            var srv = new ServiceRepository(workflowEngine.Task.RepositoryFactory);

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Contains(WorkflowAction.Endorse), "actions.Contains(WorkflowAction.Endorse)");


            var wfObject = workflowEngine.WorkflowObject;
            Console.Write($"Workflow object {wfObject} with status {wfObject.WorkflowObjectStatus}: ");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Submitted, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Submitted");

            var result = workflowEngine.ApplyAction(new WorkflowActionModel
                {
                ActionId = action.ActionId,
                WfObjectId = wfObject.WorkflowObjectId,
                ObjectType = wfObject.WorkflowObjectType,
                ApprovalDate = null,
                Comment = GenerateString("A test performed from the unit test")

                });


            Console.WriteLine($"Has been {action.ActionStatus} and the new Status is {wfObject.WorkflowObjectStatus.GetEnum()}");

            Assert.IsTrue(result.Status == Status.Success, "result.Status==Status.Success");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Endorsed, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Endorsed");

            var positionHistorySrv = srv.PositionRepository().PositionHistoryRepository;
            var entries = positionHistorySrv.GetHistoryEntriesByPositionId(wfObject.WorkflowObjectId).ToArray();

            Assert.IsTrue(entries.Any(his => his.StatusFrom.ToLower().Contains(Enums.StatusValue.Submitted.ToString().ToLower())), "entries.Any(his=>his.StatusFrom.ToLower().Contains(Enums.StatusValue.Submitted.ToString().ToLower()))");
            Assert.IsTrue(entries.Any(his => his.StatusTo.ToLower().Contains(action.GetStatus().ToString().ToLower())), "entries.Any(his => his.StatusTo.Contains(action.GetStatus().ToString()))");
            

            }

        [TestMethod]
        public void ApplyRejectActionOnSubmitted()
            {

            var action = WorkflowAction.Reject;

            this.CurrentUserRole = Enums.UserRole.DirectorateEndorser;

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Submitted, true);
            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }

            var srv = new ServiceRepository(workflowEngine.Task.RepositoryFactory);

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Contains(WorkflowAction.Reject), "actions.Contains(WorkflowAction.Reject)");


            var wfObject = workflowEngine.WorkflowObject;
            var oldStatus = wfObject.WorkflowObjectStatus;
            Console.Write($"Workflow object {wfObject} with status {wfObject.WorkflowObjectStatus}: ");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Submitted, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Submitted");

            var result = workflowEngine.ApplyAction(new WorkflowActionModel
                {
                ActionId = action.ActionId,
                WfObjectId = wfObject.WorkflowObjectId,
                ObjectType = wfObject.WorkflowObjectType,
                ApprovalDate = null,
                Comment = GenerateString("A test performed from the unit test"),                
                });

            var positionHistorySrv = srv.PositionRepository().PositionHistoryRepository;
            var entries = positionHistorySrv.GetHistoryEntriesByPositionId(wfObject.WorkflowObjectId).ToArray();

            Console.WriteLine($"Has been {action.ActionStatus} and the new Status is {wfObject.WorkflowObjectStatus.GetEnum()}");

            Assert.IsTrue(result.Status == Status.Success, "result.Status==Status.Success");

            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Draft, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Draft");
            
            Assert.IsTrue(entries.Any(his => his.StatusFrom.ToLower().Contains(oldStatus.ToString().ToLower())), "entries.Any(his=>his.StatusFrom.ToLower().Contains(Enums.StatusValue.Submitted.ToString().ToLower()))");
            Assert.IsTrue(entries.Any(his => his.StatusTo.ToLower().Contains(Enums.StatusValue.Draft.ToString().ToLower())), "entries.Any(his => his.StatusTo.ToLower().Contains(Enums.StatusValue.Draft.ToString().ToLower()))");


            }

        [TestMethod]
        public void ApplyApproveActionOnEndorsed()
            {

            var action = WorkflowAction.Approve;

            this.CurrentUserRole = Enums.UserRole.DivisionApprover;

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Endorsed, true);
            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }

            var srv = new ServiceRepository(workflowEngine.Task.RepositoryFactory);

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Contains(WorkflowAction.Reject), "actions.Contains(WorkflowAction.Reject)");


            var wfObject = workflowEngine.WorkflowObject;
            var oldStatus = wfObject.WorkflowObjectStatus;
            Console.Write($"Workflow object {wfObject} with status {wfObject.WorkflowObjectStatus}: ");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Endorsed, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Endorsed");

            var result = workflowEngine.ApplyAction(new WorkflowActionModel
                {
                ActionId = action.ActionId,
                WfObjectId = wfObject.WorkflowObjectId,
                ObjectType = wfObject.WorkflowObjectType,
                ApprovalDate = null,
                Comment = GenerateString("A test performed from the unit test"),
                });

            var positionHistorySrv = srv.PositionRepository().PositionHistoryRepository;
            var entries = positionHistorySrv.GetHistoryEntriesByPositionId(wfObject.WorkflowObjectId).ToArray();

            Console.WriteLine($"Has been {action.ActionStatus} and the new Status is {wfObject.WorkflowObjectStatus.GetEnum()}");

            Assert.IsTrue(result.Status == Status.Success, "result.Status==Status.Success");

            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Approved, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Approved");

            Assert.IsTrue(entries.Any(his => his.StatusFrom.ToLower().Contains(oldStatus.ToString().ToLower())), "entries.Any(his=>his.StatusFrom.ToLower().Contains(Enums.StatusValue.Submitted.ToString().ToLower()))");
            Assert.IsTrue(entries.Any(his => his.StatusTo.ToLower().Contains(Enums.StatusValue.Approved.ToString().ToLower())), "entries.Any(his => his.StatusTo.ToLower().Contains(Enums.StatusValue.Approved.ToString().ToLower()))");



            }

        [TestMethod]
        public void ApplyRejectAndForceDraftActionOnEndorsed()
            {

            var action = WorkflowAction.Reject;

            this.CurrentUserRole = Enums.UserRole.DivisionApprover;

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Endorsed, true);
            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }

            var srv = new ServiceRepository(workflowEngine.Task.RepositoryFactory);

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Contains(WorkflowAction.Reject), "actions.Contains(WorkflowAction.Endorse)");


            var wfObject = workflowEngine.WorkflowObject;
            Console.Write($"Workflow object {wfObject} with status {wfObject.WorkflowObjectStatus}: ");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Endorsed, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Endorsed");

            var result = workflowEngine.ApplyAction(new WorkflowActionModel
                {
                ActionId = action.ActionId,
                WfObjectId = wfObject.WorkflowObjectId,
                ObjectType = wfObject.WorkflowObjectType,
                ApprovalDate = null,
                Comment = GenerateString("A test performed from the unit test"),
                NextStatus = Enums.StatusValue.Draft

                });


            Console.WriteLine($"Has been {action.ActionStatus} and the new Status is {wfObject.WorkflowObjectStatus.GetEnum()}");

            Assert.IsTrue(result.Status == Status.Success, "result.Status==Status.Success");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Draft, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Draft");

            var positionHistorySrv = srv.PositionRepository().PositionHistoryRepository;
            var entries = positionHistorySrv.GetHistoryEntriesByPositionId(wfObject.WorkflowObjectId).ToArray();

            Assert.IsTrue(entries.Any(his => his.StatusFrom.ToLower().Contains(Enums.StatusValue.Endorsed.ToString().ToLower())), "entries.Any(his=>his.StatusFrom.ToLower().Contains(Enums.StatusValue.Submitted.ToString().ToLower()))");
            Assert.IsTrue(entries.Any(his => his.StatusTo.ToLower().Contains(Enums.StatusValue.Draft.ToString().ToLower())), "entries.Any(his => his.StatusTo.Contains(action.GetStatus().ToString()))");


            }


        [TestMethod]
        public void ApplyRejectAndKeepSubmittedActionOnEndorsed()
            {

            var action = WorkflowAction.Reject;

            this.CurrentUserRole = Enums.UserRole.DivisionApprover;

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Endorsed, true);
            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }

            var srv = new ServiceRepository(workflowEngine.Task.RepositoryFactory);

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Contains(WorkflowAction.Reject), "actions.Contains(WorkflowAction.Endorse)");


            var wfObject = workflowEngine.WorkflowObject;
            Console.Write($"Workflow object {wfObject} with status {wfObject.WorkflowObjectStatus}: ");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Endorsed, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Endorsed");

            var result = workflowEngine.ApplyAction(new WorkflowActionModel
                {
                ActionId = action.ActionId,
                WfObjectId = wfObject.WorkflowObjectId,
                ObjectType = wfObject.WorkflowObjectType,
                ApprovalDate = null,
                Comment = GenerateString("A test performed from the unit test"),
                
                });


            Console.WriteLine($"Has been {action.ActionStatus} and the new Status is {wfObject.WorkflowObjectStatus.GetEnum()}");

            Assert.IsTrue(result.Status == Status.Success, "result.Status==Status.Success");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Submitted, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Submitted");

            var positionHistorySrv = srv.PositionRepository().PositionHistoryRepository;
            var entries = positionHistorySrv.GetHistoryEntriesByPositionId(wfObject.WorkflowObjectId).ToArray();

            Assert.IsTrue(entries.Any(his => his.StatusFrom.ToLower().Contains(Enums.StatusValue.Endorsed.ToString().ToLower())), "entries.Any(his=>his.StatusFrom.ToLower().Contains(Enums.StatusValue.Submitted.ToString().ToLower()))");
            Assert.IsTrue(entries.Any(his => his.StatusTo.ToLower().Contains(Enums.StatusValue.Submitted.ToString().ToLower())), "entries.Any(his => his.StatusTo.Contains(Submitted.ToString()))");



            }


        [TestMethod]
        public void ApplyMarkAsDraftOnApproved()
            {

            var action = WorkflowAction.BringToDraft;

            this.CurrentUserRole = Enums.UserRole.DivisionApprover;

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Approved, true);
            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }

            var srv = new ServiceRepository(workflowEngine.Task.RepositoryFactory);

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Contains(WorkflowAction.BringToDraft), "actions.Contains(WorkflowAction.BringToDraft)");


            var wfObject = workflowEngine.WorkflowObject;
            Console.Write($"Workflow object {wfObject} with status {wfObject.WorkflowObjectStatus}: ");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Approved, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Approved");

            var result = workflowEngine.ApplyAction(new WorkflowActionModel
                {
                ActionId = action.ActionId,
                WfObjectId = wfObject.WorkflowObjectId,
                ObjectType = wfObject.WorkflowObjectType,
                ApprovalDate = null,
                Comment = GenerateString("A test performed from the unit test"),
                NextStatus = Enums.StatusValue.Draft

                });


            Console.WriteLine($"Has been {action.ActionStatus} and the new Status is {wfObject.WorkflowObjectStatus.GetEnum()}");

            Assert.IsTrue(result.Status == Status.Success, "result.Status==Status.Success");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Draft, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Draft");

            var positionHistorySrv = srv.PositionRepository().PositionHistoryRepository;
            var entries = positionHistorySrv.GetHistoryEntriesByPositionId(wfObject.WorkflowObjectId).ToArray();

            Assert.IsTrue(entries.Any(his => his.StatusFrom.ToLower().Contains(Enums.StatusValue.Approved.ToString().ToLower())), "entries.Any(his=>his.StatusFrom.ToLower().Contains(Enums.StatusValue.Approved.ToString().ToLower()))");
            Assert.IsTrue(entries.Any(his => his.StatusTo.ToLower().Contains(Enums.StatusValue.Draft.ToString().ToLower())), "entries.Any(his => his.StatusTo.Contains(Draft.ToString()))");


            }


        [TestMethod]
        public void ApplyMarkAsImportedOnDraft()
            {

            var action = WorkflowAction.MarkAsImported;

            this.CurrentUserRole = Enums.UserRole.Administrator;

            var workflowEngine = GetWorkflowEngine(Enums.StatusValue.Draft, true);
            if(workflowEngine == null)
                {
                Assert.Inconclusive("There isn't enough data to perform the test");
                }

            var srv = new ServiceRepository(workflowEngine.Task.RepositoryFactory);

            var actions = workflowEngine.GetWorkflowObjectActions();

            Assert.IsTrue(actions.Contains(WorkflowAction.MarkAsImported), "actions.Contains(WorkflowAction.MarkAsImported)");


            var wfObject = workflowEngine.WorkflowObject;
            Console.Write($"Workflow object {wfObject} with status {wfObject.WorkflowObjectStatus}: ");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Draft, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Draft");

            var result = workflowEngine.ApplyAction(new WorkflowActionModel
                {
                ActionId = action.ActionId,
                WfObjectId = wfObject.WorkflowObjectId,
                ObjectType = wfObject.WorkflowObjectType,
                ApprovalDate = null,
                Comment = GenerateString("A test performed from the unit test")

                });


            Console.WriteLine($"Has been {action.ActionStatus} and the new Status is {wfObject.WorkflowObjectStatus.GetEnum()}");

            Assert.IsTrue(result.Status == Status.Success, "result.Status==Status.Success");
            Assert.IsTrue(wfObject.WorkflowObjectStatus.GetEnum() == Enums.StatusValue.Imported, "wfObject.WorkflowObjectStatus.GetEnum()==Enums.StatusValue.Imported");

            var positionHistorySrv = srv.PositionRepository().PositionHistoryRepository;
            var entries = positionHistorySrv.GetHistoryEntriesByPositionId(wfObject.WorkflowObjectId).ToArray();

            Assert.IsTrue(entries.Any(his => his.StatusFrom.ToLower().Contains(Enums.StatusValue.Draft.ToString().ToLower())), "entries.Any(his=>his.StatusFrom.ToLower().Contains(Enums.StatusValue.Submitted.ToString().ToLower()))");
            Assert.IsTrue(entries.Any(his => his.StatusTo.ToLower().Contains(Enums.StatusValue.Imported.ToString().ToLower())), "entries.Any(his => his.StatusTo.ToLower().Contains(Enums.StatusValue.Imported.ToString().ToLower()))");


            }
        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();
            }

        }


    }

