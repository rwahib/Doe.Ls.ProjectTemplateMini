using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.PdfService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
    {
    [TestClass]
    public class PdfServiceTests : TestBase
        {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }

        [TestMethod]
        public void GeneratePosForposDescriptionTest1()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            

            var serviceRep = new ServiceRepository(factory);

            var srv = serviceRep.PdfService();

            var positionForPd =
                serviceRep.PositionRepository().List().FirstOrDefault(p =>p.RolePositionDescription.IsPositionDescription&&p.PositionId!=-1&&p.RolePositionDescriptionId!=-1&& p.RolePositionDescription.IsPositionDescription && (p.StatusId == (int)Enums.StatusValue.Approved || p.StatusId == (int)Enums.StatusValue.Imported));

            positionForPd = serviceRep.PositionRepository().GetPositionById(positionForPd.PositionId);


            var result = srv.GeneratePdf(positionForPd);
            Console.WriteLine(result.OutputFileName);
            Console.WriteLine(result.OutputFileFullPath);
            File.Delete(result.OutputFileFullPath);

            }


        [TestMethod]
        public void GeneratePosForRoleDescriptionTest1()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var serviceRep = new ServiceRepository(factory);

            var srv = serviceRep.PdfService();
            
            var positionForRd =
                serviceRep.PositionRepository().ListForPdRoleDesc().FirstOrDefault(p => !p.RolePositionDescription.IsPositionDescription && p.PositionId != -1 && ( p.StatusId == (int)Enums.StatusValue.Approved || p.StatusId == (int)Enums.StatusValue.Imported));

            positionForRd = serviceRep.PositionRepository().GetPositionById(positionForRd.PositionId);
            
            var result = srv.GeneratePdf(positionForRd);
            Console.WriteLine(result.OutputFileName);
            Console.WriteLine(result.OutputFileFullPath);
            File.Delete(result.OutputFileFullPath);
            }


        [TestMethod]
        public void TestHtmlParseForPdfService()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var pdfSrv = srv.PdfService();
            var mockFileResolver = new MockupFileResolver();
            
            var testFile = mockFileResolver.GetAppDataFilePath("TestFile.txt");
            
            var cssText = "";
            var outputFile = mockFileResolver.GetAppDataFilePath("TestOutput.pdf");
            var htmlText = ReadFileToText(testFile);
           
            pdfSrv.GeneratePdfFromHtml(htmlText, cssText, outputFile);

            File.Delete(outputFile);
        }

        [TestMethod]
        public void TestChangeTemplate()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var pdfSrv = srv.PdfService();
            var mockFileResolver = new MockupFileResolver();

            var testFile = mockFileResolver.GetAppDataFilePath("TestFile2.txt");
            var cssFile = mockFileResolver.GetPdfTemplateFolderPath() + "PDFGenerator.css";

            var outputFile = mockFileResolver.GetAppDataFilePath("TestOutput.pdf");

            var htmlText = ReadFileToText(testFile);
            var cssText = ReadFileToText(cssFile);

            pdfSrv.GeneratePdfFromHtml(htmlText, cssText, outputFile);

            File.Delete(outputFile);
        }

        [TestMethod]
        public void PrintGenericPositionDescription() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var serviceRep = new ServiceRepository(factory);
            var posSrv = serviceRep.PositionRepository();


            var positionForPd =
                serviceRep.PositionDescriptionRepository().LiveList().Take(10);

            foreach (var positionDescription in positionForPd)
            {
                var pos=posSrv.GetGenericPositionForTrim(positionDescription.PositionDescriptionId);
                var result = serviceRep.PdfService().GeneratePdf(pos);
                Console.WriteLine(result.OutputFileName);
                Console.WriteLine(result.OutputFileFullPath);
                }
         

            }


        [TestMethod]
        public void PrintGenericRoleDescription() {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var serviceRep = new ServiceRepository(factory);
            var posSrv = serviceRep.PositionRepository();


            var rds =
                serviceRep.RoleDescriptionRepository().LiveList().Where(rd=>rd.RoleDescriptionId>-1).Take(10);

            foreach(var rd in rds) {
                var pos = posSrv.GetGenericPositionForTrim(rd.RoleDescriptionId);
                var result = serviceRep.PdfService().GeneratePdf(pos);
                Console.WriteLine(result.OutputFileName);
                Console.WriteLine(result.OutputFileFullPath);
                }


            }

        private string ReadFileToText(string fileName)
        {
            string contentText;
            using (var streamReader = new StreamReader(fileName, Encoding.UTF8))
            {
                contentText = streamReader.ReadToEnd();
            }
            return contentText;

        }
    }

    }
