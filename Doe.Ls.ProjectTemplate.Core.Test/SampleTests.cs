using System;
using System.Collections.Generic;
using System.IO;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test
    {
    [TestClass]
    public class SampleTests : TestBase
        {
        [TestMethod]
        [Ignore]
        public void CopyFileTest()
            {
            try
                {
                var fileName = "test2.pdf";
                var sourcePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\TestFiles"));
                var destinationPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\OutputFiles"));

                var sourceFile = Path.Combine(sourcePath, fileName);
                var destFile = Path.Combine(destinationPath, fileName);

                Console.WriteLine("Source File: {0}", sourceFile);
                Console.WriteLine("Destination File: {0}", destFile);

                Console.WriteLine("Checking Destination Directory...");
                if(!Directory.Exists(destinationPath))
                    {
                    Directory.CreateDirectory(destinationPath);
                    Console.WriteLine("Destination Directory now created...");
                    }

                Console.WriteLine("Copying file...");
                File.Copy(sourceFile, destFile, true);
                Console.WriteLine("Copy complete...");

                Console.WriteLine("Removing file...");
                File.Delete(destFile);
                Console.WriteLine("File remove complete...");

                Console.WriteLine("Removing Directory...");
                Directory.Delete(destinationPath);
                Console.WriteLine("Directory remove complete...");
                }
            catch(Exception ex)
                {
                Assert.Fail("test fails due to " + ex.Message);
                }
            }

        [TestMethod]
        public void TextTests()
            {

            var list = new List<string>
            {
                "Geoffrey Test",
                "TinyMce Accessibility",
                "Hunter Trial/Championship Information"
            };

            foreach(var item in list)
                {
                Console.WriteLine(item.CleanFilename().ToUpper());
                }


            }

        [TestMethod]
        public void TestEnums()
        {
            
        }

        [TestMethod]
        public void Condition()
            {

#if DEV
                  Console.WriteLine("FROM DEV");
#endif


#if UAT
            Console.WriteLine("FROM UAT");
#endif
            }
        }
    }
