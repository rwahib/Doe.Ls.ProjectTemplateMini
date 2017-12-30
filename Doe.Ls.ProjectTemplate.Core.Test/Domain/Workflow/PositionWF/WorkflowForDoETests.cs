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
   // [Ignore]
    public class WorkflowForDoETests : WorkflowBaseTest
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

        
        public override void ActionsForDraftTest()
        {
            throw new NotImplementedException();
        }

        public override void ActionsForDraftButHasNoAccessTest()
        {
            throw new NotImplementedException();
        }

        public override void ActionsForSubmittedTest()
        {
            throw new NotImplementedException();
        }

        public override void ActionsForSubmittedButHasNoAccessTest()
        {
            throw new NotImplementedException();
        }

        public override void ActionsForEndorsedTest()
        {
            throw new NotImplementedException();
        }

        public override void ActionsForEndorsedButHasNoAccessTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public override void ActionsForImportedTest()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var posRep = srv.PositionRepository();
            var posList = posRep.List().Where(p => p.StatusId == (int)Enums.StatusValue.Imported);
            var admin = GetSampleUsers(Enums.UserRole.DoEUser, factory, true, 1).FirstOrDefault();
            if(admin == null)
                {
                Assert.Inconclusive("There is no valid user role to perform the test");
                }
            var testIsRan = false;
            foreach(var pos in posList)
                {

                var workflowEngine = WorkflowEngineFactory.CreatEngine(pos, admin, factory);

                var actions = workflowEngine.GetWorkflowObjectActions();
                Assert.IsTrue(!actions.Any(),"!actions.Any()");
                testIsRan = true;
                break;
                }


            if(!testIsRan) Assert.Inconclusive("There is no valid data to perform the test");
            }

        public override void ActionsForImportedButHasNoAccessTest()
        {
            throw new NotImplementedException();
        }

        public override void ActionsForApprovedTest()
        {
            throw new NotImplementedException();
        }

        public override void ActionsForApprovedButHasNoAccessTest()
        {
            throw new NotImplementedException();
        }
        }
    }

