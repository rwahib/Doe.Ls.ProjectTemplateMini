using System;
using System.IO;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
{
    [TestClass]
    public class PrivilegeTests : TestBase
    {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }

        [TestMethod]
        public void Operations()
        {
            var priv1 = Enums.Privilege.ReadOnlyPrivilege;
            var priv2 = Enums.Privilege.AccessDeniedPrivilege;

            Assert.IsTrue(priv1!=priv2,"priv1==priv2");
            Assert.IsFalse(priv1==priv2,"priv1==priv2");

            Assert.IsTrue(priv1 != null, "priv1 != null");
            Assert.IsTrue(null != priv2, "null != priv2");


             priv1 = Enums.Privilege.AccessDeniedPrivilege;
             priv2 = Enums.Privilege.AccessDeniedPrivilege;

            Assert.IsTrue(priv1 == priv2, "priv1==priv2");
            Assert.IsFalse(priv1 != priv2, "priv1==priv2");

            priv1 = Enums.Privilege.AccessDeniedPrivilege;
            priv2 = priv1+ Enums.Privilege.ModifyPrivilege;

            Assert.IsTrue(priv2.CanRead, "priv2.CanRead");
            Assert.IsTrue(priv2.CanEdit, "priv2.CanEdit");


            priv1 = Enums.Privilege.ModifyPrivilege;
            priv2 = priv1 - Enums.Privilege.ReadOnlyPrivilege;

            Assert.IsFalse(priv2.CanRead, "priv2.CanRead");

            priv1 = Enums.Privilege.ReadOnlyPrivilege;
            priv2 = priv1 - Enums.Privilege.ModifyPrivilege;

            Assert.IsFalse(priv2.CanRead, "priv2.CanRead");

            }

        [TestCleanup]
        public void CleanUp()
        {
            

        }


    }

}

