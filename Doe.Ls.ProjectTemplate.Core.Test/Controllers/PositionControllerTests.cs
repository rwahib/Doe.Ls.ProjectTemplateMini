using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Light;
using Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Doe.Ls.ProjectTemplate.Core.Test.Controllers
    {
    [TestClass]
    public class PositionControllerTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }
        

        [TestMethod]        
        public void ListJson()
            {
            IRepositoryFactory factory = null;

            var ctr = GetController(out factory);
            var srv = new ServiceRepository(factory);

            var admin = GenerateSampleUser(Enums.UserRole.BusinessUnitAuthor, factory);

            srv.SessionService().AddToSession(Cnt.CurrentUserKey, admin);
            var result =
                ctr.ListJson(new JQueryDatatableParamPositionExtension()
                {
                    StatusId = (int)Enums.StatusValue.Imported,
                    iDisplayLength = 10
                }) as JsonResult;

            var jsResult = (result.Data as DataTableResult).aaData.ToList();
            if(jsResult.Any())
                {
                var positions = jsResult.Take(10).Cast<PositionLight>();

                foreach(var pos in positions)
                    {
                    Console.WriteLine($"{pos}");
                    }
                Assert.IsTrue(positions.All(g =>g.StatusId == (int)Enums.StatusValue.Imported),"positions.All(g =>g.StatusId == (int)Enums.StatusValue.Imported)");
                }
            else
                {
                Assert.Inconclusive("No results");
                }

            }


        private PositionController GetController(out IRepositoryFactory repositoryFactory)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new PositionController(service);
            ctr.ControllerContext = new ControllerContext(context.Object, new RouteData(), ctr);
            repositoryFactory = factory;
            return ctr;
            }

        }

    }

