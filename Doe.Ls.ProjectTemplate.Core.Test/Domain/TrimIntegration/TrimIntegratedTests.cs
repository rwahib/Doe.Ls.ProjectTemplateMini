using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule;
using Doe.Ls.TrimBase;
using HP.HPTRIM.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.TrimIntegration {
    [TestClass]
    public class TrimIntegratedTests : SecurityBase {
        [ClassInitialize]
        public static void Initialise(TestContext testContext) {
            if(!Settings.ProjectTemplateSettings.Site.TrimIntegration) {
                Assert.Inconclusive("Trim integration is not enabled");
                }
            }

        [TestMethod]
        public void GetRecordProperties() {
            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();

            var docNumber = GetSampleRecord().Number;

            var record = trimSrv.GetRecordByRecordNumber(docNumber, PropertyIds.FieldDefinitionExternalId);
            PrintRecord(record, true);

            var record2 = trimSrv.GetRecordByUri(record.Uri);
            PrintRecord(record2, true);

            }

        [TestMethod]
        public void UpdateRecords() {
            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();

            var docNumber = GetSampleRecord().Number;

            var record = trimSrv.GetRecordByRecordNumber(docNumber, PropertyIds.FieldDefinitionExternalId);
            var savedTitle = record.Title;


            record = trimSrv.UpdateRecord(record.Uri, new TrimPropVal(PropertyIds.RecordTitle, "Updated from unit test " + record.Title.Value));

            Assert.IsTrue(record.Title.Value.StartsWith("Updated from unit test"), "record.Title.Value.StartsWith('Updated from unit test')");
            record = trimSrv.UpdateRecord(record.Uri, new TrimPropVal(PropertyIds.RecordTitle, savedTitle));
            Assert.IsTrue(record.Title.Value.Equals(savedTitle), "record.Title.Value.Equals(savedTitle)");
            }


        [TestMethod]
        public void UpdateExternalField() {
            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();

            var docNumber = GetSampleRecord().Number;

            var record = trimSrv.GetRecordByRecordNumber(docNumber, PropertyIds.FieldDefinitionExternalId);



            record = trimSrv.UpdateRecord(record.Uri, new TrimPropVal(PropertyIds.FieldDefinitionExternalId, DateTime.Now));
            var result = trimSrv.GetRecordByUri(record.Uri, trimSrv.GetAllPropertyList().Take(5).ToArray());

            }


        [TestMethod]
        public void GetDocumentInfo() {

            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();
            var docNumber = GetSampleRecord().Number;
            var record = trimSrv.GetRecordByRecordNumber(docNumber, trimSrv.GetAllPropertyList().Take(5).ToArray());

            PrintRecord(record);

            }

        [TestMethod]
        public void GetFileInfo() {

            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();



            var record = GetSampleRecord();
            var tmpFolderName = Directory.CreateDirectory(Path.GetTempPath() + $"{Guid.NewGuid().ToString().Substring(0, 10).CleanPropertyName()}").FullName;

            var pdfSample = GetSampleFile("Sample-RD-1.pdf");
            if(!pdfSample.Exists) {
                throw new FileNotFoundException(pdfSample.FullName);
                }

            trimSrv.UploadDocumnet(record.Uri, pdfSample, UnitTestToken, Guid.NewGuid().ToString());

            var model = trimSrv.GetRecordModel(record.Uri);

            var fileName = trimSrv.GetFileInfo(record.Uri, 1000);


            }

        [TestMethod]
        public void GetRecordUri() {

            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();
            var result = trimSrv.GetRecordList("Doc", 20, 20);
            foreach(var record in result) {
                var startTimer = DateTime.Now;
                var uri = trimSrv.GetRecordUri(record.Number);
                var span = DateTime.Now - startTimer;
                Console.WriteLine($"Uri of Document number {record.Number} is {uri} it took {span}");
                Assert.AreEqual(uri.Value, record.Uri, "uri.Value,record.Uri");
                }

            }

        [TestMethod]
        public void GetRecordModel() {

            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();
            var result = new List<Record> { GetSampleRecord() };

            foreach(var record in result) {
                var timer = DateTime.Now;
                var model = trimSrv.GetRecordModel(record.Uri, true);
                var span = DateTime.Now - timer;
                Console.WriteLine($"Time span={span}");
                base.PrintDumpObject(model);
                }

            }
        [TestMethod]
        public void GetRecordPropertiesField1() {
            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();

            var docNumber = GetSampleRecord().Number;

            var record = trimSrv.GetRecordByRecordNumber(docNumber);
            PrintRecord(record, true);

            var record2 = trimSrv.GetRecordByUri(record.Uri);
            PrintRecord(record2, true);


            }

        [TestMethod]
        public void ListRecords() {

            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();
            var recordList = trimSrv.GetRecordList("PROJ17/11398", max: 30);
            Console.WriteLine($"{recordList.Length} has been found");
            foreach(var record in recordList) {
                Console.WriteLine($"{record.Uri}-{record.Number}-{record.Title}");
                }


            }


        [TestMethod]
        public void UploaddTest() {
            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();

            var docNumber = GetSampleRecord().Number;
            var record = trimSrv.GetRecordByRecordNumber(docNumber, PropertyIds.RecordTitle);
            var pdfSample = GetSampleFile("Sample-RD-2.pdf");
            if(!pdfSample.Exists) {
                throw new FileNotFoundException(pdfSample.FullName);
                }

            trimSrv.UploadDocumnet(record.Uri, pdfSample, UnitTestToken, Guid.NewGuid().ToString());

            }

        /// <summary>
        /// Creating 20 records
        /// so the overnight sync will publish the Trim records
        /// </summary>
        [Ignore]
        [TestMethod]
        public void CreateTestRecordsInTrim() {
            var srv = this.GetServiceRepository();
            var trimSrv = srv.TrimService();
            var samples = 20;
           // var containerUri = 3596042; //DEV
           var containerUri = 3596749; //UAT
            for (int i = 0; i < samples; i++)
            {
                var record = trimSrv.CreateRecord(containerUri, $"Place holder for Position description {i}");
                var rdPd = TestCreateRoleDescriptionForTrim(srv, record.Number, GetSamplePdTitle(srv));
                srv.RolePositionDescriptionRepository().UpdateStatus(rdPd.RoleDescriptionId,Enums.StatusValue.Imported); // so the manual Sync will publish to trim

            }
          

            }

        private string GetSamplePdTitle(ServiceRepository srv)
        {
            var activeList = srv.PositionDescriptionRepository().ActiveList();
            var rdpdCount = activeList.Count(pd => pd.PositionDescriptionId > 1);
            var index=new Random(DateTime.Now.Millisecond).Next(1, rdpdCount-10);
            return activeList.ToList()[index].RolePositionDescription.Title;
        }

        [TestMethod]
        public void DownloadTest() {
            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();

            var record = GetSampleRecord();
            var tmpFolderName = Directory.CreateDirectory(Path.GetTempPath() + $"{Guid.NewGuid().ToString().Substring(0, 10).CleanPropertyName()}").FullName;

            var pdfSample = GetSampleFile("Sample-RD-1.pdf");
            if(!pdfSample.Exists) {
                throw new FileNotFoundException(pdfSample.FullName);
                }

            trimSrv.UploadDocumnet(record.Uri, pdfSample, UnitTestToken, Guid.NewGuid().ToString());

            trimSrv.DownloadDocumnet(record.Uri, tmpFolderName);
            var files = Directory.GetFiles(tmpFolderName);
            try {
                Assert.IsTrue(files.Length > 0);

                foreach(var file in files) {
                    Console.WriteLine(file);
                    }
                }
            finally {
                Directory.Delete(tmpFolderName, true);
                }

            }

        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestMethod]
        public void ListVleTestRecords() {

            var result = GetChildRecords("PROJ17/11398");
            foreach(var recordRef in result) {
                Console.WriteLine($"{recordRef.Uri}-{recordRef.Number}-{recordRef.Title}");

                }
            }

        

        }
    }
