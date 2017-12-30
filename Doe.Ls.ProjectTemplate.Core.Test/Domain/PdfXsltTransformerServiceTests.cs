using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.FileService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.PdfService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
    {
    [TestClass]
    public class PdfXsltTransformerServiceTests: TestBase
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
                serviceRep.PositionRepository().ListForPdRoleDesc().FirstOrDefault(
                    p => p.PositionId>10
                    && p.RolePositionDescription.IsPositionDescription &&( p.StatusId == (int)Enums.StatusValue.Approved || p.StatusId == (int)Enums.StatusValue.Imported));

            positionForPd = serviceRep.PositionRepository().GetPositionById(positionForPd.PositionId);


            var result = srv.GeneratePdf(positionForPd);
            Console.WriteLine(result.OutputFileName);
            Console.WriteLine(result.OutputFileFullPath);
            File.Delete(result.OutputFileFullPath);

            }

        [Ignore]
        [TestMethod]
        public void GenerateBulkPositions()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            factory.RegisterType<IPdfService, PdfXsltTransformer>();

            var serviceRep = new ServiceRepository(factory);

            var srv = serviceRep.PdfService();

            var positions =
                serviceRep.PositionRepository().ListForPdRoleDesc().Where(p => p.RolePositionDescription.IsPositionDescription && (p.StatusId == (int)Enums.StatusValue.Approved || p.StatusId == (int)Enums.StatusValue.Imported)).Take(50);

            foreach (var pos in positions)
            {
                var positionForPd = serviceRep.PositionRepository().GetPositionById(pos.PositionId);
                var result = srv.GeneratePdf(positionForPd);
                File.Copy(result.OutputFileFullPath,$@"C:\temp\pos_{pos.PositionId}.pdf");
                Console.WriteLine(result.OutputFileName);
                Console.WriteLine(result.OutputFileFullPath);
                File.Delete(result.OutputFileFullPath);
                }
         

            }


        //[TestMethod]
        //public void FixingBugA() {
        //    var factory = new MockRepositoryFactory();
        //    factory.RegisterAllDependencies();
        //    factory.RegisterType<IPdfService, PdfXsltTransformer>();

        //    var serviceRep = new ServiceRepository(factory);

        //    var srv = serviceRep.PdfService();

        //    var position =
        //        serviceRep.PositionRepository().GetPositionById(22348);

        //        if(position==null)Assert.Inconclusive();

        //        var positionForPd = serviceRep.PositionRepository().GetPositionById(position.PositionId);
        //        var result = srv.GeneratePdf(positionForPd);
        //        File.Copy(result.OutputFileFullPath, $@"C:\temp\pos_{position.PositionId}.pdf");
        //        Console.WriteLine(result.OutputFileName);
        //        Console.WriteLine(result.OutputFileFullPath);

        //    }


        [TestMethod]
        public void TestPositionXml()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            factory.RegisterType<IPdfService, PdfXsltTransformer>();

            var serviceRep = new ServiceRepository(factory);

            var customResult = serviceRep.PositionFocusCriteriaRepository().
                List()
                .Include(pfc => pfc.PositionDescription)
                .Include(pfc => pfc.PositionDescription.RolePositionDescription)
                .
                Where(
                    pfc =>
                        pfc.LookupCustomContent.Length > 0 &&
                        pfc.PositionDescription.RolePositionDescription.Positions.Any() &&
                        (pfc.PositionDescription.RolePositionDescription.StatusId == (int) Enums.StatusValue.Approved ||
                         pfc.PositionDescription.RolePositionDescription.StatusId == (int) Enums.StatusValue.Imported)).Distinct();


            int prevPosId = -1;

            foreach (var positionFocusCriteria in customResult.Take(20))
            {
                var position =
                    serviceRep
                        .RolePositionDescriptionRepository().List().Include(r=>r.Positions)
                        .First(r => r.RolePositionDescId== positionFocusCriteria.PositionDescriptionId).Positions.FirstOrDefault();

                if(position==null)continue;
                
                if(prevPosId==position.PositionId)continue;

                prevPosId = position.PositionId;
                var noOfSelectionCriterias =
                    serviceRep
                        .PositionFocusCriteriaRepository()
                        .List()
                        .Include(pfc => pfc.PositionDescription)
                        .Include(pfc => pfc.PositionDescription.RolePositionDescription).Count(pfc => pfc.PositionDescriptionId == position.RolePositionDescriptionId);

                var xmlPos = serviceRep.PositionRepository().XmlSerialize(position);
                var elementsCount = xmlPos.Descendants("Criteria").Count();
                Assert.AreEqual(elementsCount,noOfSelectionCriterias,$"No of criteria counts in xml are not equal in database for position {position}");
            }

            }

        [TestMethod]
        public void GeneratePosForRoleDescriptionTest1()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var serviceRep = new ServiceRepository(factory);

            var srv = serviceRep.PdfService();
            
            var positionForRd =
                serviceRep.PositionRepository().ListForPdRoleDesc().FirstOrDefault(p => !p.RolePositionDescription.IsPositionDescription &&( p.StatusId == (int)Enums.StatusValue.Approved || p.StatusId == (int)Enums.StatusValue.Imported));

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
