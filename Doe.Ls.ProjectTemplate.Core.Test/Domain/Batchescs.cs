using System;
using System.IO;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
{
    [TestClass]
    //[Ignore]
     public class Batches : TestBase
    {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }

        [TestMethod]
        [Ignore]
        public void UpdateUrls()
        {
            var filePath = @"C:\Projects\LS\Workspaces\Doe.Ls.PositionDescriptions\Dev-branch\Doe.Ls.ProjectTemplate.Core\BL\UI\Dashboards\DashboardItem.cs";
            var snippet = "?cache_id={AppCacheHelper.Token}\",";
            var lines = File.ReadAllLines(filePath);
            var sb = new StringBuilder();
            foreach(var line in lines)
                {
                var urlLine = line.Trim();
                if(urlLine.StartsWith("Url =")|| urlLine.StartsWith("Url=")){
                    if(!urlLine.Contains("AppCacheHelper.Token"))
                        {
                        urlLine = urlLine.Replace("\",", snippet);
                        sb.Append($"{urlLine}\n");
                        }
                    else
                        {
                        
                        sb.Append($"{line}\n");
                        }
                    }else
                    {
                    sb.Append($"{line}\n");                    
                    }


                }
            var newPath = filePath.Replace("DashboardItem.cs", "DashboardItem_new.cs");
            File.WriteAllText(newPath, sb.ToString(),Encoding.Unicode);
            
        }
        
    }

}

