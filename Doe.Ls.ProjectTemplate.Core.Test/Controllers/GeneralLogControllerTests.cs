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
    public class GeneralLogControllerTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }

        private GeneralLogController GetController(out IRepositoryFactory repositoryFactory)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new GeneralLogController(service);
            ctr.ControllerContext = new ControllerContext(context.Object, new RouteData(), ctr);
            repositoryFactory = factory;
            return ctr;
            }

        [TestMethod]
        public void ListJsonSearch()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);

            var result =
                ctr.ListJson(new GeneralLogArgument
                    {
                    sSearch = "ed"
                    }) as JsonResult;

            var jsResult = (result.Data as DataTableResult).aaData.ToList();
            if(jsResult.Any())
                {
                var logs = jsResult.Take(10).Cast<GeneralLogLight>();

                foreach(var item in logs)
                    {
                    Console.WriteLine($"{item}");
                    }


                }
            else
                {
                Assert.Inconclusive("No results");
                }

            }

        [TestMethod]
        public void ListJsonDateRange()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var list = ctr.Repository.List();
            var srv = new ServiceRepository(ctr.Repository.RepositoryFactory);
            //srv.LogLinqSQLToConsole();
            
            var item1 = list.OrderBy(l => l.CreationDate).Skip(10).FirstOrDefault();
            var item2 = list.OrderBy(l => l.CreationDate).Skip(list.Count()-10).FirstOrDefault();
            var result =
                ctr.ListJson(new GeneralLogArgument
                    {
                    FromDate= item1==null?null: item1.CreationDate,
                    ToDate = item2 == null ? null : item2.CreationDate,

                    }) as JsonResult;

            var jsResult = (result.Data as DataTableResult).aaData.ToList();
            if(jsResult.Any())
                {
                var logs = jsResult.Take(10).Cast<GeneralLogLight>();

                if(item1!=null)  Assert.IsTrue(logs.All(l => l.CreationDate >= item1.CreationDate), "logs.All(l => l.CreationDate >= item1.CreationDate)");
                if(item2 != null) Assert.IsTrue(logs.All(l => l.CreationDate <= item2.CreationDate), "logs.All(l => l.CreationDate >= item2.CreationDate)");

                foreach(var item in logs)
                    {
                    Console.WriteLine($"{item}");
                    }

                }
            else
                {
                Assert.Inconclusive("No results");
                }

            }


        [TestMethod]
        public void ListJsonAction()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var srv = new ServiceRepository(ctr.Repository.RepositoryFactory);
            var uw = srv.GetUnitOfWork();
            uw.DbContext.Database.Log = Console.WriteLine;

            var action = Enums.LogActions.LogIn.GetDescription();
            var result =
                ctr.ListJson(new GeneralLogArgument
                    {
                    LogAction = action
                    }) as JsonResult;

            var jsResult = (result.Data as DataTableResult).aaData.ToList();
            if(jsResult.Any())
                {
                var logs = jsResult.Take(10).Cast<GeneralLogLight>();
                Assert.IsTrue(logs.All(l => l.Action == action), "logs.All(l=>l.Action== action)");
                foreach(var item in logs)
                    {
                    Console.WriteLine($"{item}");
                    }


                }
            else
                {
                Assert.Inconclusive("No results");
                }

            }

        }



    }

