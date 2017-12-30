using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.TrimBase;
using HP.HPTRIM.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.DataAnnotations;


namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.TrimIntegration
{
    [TestClass]
    public class PositionTrimTests : SecurityBase
    {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
            if(!Settings.ProjectTemplateSettings.Site.TrimIntegration) {
                Assert.Inconclusive("Trim integration is not enabled");
                }
            }

        [TestMethod]
        public void GetRecordInfoTest()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);
            var trimRecordSrv = service.TrimRecordRepository();

            var list = base.GetTrimDocNumberList();

            // find any match in database
            bool found = false;
            var dbDocNumber = string.Empty;
            foreach (var docnumber in list)
            {
                if (
                    trimRecordSrv.PositionRepository.RolePositionDescriptionRepository.List()
                        .Any(rdp => rdp.DocNumber.Equals(docnumber, StringComparison.CurrentCulture)))
                {
                    dbDocNumber = docnumber;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                dbDocNumber = list[0];
                var trimRecordInfo = trimRecordSrv.TrimService.GetRecordByRecordNumber(dbDocNumber);
                TestCreatePositionDescriptionForTrim(service, trimRecordInfo.Number,
                    trimRecordInfo.Title);

            }
            var rp =
                service.RolePositionDescriptionRepository().List().FirstOrDefault(rdp => rdp.DocNumber == dbDocNumber);
            var model = trimRecordSrv.SynchRolePosDescription(rp.RolePositionDescId, "Initialise");

            //------------  The Test

            var dbRecord = trimRecordSrv.GetTrimRecordById(rp.RolePositionDescId);

            var remoteRecord = trimRecordSrv.TrimService.GetRecordByUri(dbRecord.Uri.Value);

            var modelInfo = trimRecordSrv.GetRecordInfoModel(rp.RolePositionDescId);

            if (remoteRecord == null)
            {
                Assert.IsTrue(modelInfo.OnlineRecordStatus == OnlineRecordStatus.NoMatchingRecord,
                    "modelInfo.NoMatchingRecord == OnlineRecordStatus.NotExists");

            }
            if (dbRecord == null)
            {
                Assert.IsTrue(modelInfo.OnlineRecordStatus == OnlineRecordStatus.NotPublished,"modelInfo.OnlineRecordStatus == OnlineRecordStatus.NotPublished");
                }
            if (!dbRecord.RolePositionDescription.StatusValue.IsLive())
            {
                
                Assert.IsTrue(modelInfo.OnlineRecordStatus == OnlineRecordStatus.OutOfSync,
                    "modelInfo.OnlineRecordStatus == OnlineRecordStatus.OutOfSync");
            }
            else if (dbRecord.Token == null || dbRecord.Token != modelInfo.Token)
            {
                Assert.IsTrue(modelInfo.OnlineRecordStatus == OnlineRecordStatus.OutOfSync,
                    "modelInfo.OnlineRecordStatus==OnlineRecordStatus.OutOfSync");
            }
            else
            {
                if (dbRecord.Token == modelInfo.Token)
                {
                    Assert.IsTrue(modelInfo.OnlineRecordStatus == OnlineRecordStatus.UpToDate,
                        "modelInfo.OnlineRecordStatus == OnlineRecordStatus.Uptodate");
                }
            }

        }

        [TestMethod]
        public void SyncDraftPosInfo()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);
            var trimRecordSrv = service.TrimRecordRepository();

            var list = base.GetTrimDocNumberList();

            // find any match in database
            bool found = false;
            var dbDocNumber = string.Empty;
            foreach (var docnumber in list)
            {
                if (
                    trimRecordSrv.PositionRepository.RolePositionDescriptionRepository.List()
                        .Any(rdp => rdp.DocNumber.Equals(docnumber, StringComparison.CurrentCulture)))
                {
                    dbDocNumber = docnumber;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                dbDocNumber = list[1];
                var trimRecordInfo = trimRecordSrv.TrimService.GetRecordByRecordNumber(dbDocNumber);
                TestCreatePositionDescriptionForTrim(service, trimRecordInfo.Number,
                    trimRecordInfo.Title);

            }
            var rp =
                service.RolePositionDescriptionRepository().List().FirstOrDefault(rdp => rdp.DocNumber == dbDocNumber);

            if (rp.StatusValue.GetEnum() != Enums.StatusValue.Draft)
            {
                service.RolePositionDescriptionRepository().UpdateStatus(rp.RolePositionDescId, Enums.StatusValue.Draft);
            }

            trimRecordSrv.SynchRolePosDescription(rp.RolePositionDescId, "Re draft");

            var record=trimRecordSrv.GetTrimRecordById(rp.RolePositionDescId);
            var modelInfo = trimRecordSrv.GetRecordInfoModel(rp.RolePositionDescId);
            if (record != null)
            {
                Assert.IsTrue(modelInfo.OnlineRecordStatus == OnlineRecordStatus.OutOfSync,
                    "modelInfo.OnlineRecordStatus==OnlineRecordStatus.OutOfSync");
            }



        }

        [TestMethod]
        public void SyncNonExistsPosInfo()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);
            var trimRecordSrv = service.TrimRecordRepository();

            var list = base.GetTrimDocNumberList();

            var nonTrimDocNumberList =
                service.RolePositionDescriptionRepository()
                    .List()
                    .Where(rdp => !list.Contains(rdp.DocNumber))
                    .Take(10)
                    .ToArray();

            foreach (var rolePositionDescription in nonTrimDocNumberList)
            {

                var recordInfoModel = trimRecordSrv.GetRecordInfoModel(rolePositionDescription.RolePositionDescId);

                if (recordInfoModel == null)
                {
                    try
                    {
                        trimRecordSrv.SynchRolePosDescription(rolePositionDescription.RolePositionDescId,
                            "Initialise 'HP RM recod' record");

                        Assert.Fail("SynchRolePosDescription should throw exception");
                    }
                    catch (ObjectNotFoundException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }


            }


        }

        [TestMethod]
        public void SyncLiveRecords()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);
            var rdpService = service.RolePositionDescriptionRepository();
            var trimRecordSrv = service.TrimRecordRepository();

            var list = base.GetTrimDocNumberList();

            foreach (var docNumber in list.Take(5))
            {
                var remoteModel = trimRecordSrv.TrimService.GetRecordByRecordNumber(docNumber);
                var rdp =
                    rdpService
                        .GetRolePositionDescByDocumentNumber(docNumber)
                        .FirstOrDefault();
                if (rdp == null)
                {
                    TestCreatePositionDescriptionForTrim(service, docNumber, remoteModel.Title);
                    rdp =
                   rdpService
                       .GetRolePositionDescByDocumentNumber(docNumber)
                       .FirstOrDefault();
                    }

                if (!rdp.StatusValue.IsLive())
                {
                    CompleteStepsUntoLive(rdp, service);
                }
                rdp =
                   rdpService
                       .GetRolePositionDescByDocumentNumber(docNumber)
                       .FirstOrDefault();
                var recordInfoModel = trimRecordSrv.GetRecordInfoModel(rdp.RolePositionDescId);
                if (recordInfoModel == null)
                {
                  trimRecordSrv.SynchRolePosDescription(rdp.RolePositionDescId, "Initialise trim record");
                }


                // test
                var trimRecord = trimRecordSrv.GetTrimRecordById(rdp.RolePositionDescId);
                var remoteRecord = trimRecordSrv.TrimService.GetRecordByUri(trimRecord.Uri.Value);

                recordInfoModel = trimRecordSrv.GetRecordInfoModel(rdp.RolePositionDescId);

                Console.WriteLine(recordInfoModel);

                if (trimRecord.Token != recordInfoModel.Token || string.IsNullOrWhiteSpace(trimRecord.Token))
                {
                    trimRecordSrv.SynchRolePosDescription(rdp.RolePositionDescId, "Initialise trim record");
                }

                recordInfoModel = trimRecordSrv.GetRecordInfoModel(rdp.RolePositionDescId);
                Assert.IsTrue(recordInfoModel.OnlineRecordStatus == OnlineRecordStatus.UpToDate,
                    "recordInfoModel.OnlineRecordStatus==OnlineRecordStatus.Uptodate");

                }
            

        }

      
    }
}