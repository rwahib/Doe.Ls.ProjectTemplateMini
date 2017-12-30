using System;
using System.Linq;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
{
    [TestClass]
    public class EntityBaseHelperTests : TestBase
    {

        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }

        [TestMethod]
        public void UpdateSignature()
        {
            var srv = GetServiceRepository(new UserInfoExtension
            {
                UserName = "firstname.lastname",
                

            });

            var position = new Position
            {

            };

       var result=     position.UpdateSignature(srv.RepositoryFactory);
            foreach (var prop in result)
            {
                Console.WriteLine(prop);
            }

            Assert.IsTrue(result.Contains("CreatedDate"),"result.Contains('CreatedDate')");
            Assert.IsTrue(result.Contains("LastModifiedDate"), "result.Contains('LastModifiedDate')");

            Assert.IsTrue(result.Contains("CreatedBy"), "result.Contains('CreatedBy')");
            Assert.IsTrue(result.Contains("LastModifiedBy"), "result.Contains('LastModifiedBy')");

            
            Assert.IsTrue(position.CreatedDate>=DateTime.Now.AddMinutes(-1),"position.CreatedDate>=DateTime.Now.AddMinutes(-1)");
            Assert.IsTrue(position.LastModifiedDate>=DateTime.Now.AddMinutes(-1), "position.LastModifiedDate>=DateTime.Now.AddMinutes(-1)");

            Assert.IsTrue(position.CreatedBy== "firstname.lastname", "position.CreatedBy== 'firstname.lastname'");
            Assert.IsTrue(position.LastModifiedBy== "firstname.lastname", "position.LastModifiedBy== 'firstname.lastname'");
            

            }

        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();

        }




    }
}
