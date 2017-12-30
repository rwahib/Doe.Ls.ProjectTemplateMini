using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Doe.Ls.ProjectTemplate.Core.Test.Controllers
    {
    [TestClass]
    public class PositionDescriptionControllerTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }
        [TestMethod]
        public void ManageOverViewForCreation()
            {
            IRepositoryFactory factory = null;

            var ctr = GetController(out factory);
            var srv = new ServiceRepository(factory);
            var admin = GenerateSampleUser(Enums.UserRole.BusinessUnitAuthor, factory);

            srv.SessionService().AddToSession(Cnt.CurrentUserKey, admin);
            // create a form for new Position  description
            var result = ctr.ManageOverview(0) as ViewResult;

            Assert.IsNotNull(result, "result != null");

            var model = result.Model as PositionDescription;
            Assert.IsNotNull(model, "model != null");

            Assert.IsTrue(model.PositionDescriptionId == 0, "model.PositionDescriptionId==0");

            var formType = ViewBagWrapper.FormOperations.GetFormType(ctr.ViewData);
            Assert.IsTrue(formType == FormType.Create, "formType==FormType.Create");

            var wf = ViewBagWrapper.WorkflowBag.GetWorkflowEngine(ctr.ViewData);

            var priv = wf.GetWorkflowObjectPrivilege();
            var actions = wf.GetWorkflowObjectActions();

            Assert.IsTrue(priv.CanEdit, "priv.CanEdit");

            Assert.IsTrue(wf.Task.User.UserName == admin.UserName, "wf.Task.User.UserName== admin.UserName");

            Assert.IsTrue(wf.WorkflowObject.WorkflowObjectId == model.PositionDescriptionId, "wf.WorkflowObject.WorkflowObjectId==model.PositionDescriptionId");
            Assert.IsTrue(wf.WorkflowObject.WorkflowObjectStatus.StatusId == model.StatusValue.StatusId, "wf.WorkflowObject.WorkflowObjectStatus.StatusId==model.StatusValue.StatusId");



            }

        [TestMethod]
        public void ManageOverViewForUpdate()
            {
            IRepositoryFactory factory = null;

            var ctr = GetController(out factory);
            var srv = new ServiceRepository(factory);
            var admin = GenerateSampleUser(Enums.UserRole.BusinessUnitAuthor, factory);

            srv.SessionService().AddToSession(Cnt.CurrentUserKey, admin);
            // create a form for new Position  description
            var posSample =
                srv
                    .PositionDescriptionRepository()
                    .List()
                    .FirstOrDefault(pd => pd.RolePositionDescription.StatusId == (int)Enums.StatusValue.Draft);

            if(posSample == null)
                {
                Assert.Inconclusive("No sufficient data to berform the test");
                }
            var result = ctr.ManageOverview(posSample.PositionDescriptionId) as ViewResult;

            Assert.IsNotNull(result, "result != null");

            var model = result.Model as PositionDescription;
            Assert.IsNotNull(model, "model != null");

            Assert.IsTrue(model.PositionDescriptionId == posSample.PositionDescriptionId, "model.PositionDescriptionId == posSample.PositionDescriptionId");
            Assert.IsTrue(model.RolePositionDescription.Title == posSample.RolePositionDescription.Title, "model.RolePositionDescription.Title == posSample.RolePositionDescription.Title");

            var formType = ViewBagWrapper.FormOperations.GetFormType(ctr.ViewData);
            Assert.IsTrue(formType == FormType.Edit, "formType==FormType.Edit");


            var wf = ViewBagWrapper.WorkflowBag.GetWorkflowEngine(ctr.ViewData);

            var priv = wf.GetWorkflowObjectPrivilege();
            var actions = wf.GetWorkflowObjectActions();

            Assert.IsTrue(priv.CanEdit, "priv.CanEdit");
            if(!posSample.RolePositionDescription.Positions.HasAnyIncludeLive(Enums.StatusValue.Endorsed,
                Enums.StatusValue.Draft, Enums.StatusValue.Submitted))
                {
                Assert.IsTrue(actions.Contains(WorkflowAction.Delete), "actions.Contains(WorkflowAction.Delete)");
                }
            Assert.IsTrue(wf.Task.User.UserName == admin.UserName, "wf.Task.User.UserName== admin.UserName");

            Assert.IsTrue(wf.WorkflowObject.WorkflowObjectId == model.PositionDescriptionId, "wf.WorkflowObject.WorkflowObjectId==model.PositionDescriptionId");
            Assert.IsTrue(wf.WorkflowObject.WorkflowObjectTitle == model.RolePositionDescription.Title, "wf.WorkflowObject.WorkflowObjectTitle == model.RolePositionDescription.Title");

            Assert.IsTrue(wf.WorkflowObject.WorkflowObjectStatus.StatusId == model.StatusValue.StatusId, "wf.WorkflowObject.WorkflowObjectStatus.StatusId==model.StatusValue.StatusId");



            }
        [TestMethod]
        public void ListJson()
            {
            IRepositoryFactory factory = null;

            var ctr = GetController(out factory);
            var srv = new ServiceRepository(factory);

            var admin = GenerateSampleUser(Enums.UserRole.BusinessUnitAuthor, factory);

            srv.SessionService().AddToSession(Cnt.CurrentUserKey, admin);

            var param = new JQueryDataTableRolePositionDesc
                {
                StatusCode =
                    new[] { $"{Enums.StatusValue.Imported.ToInteger()}, {Enums.StatusValue.Approved.ToInteger()}" },
                iDisplayLength = 10
                };
            var result =
                ctr.ListJson(
              param
                    ) as JsonResult;

            var jsResult = (result.Data as DataTableResult).aaData.ToList();
            if(jsResult.Any())
                {
                var pds = jsResult.Take(10).Cast<PositionDescriptionLight>();

                foreach(var pd in pds)
                    {
                    Console.WriteLine($"{pd}");
                    }
                Assert.IsTrue(pds.All(g => g.StatusValue.ToLower().Contains(Enums.StatusValue.Imported.ToString().ToLower()) || g.StatusValue.ToLower().Contains(Enums.StatusValue.Approved.ToString().ToLower())), "pds.All(g =>g.StatusValue.ToLower().Contains(Enums.StatusValue.Imported.ToString().ToLower()) || g.StatusValue.ToLower().Contains(Enums.StatusValue.Approved.ToString().ToLower()))");
                }
            else
                {
                Assert.Inconclusive("No results");
                }
            

            }
        [TestMethod]
        public void ListJsonEmtyStatusCode()
            {
            IRepositoryFactory factory = null;

            var ctr = GetController(out factory);
            var srv = new ServiceRepository(factory);

            var admin = GenerateSampleUser(Enums.UserRole.BusinessUnitAuthor, factory);

            srv.SessionService().AddToSession(Cnt.CurrentUserKey, admin);

            var param = new JQueryDataTableRolePositionDesc
                {
                StatusCode =new string[] { "" },
                iDisplayLength = 10
                };
         

 
           var result =
               ctr.ListJson(
             param
                   ) as JsonResult;

            var jsResult = (result.Data as DataTableResult).aaData.ToList();

            Assert.IsTrue(jsResult.Any(), "jsResult.Any()");

            }


        [TestMethod]
        public void ListJsonForDoe()
            {
            IRepositoryFactory factory = null;

            var ctr = GetController(out factory);
            var srv = new ServiceRepository(factory);

            var doeUser = GenerateSampleUser(Enums.UserRole.DoEUser, factory);

            srv.SessionService().AddToSession(Cnt.CurrentUserKey, doeUser);

            var param = new JQueryDataTableRolePositionDesc
                {
                StatusCode = new string[] {$"{Enums.StatusValue.Draft.ToInteger()},{Enums.StatusValue.Deleted.ToInteger()},{Enums.StatusValue.Endorsed.ToInteger()},{Enums.StatusValue.Submitted.ToInteger()}" },
                iDisplayLength = 20
                };

            
            var result =
                ctr.ListJson(
              param
                    ) as JsonResult;

            var jsResult = (result.Data as DataTableResult).aaData.ToList();

            Assert.IsFalse(jsResult.Any(), "DoE Users should not access any non live records");

            }
        private PositionDescriptionController GetController(out IRepositoryFactory repositoryFactory)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new PositionDescriptionController(service);
            ctr.ControllerContext = new ControllerContext(context.Object, new RouteData(), ctr);
            repositoryFactory = factory;
            return ctr;
            }

        }

    }

