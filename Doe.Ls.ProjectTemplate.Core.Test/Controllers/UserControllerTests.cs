using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
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
    public class UserrControllerTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }

        private UserController GetController(out IRepositoryFactory repositoryFactory)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new UserController(service);
            ctr.ControllerContext = new ControllerContext(context.Object, new RouteData(), ctr);
            repositoryFactory = factory;
            return ctr;
            }

        [TestMethod]
        public void ListJsonStatus()
            {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var userList = new List<string>();

            userList.AddRange(GetSampleUsers(Enums.UserRole.DirectorateDataEntry, factory, true, 3).Select(su => su.UserName));
            userList.AddRange(GetSampleUsers(Enums.UserRole.Administrator, factory, true, 3).Select(su => su.UserName));
            userList.AddRange(GetSampleUsers(Enums.UserRole.BusinessUnitDataEntry, factory, true, 3).Select(su => su.UserName));

            if(!userList.Any())
                {
                Assert.Inconclusive("There are not enough data");
                }

            foreach(var userName in userList)
                {
                var result =
             ctr.ListJson(new JQueryDataTableSysUser
                 {
                 sSearch = userName

                 }) as JsonResult;

                var jsResult = (result.Data as DataTableResult).aaData.ToList();
                if(jsResult.Any())
                    {
                    var userInfoList = jsResult.Take(10).Cast<UserInfoExtensionLight>();                        
                    foreach(var userInfo in userInfoList)
                        {
                        Console.WriteLine($"{userInfo}");
                        }

                        foreach (var userInfo in userInfoList)
                        {
                            Assert.IsTrue(userInfo.UserName.Equals(userName,StringComparison.CurrentCultureIgnoreCase) ||
                                userInfo.Email.Equals(userName, StringComparison.CurrentCultureIgnoreCase)
                                );
                        }
                    
                    }
                else
                    {
                    Assert.Inconclusive("No results");
                    }
                }



            }



        }



    }

