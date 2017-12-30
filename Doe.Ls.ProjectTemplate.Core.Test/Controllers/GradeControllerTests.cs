using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
    public class GradeControllerTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }

        private GradeController GetController(out IRepositoryFactory repositoryFactory)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new GradeController(service);
            ctr.ControllerContext = new ControllerContext(context.Object, new RouteData(), ctr);
            repositoryFactory = factory;
            return ctr;
            }

        [TestMethod]
        public void ListJsonStatus()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);

            var result =
                ctr.ListJson(new BasicStructureArgument
                    {
                    StatusCode = new string[] { $"{Enums.StatusValue.Active.ToInteger()},{Enums.StatusValue.Inactive.ToInteger()}" }
                    }) as JsonResult;

            var jsResult = (result.Data as DataTableResult).aaData.ToList();
            if(jsResult.Any())
                {
                var grades = jsResult.Take(10).Cast<GradeLight>();

                foreach(var grade in grades)
                    {
                    Console.WriteLine($"{grade}");
                    }

                Assert.IsTrue(
                    grades.All(
                        g =>
                            g.StatusId == (int)Enums.StatusValue.Active ||
                            g.StatusId == (int)Enums.StatusValue.Inactive),
                    "grades.All(g => g.StatusId == (int)Enums.StatusValue.Active|| g.StatusId == (int)Enums.StatusValue.InActive)");
                }
            else
                {
                Assert.Inconclusive("No results");
                }

            }


        [TestMethod]
        public void ListJsonGradeType()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);

            var result =
                    ctr.ListJson(new BasicStructureArgument
                        {
                        GradeType = Enums.GradeType.NSBTS.ToString()
                        }) as JsonResult;

            var jsResult = (result.Data as DataTableResult).aaData.ToList();
            if(jsResult.Any())
                {
                var grades = jsResult.Take(10).Cast<GradeLight>();

                foreach(var grade in grades)
                    {
                    Console.WriteLine($"{grade}");
                    }
                    var cat = Enums.GradeType.NSBTS.ToString();
                    Assert.IsTrue(
                        grades.All(g => g.GradeType == cat), "grades.All(g =>g.GradeType== cat)");
                }
            else
                {
                Assert.Inconclusive("No results");
                }

            }


        }



    }

