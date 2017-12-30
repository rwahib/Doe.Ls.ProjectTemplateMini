using System.Collections.Specialized;
using System.Configuration;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
    {
    /// <summary>
    /// This class for any R and D code
    /// </summary>
    [TestClass]
    public class PositionEstablishmentSettingsTest : TestBase
        {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }

        [TestMethod]
        public void AppUrl()
        {
            var fixedUrl = ProjectTemplateSettings.Site.FixedUrl;
            Assert.IsTrue(fixedUrl|| fixedUrl==false,"fixedUrl==true || fixedUrl==false");

            if (ProjectTemplateSettings.Site.FixedUrl)
            {
                Assert.IsTrue(ProjectTemplateSettings.Site.AppUrl== (ConfigurationManager.GetSection(@"site") as NameValueCollection)["AppUrl"].Trim());

            }


        }


        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();
            }

        }
    }

