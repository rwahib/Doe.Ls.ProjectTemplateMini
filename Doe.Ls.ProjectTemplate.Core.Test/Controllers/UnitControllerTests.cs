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
    public class UnitControllerTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }

        private UnitController GetController(out IRepositoryFactory repositoryFactory)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new UnitController(service);
            ctr.ControllerContext = new ControllerContext(context.Object, new RouteData(), ctr);
            repositoryFactory = factory;
            return ctr;
            }

       
        [TestMethod]
        public void ListJsonDiv()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var serviceReopsitory = new ServiceRepository(factory);
            var divSrv = serviceReopsitory.ExecutiveRepository();

            foreach(var div in divSrv.List().Take(2).ToArray())
                {
                var result = ctr.ListJson(new BasicStructureArgument {DivisionCode = div.ExecutiveCod, SortColumnName = "ExecutiveCod",iDisplayLength = 10}) as JsonResult;

                var jsResult = (result.Data as DataTableResult).aaData.ToList();
                if(jsResult.Any())
                    {
                    var teams = jsResult.Take(10).Cast<UnitLight>();

                        foreach(var team in teams)
                        {
                        Console.WriteLine($"{team}");
                        }

                    Assert.IsTrue(teams.All(t => t.ExecutiveCode == div.ExecutiveCod),"teams.All(t => t.ExecutiveCode == div.ExecutiveCod)");
                    }

                }

            }
        [TestMethod]
        public void ListJsonDivDir()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var serviceReopsitory = new ServiceRepository(factory);
            var dirSrv = serviceReopsitory.DirectorateRepository();

            foreach(var dir in dirSrv.List().Take(3).ToArray())
                {
                var result = ctr.ListJson(new BasicStructureArgument { DivisionCode = dir.ExecutiveCod,DirectorateId =dir.DirectorateId, SortColumnName = "ExecutiveCod", iDisplayLength = 10 }) as JsonResult;

                var jsResult = (result.Data as DataTableResult).aaData.ToList();
                if(jsResult.Any())
                    {
                    var teams = jsResult.Take(10).Cast<UnitLight>();

                    foreach(var team in teams)
                        {
                        Console.WriteLine($"{team}");
                        }

                    Assert.IsTrue(teams.All(t => t.DirectorateId == dir.DirectorateId),"teams.All(t => t.DirectorateId == dir.DirectorateId)");
                    Assert.IsTrue(teams.All(t => t.ExecutiveCode == dir.ExecutiveCod),"teams.All(t => t.ExecutiveCode == dir.ExecutiveCod)");
                    }

                }

            }

        [TestMethod]
        public void ListJsonDivBu()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var serviceReopsitory = new ServiceRepository(factory);
            var buSrv = serviceReopsitory.BusinessUnitRepository();

            foreach(var bUnit in buSrv.List().Take(10).ToArray())
                {
                var result = ctr.ListJson(new BasicStructureArgument { DivisionCode = bUnit.Directorate.ExecutiveCod, DirectorateId = bUnit.DirectorateId, BusinessUnitId = bUnit.BUnitId,SortColumnName = "ExecutiveCod", iDisplayLength = 10 }) as JsonResult;

                var jsResult = (result.Data as DataTableResult).aaData.ToList();
                if(jsResult.Any())
                    {
                    var teams = jsResult.Take(10).Cast<UnitLight>();                    
                    foreach(var team in teams)
                        {
                        Console.WriteLine($"{team}");
                        }

                    Assert.IsTrue(teams.All(t => t.BUnitId == bUnit.BUnitId), "teams.All(t => t.BUnitId == bUnit.BUnitId)");
                    Assert.IsTrue(teams.All(t => t.DirectorateId == bUnit.DirectorateId), "teams.All(t => t.DirectorateId == dir.DirectorateId)");
                    Assert.IsTrue(teams.All(t => t.ExecutiveCode == bUnit.Directorate.ExecutiveCod), "teams.All(t => t.ExecutiveCode == dir.ExecutiveCod)");
                    
                    }

                }

            }

        [TestMethod]
        public void GetBusinessUnits()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var serviceReopsitory = new ServiceRepository(factory);
            var dirSrv = serviceReopsitory.DirectorateRepository();
            var positionRep = serviceReopsitory.PositionRepository();
            var positions = positionRep.CachedPositionListForChart();
            foreach (var dir in dirSrv.List().ToArray())
            {
                var result = ctr.GetUnits(dir.DirectorateId, true) as JsonResult;
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

