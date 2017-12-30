using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow.PositionWF
    {
    [TestClass]
    //[Ignore]
    public class WorkflowForSysAdminTests : WorkflowBaseTest
        {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }
 [TestInitialize]
        public void TestInitialize()
        {
            
            this.BoolIntegrateWithTrim = false;
        }


        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();
            }


        [TestMethod]
        public override void ActionsForDraftTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var posRep=srv.PositionRepository();
            var posList = posRep.BaseList().Where(p => p.StatusId == (int) Enums.StatusValue.Draft);
            var admin = GetSampleUsers(Enums.UserRole.SystemAdministrator, factory, true, 1).FirstOrDefault();
            if (admin == null)
            {
                 Assert.Inconclusive("There is no valid user role to perform the test");
                }
            var testIsRan = false;
            foreach (var pos in posList)
            {

                var workflowEngine = WorkflowEngineFactory.CreatEngine(pos, admin, factory);

                if (!workflowEngine.PositionRdPdTasks.IsValid()) continue;
                var actions = workflowEngine.GetWorkflowObjectActions();
                    
                Assert.IsTrue(actions.Contains(WorkflowAction.MarkAsImported),"actions.Contains(WorkflowAction.MarkAsImported)");
                Assert.IsTrue(actions.Contains(WorkflowAction.Delete), "actions.Contains(WorkflowAction.Delete)");
                testIsRan = true;
                break;
            }
            

          if(!testIsRan)Assert.Inconclusive("There is no valid data to perform the test");


            }

        
        [TestMethod]
        [Ignore]
        public override void ActionsForDraftButHasNoAccessTest()
            {
            
            }

        [TestMethod]
        public override void ActionsForSubmittedTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var posRep = srv.PositionRepository();
            var posList = posRep.BaseList().Where(p => p.StatusId == (int)Enums.StatusValue.Submitted);
            var admin = GetSampleUsers(Enums.UserRole.SystemAdministrator, factory, true, 1).FirstOrDefault();
            if(admin == null)
                {
                Assert.Inconclusive("There is no valid user role to perform the test");
                }
            var testIsRan = false;
            foreach(var pos in posList)
                {

                var workflowEngine = WorkflowEngineFactory.CreatEngine(pos, admin, factory);

                
                var actions = workflowEngine.GetWorkflowObjectActions();
                    if (workflowEngine.PositionRdPdTasks.IsValid())
                    {
                        Assert.IsTrue(actions.Contains(WorkflowAction.Approve),
                            "actions.Contains(WorkflowAction.Approve)");
                        Assert.IsTrue(actions.Contains(WorkflowAction.Reject), "actions.Contains(WorkflowAction.Reject)");
                    }
                    testIsRan = true;
                break;
                }


            if(!testIsRan) Assert.Inconclusive("There is no valid data to perform the test");

            }

        [TestMethod]
        [Ignore]
        public override void ActionsForSubmittedButHasNoAccessTest()
            {
            
            }

        [TestMethod]
        public override void ActionsForEndorsedTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var posRep = srv.PositionRepository();
            var posList = posRep.BaseList().Where(p => p.StatusId == (int)Enums.StatusValue.Endorsed);
            var admin = GetSampleUsers(Enums.UserRole.SystemAdministrator, factory, true, 1).FirstOrDefault();
            if(admin == null)
                {
                Assert.Inconclusive("There is no valid user role to perform the test");
                }
            var testIsRan = false;
            foreach(var pos in posList)
                {

                var workflowEngine = WorkflowEngineFactory.CreatEngine(pos, admin, factory);


                var actions = workflowEngine.GetWorkflowObjectActions();
                    if (workflowEngine.PositionRdPdTasks.IsValid())
                    {
                        Assert.IsTrue(actions.Contains(WorkflowAction.Approve),
                            "actions.Contains(WorkflowAction.Approve)");
                        Assert.IsTrue(actions.Contains(WorkflowAction.Reject), "actions.Contains(WorkflowAction.Reject)");
                    }
                    testIsRan = true;
                break;
                }


            if(!testIsRan) Assert.Inconclusive("There is no valid data to perform the test");
            }

        [TestMethod]
        [Ignore]
        public override void ActionsForEndorsedButHasNoAccessTest()
            {
            
            }

        [TestMethod]
        public override void ActionsForImportedTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var posRep = srv.PositionRepository();
            var posList = posRep.BaseList().Where(p => p.StatusId == (int)Enums.StatusValue.Imported);
            var admin = GetSampleUsers(Enums.UserRole.SystemAdministrator, factory, true, 1).FirstOrDefault();
            if(admin == null)
                {
                Assert.Inconclusive("There is no valid user role to perform the test");
                }
            var testIsRan = false;
            foreach(var pos in posList)
                {

                var workflowEngine = WorkflowEngineFactory.CreatEngine(pos, admin, factory);


                var actions = workflowEngine.GetWorkflowObjectActions();

                Assert.IsTrue(actions.Contains(WorkflowAction.Clone), "actions.Contains(WorkflowAction.Clone)");
                Assert.IsTrue(actions.Contains(WorkflowAction.BringToDraft), "actions.Contains(WorkflowAction.BringToDraft)");
                

                testIsRan = true;
                break;
                }


            if(!testIsRan) Assert.Inconclusive("There is no valid data to perform the test");
            }

        [TestMethod]
        [Ignore]
        public override void ActionsForImportedButHasNoAccessTest()
            {
            
            }

        [TestMethod]
        public override void ActionsForApprovedTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var posRep = srv.PositionRepository();
            var posList = posRep.BaseList().Where(p => p.StatusId == (int)Enums.StatusValue.Approved);
            var admin = GetSampleUsers(Enums.UserRole.SystemAdministrator, factory, true, 1).FirstOrDefault();
            if(admin == null)
                {
                Assert.Inconclusive("There is no valid user role to perform the test");
                }
            var testIsRan = false;
            foreach(var pos in posList)
                {

                var workflowEngine = WorkflowEngineFactory.CreatEngine(pos, admin, factory);


                var actions = workflowEngine.GetWorkflowObjectActions();

                Assert.IsTrue(actions.Contains(WorkflowAction.Clone), "actions.Contains(WorkflowAction.Clone)");
                Assert.IsTrue(actions.Contains(WorkflowAction.BringToDraft), "actions.Contains(WorkflowAction.BringToDraft)");
                testIsRan = true;
                break;
                }


            if(!testIsRan) Assert.Inconclusive("There is no valid data to perform the test");
            }

        [TestMethod]
        [Ignore]
        public override void ActionsForApprovedButHasNoAccessTest()
            {

            }

        }
    }

