using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
{

    [TestClass]
    public class DataDrivienUnitTests 
    {
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
            //var data=AppDomain.CurrentDomain.SetData("DataDirectory ");

        }

        
        [Ignore]
        [TestMethod]
        [DeploymentItem("\\App_Data\\Datasource\\TestData.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            "c:\\temp\\TestData.xml",
            "TimeOffTest_ApplyTimeOffTest", DataAccessMethod.Sequential)]
        public void TestDataDriven()
        {
            
            var userId = int.Parse(TestContext.DataRow["UserID"].ToString());
            var startDate = DateTime.Parse(TestContext.DataRow["StartDate"].ToString());
            var endDate = DateTime.Parse(TestContext.DataRow["EndDate"].ToString());

            Console.WriteLine($"{userId}-{startDate}-{endDate}");

            Assert.IsTrue(userId>2,"userId>2");
        }

    }

}
