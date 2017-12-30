using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL;
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
    public class DirectorateControllerTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }

        private DirectorateController GetController(out IRepositoryFactory repositoryFactory)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new DirectorateController(service);
            ctr.ControllerContext = new ControllerContext(context.Object, new RouteData(), ctr);
            repositoryFactory = factory;
            return ctr;
            }


        [TestMethod]
        public void ListJson()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var serviceReopsitory = new ServiceRepository(factory);
            var exSrv = serviceReopsitory.ExecutiveRepository();

            foreach(var ex in exSrv.List().ToArray())
                {
                var result = ctr.ListJson(new BasicStructureArgument { DivisionCode = ex.ExecutiveCod, SortColumnName = "DirectorateId", iDisplayLength = 10 }) as JsonResult;

                var jsResult = (result.Data as DataTableResult).aaData.ToList();
                if(jsResult.Any())
                    {
                    var directiorates = jsResult.Take(10).Cast<DirectorateLight>();

                    foreach(var directiorate in directiorates)
                        {
                        Console.WriteLine($"{directiorate.DirectorateId}-{directiorate.DirectorateName}");
                        }

                    Assert.IsTrue(directiorates.All(d => d.ExecutiveCod == ex.ExecutiveCod),
                        "directiorates.All(d=>d.ExecutiveCod==ex.ExecutiveCod)");
                    }

                }

            }

        [TestMethod]
        public void GetDirectorates()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var serviceReopsitory = new ServiceRepository(factory);
            var exSrv = serviceReopsitory.ExecutiveRepository();
            var positionRep = serviceReopsitory.PositionRepository();
            var positions = positionRep.CachedPositionListForChart();
            foreach (var ex in exSrv.List().ToArray())
            {
                var result = ctr.GetDirectorates(ex.ExecutiveCod, true) as JsonResult;
                var list = result.Data as IEnumerable<SelectListItemExtension>;

                foreach (var item in list)
                {
                    Console.WriteLine(item.Text + "" + positions.Count(p => p.Unit.BusinessUnit.DirectorateId == item.Value.ToInteger()));
                    Assert.IsFalse(item.Text.Contains(positions.Count(p=>p.Unit.BusinessUnit.DirectorateId != item.Value.ToInteger()).ToString()));
                    
                }
            }

            }
        }

    }

