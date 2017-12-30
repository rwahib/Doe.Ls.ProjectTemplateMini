using System;
using System.IO;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
{
    [TestClass]
    [Ignore]
    public class FileServiceTest : TestBase
    {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }

        [TestMethod]
        public void Save()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gRep = new ServiceRepository(factory);
            var fService = gRep.FileService();

            var sourcePath = (fService.FileResolver as MockupFileResolver).GetSourceAssetPath("test1.pdf");

         
            
            var fi = new FileInfo(sourcePath);
            var destinationFile = fService.FileResolver.GetAssetFullFilePath("test1.pdf");
            var destinationFolderPath = Path.GetDirectoryName(destinationFile);
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            if (fService.AssetExists("test1.pdf"))
            {
                fService.DeleteAssetFile("test1.pdf");
            }

            var result = fService.SaveAssetFileAs(fi, "test1.pdf", false);

            var exists = fService.AssetExists(result);

            Assert.IsTrue(exists, "exists");

            fService.DeleteAssetFile("test1.pdf");

        }

        [TestMethod]
        [Ignore]
        public void Save2()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gRep = new ServiceRepository(factory);
            var fService = gRep.FileService();


            var sourcePath = (fService.FileResolver as MockupFileResolver).GetSourceAssetPath("test1.pdf");
            var destinationFile = fService.FileResolver.GetAssetFullFilePath("test1.pdf");
            var destinationFolderPath = Path.GetDirectoryName(destinationFile);

            

            var fi = new FileInfo(sourcePath);
            
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            if (fService.AssetExists("test1.pdf"))
            {
                fService.DeleteAssetFile("test1.pdf");
            }

            var result = fService.SaveAssetFileAs(fi, "test1.pdf", false);
            var exists = fService.AssetExists(result);
            Assert.IsTrue(exists, "exists");

            var fileName = Path.GetFileName(result);

            Console.WriteLine(fileName);

            var result2 = fService.SaveAssetFileAs(fi, "test1.pdf", false);
            exists = fService.AssetExists(result2);
            Assert.IsTrue(exists, "exists");

            var fileName2 = Path.GetFileName(result2);
            Console.WriteLine(fileName2);
            Assert.IsTrue(fileName2!=fileName,"fileName2!=fileName");

            fService.DeleteAssetFile(result);
            fService.DeleteAssetFile(result2);

        }


        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();

        }


    }

}

