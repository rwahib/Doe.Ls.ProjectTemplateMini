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
    public class UserRoleControllerTests : SecurityBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }

        private UserRoleController GetController(out IRepositoryFactory repositoryFactory)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);

            var request = new Mock<HttpRequestBase>();


            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var ctr = new UserRoleController(service);
            ctr.ControllerContext = new ControllerContext(context.Object, new RouteData(), ctr);
            repositoryFactory = factory;
            return ctr;
            }


        [TestMethod]
        public void EditTest()
        {
            IRepositoryFactory factory = null;
            var ctr = GetController(out factory);
            var srv=new ServiceRepository(factory);
            var roleId = Enums.UserRole.BusinessUnitAuthor.ToInteger();
            var usrGrpList = ctr.Repository.List().Where(ur => ur.RoleId == roleId).GroupBy(ur => ur.UserId);

            var userWithOneBu = usrGrpList.First(g => g.Count() == 1);
            if (userWithOneBu == null)
            {
                Assert.Inconclusive("There is no enough data to test");
            }
            var sysUserRole = userWithOneBu.Single();
            var model = new UserRoleModel
            {
                UserId = sysUserRole.UserId,
                RoleId = sysUserRole.RoleId,
                StructureId = sysUserRole.StructureId,
                OrgLevelId = sysUserRole.OrgLevelId,                               
            };

            sysUserRole = ctr.Repository.GetSysUserRoleByModel(model);
            model = sysUserRole.ToUserOrgLevelObject(srv);

            Console.WriteLine(model);

            var oldStructure = model.StructureId;
            var oldStructInt = int.Parse(oldStructure);
            model.StructureId =
                srv.BusinessUnitRepository().List().First(bu => bu.BUnitId != oldStructInt).BUnitId.ToString();


            ctr.Repository.UpdateWithNewStructure(model);

            var newSysUser = ctr.Repository.GetSysUserRoleByModel(model);
            Assert.IsNotNull(newSysUser);
            Assert.IsTrue(newSysUser.StructureId== model.StructureId,"newSysUser.StructureId== model.StructureId");

            newSysUser.StructureId = oldStructure;
            var newModel = newSysUser.ToUserOrgLevelObject(srv);


            //ctr.Repository.UpdateWithNewStructure(newModel);

            // newSysUser = ctr.Repository.GetSysUserRoleByModel(newModel);

            //Assert.IsNotNull(newSysUser);
            //Assert.IsTrue(newSysUser.StructureId == oldStructure, "newSysUser.StructureId == oldStructure");

            }



        }



    }

