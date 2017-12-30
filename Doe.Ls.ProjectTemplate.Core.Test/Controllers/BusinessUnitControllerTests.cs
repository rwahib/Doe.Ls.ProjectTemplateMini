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
    public class BusinessUnitControllerTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }

        private BusinessUnitController GetController(out IRepositoryFactory repositoryFactory)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new BusinessUnitController(service);
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
            var dirSrv = serviceReopsitory.DirectorateRepository();

            foreach(var dir in dirSrv.List().Take(10).ToArray())
                {
                var result = ctr.ListJson(new BasicStructureArgument { DirectorateId = dir.DirectorateId, SortColumnName = "DirectorateId" ,iDisplayLength = 10}) as JsonResult;

                var jsResult = (result.Data as DataTableResult).aaData.ToList();
                if(jsResult.Any())
                    {
                    var bUnites = jsResult.Take(10).Cast<BusinessUnitLight>();

                        foreach(var bUnit in bUnites)
                        {
                        Console.WriteLine($"{bUnit}");
                        }

                    Assert.IsTrue(bUnites.All(bu => bu.DirectorateId==dir.DirectorateId),
                        "");
                    }

                }

            }

        [TestMethod]
        public void ListJson2()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var serviceReopsitory = new ServiceRepository(factory);
            var execSrv = serviceReopsitory.ExecutiveRepository();

            foreach(var exec in execSrv.List().ToArray())
                {
                var result = ctr.ListJson(new BasicStructureArgument { DivisionCode = exec.ExecutiveCod, SortColumnName = "DirectorateId", iDisplayLength = 10 }) as JsonResult;

                var jsResult = (result.Data as DataTableResult).aaData.ToList();
                if(jsResult.Any())
                    {
                    var bUnites = jsResult.Take(10).Cast<BusinessUnitLight>();

                    foreach(var bUnit in bUnites)
                        {
                        Console.WriteLine($"{bUnit}");
                        }

                    Assert.IsTrue(bUnites.All(bu => bu.ExecutiveCode == exec.ExecutiveCod),"bUnites.All(bu => bu.ExecutiveCode == exec.ExecutiveCod)");
                    }

                }

            }
        [TestMethod]
        public void GetBusinessunits()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var serviceReopsitory = new ServiceRepository(factory);
            var dirSrv = serviceReopsitory.DirectorateRepository();
            var positionRep = serviceReopsitory.PositionRepository();
            var positions = positionRep.CachedPositionListForChart();
            foreach (var dir in dirSrv.List().ToArray())
            {
                var result = ctr.GetBusinessUnits(dir.DirectorateId, true) as JsonResult;
                var list = result.Data as IEnumerable<SelectListItemExtension>;

                foreach (var item in list)
                {
                    Console.WriteLine(item.Text + "" + positions.Count(p => p.Unit.BUnitId== item.Value.ToInteger()));
                    Assert.IsFalse(item.Text.Contains(positions.Count(p=>p.Unit.BusinessUnit.BUnitId != item.Value.ToInteger()).ToString()));
                    
                }
            }

            }
        }
    
        
    }

