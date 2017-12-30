/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Models;

using Doe.Ls.SchoolSportsUnit.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static System.Console;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Newtonsoft.Json;

namespace Doe.Ls.SchoolSportsUnit.Core.Test.Controllers
{
    [TestClass]
    public class AppApiControllerTests : TestBase
    {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {

        }

        private AppApiController GetController(ref IRepositoryFactory repositoryFactory)
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new AppApiController(service);
            ctr.ControllerContext = new ControllerContext(context.Object, new RouteData(), ctr);
            repositoryFactory = factory;
            return ctr;
        }


        [TestMethod]
        public void GetAssetTags()
        {
            IRepositoryFactory factory = null;
            var ctr = GetController(ref factory);
            var serviceReopsitory = new ServiceRepository(factory);
            var assocService = serviceReopsitory.AssociationRepository();
            SetCurrentUserAtController(ctr, Enums.UserRole.SystemAdministrator);

            var tags = new string[] {"for", "ba", "bo", "on", "app"};
            var assoclist = assocService.List().Take(5);
            foreach (var association in assoclist)
            {

                foreach (var term in tags)
                {
                    var result = ctr.GetAssetTags(term, association.AssociationId) as JsonResult;
                    var data = (result.Data as Result).Data as IEnumerable;
                    foreach (string tagResult in data)
                    {
                        WriteLine(
                            $"For term '{term}' ==>> '{tagResult}' for association ==>> {association.AssociationId}-{association.AssociationName}");

                        Assert.IsTrue(tagResult.Contains(term), "tagResult.Contains(term)");
                    }
                }

            }
        }



        [TestMethod]
        public void GetImageList()
        {
            IRepositoryFactory factory = null;
            var ctr = GetController(ref factory);
            


            SetCurrentUserAtController(ctr, Enums.UserRole.SsuAdministrator);

            var result = ctr.GetImageList() as JsonResult;
            var jsonStr = result.Data.JsonSerialise(Formatting.Indented);
            var jsonObj = jsonStr.JsonDeserialise<dynamic>();
            var counter = 0;
            foreach (var item in jsonObj)
            {
                Console.WriteLine($"{item.title}-{item.value}");
                if (++counter > 10) break;
            }

            SetCurrentUserAtController(ctr, Enums.UserRole.AssociationAdministrator,Enums.Association.HUN);

            result = ctr.GetImageList() as JsonResult;
            jsonStr = result.Data.JsonSerialise(Formatting.Indented);
            jsonObj = jsonStr.JsonDeserialise<dynamic>();
            counter = 0;
            foreach(var item in jsonObj)
                {
                Console.WriteLine($"{item.title}-{item.value}");
                if(++counter > 10) break;
                }

            }
        [TestMethod]
        public void GetVenueList_ReturnsVenues_BeginningWithTBA_WhenSearchStringFindsTBA()
        {
            IRepositoryFactory factory = null;
            var ctr = GetController(ref factory);
            SetCurrentUserAtController(ctr, Enums.UserRole.SystemAdministrator);
            var result = ctr.GetVenueList("tb") as JsonResult;
            var venues = ((Result)result.Data).Data as List<VenueLight>;
            Assert.AreEqual(Enums.Constants.TBA, venues.First().VenueName);
            result = ctr.GetVenueList("tba") as JsonResult;
            venues = ((Result)result.Data).Data as List<VenueLight>;
            Assert.AreEqual(Enums.Constants.TBA, venues.First().VenueName, "First venue name in list is not 'TBA'");
        }
    }

}
*/
