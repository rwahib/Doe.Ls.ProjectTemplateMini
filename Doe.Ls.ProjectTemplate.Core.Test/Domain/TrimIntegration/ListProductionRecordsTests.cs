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
    public class ListProductionRecords : SecurityBase {
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


            var samples = srv.RoleDescriptionRepository().LiveList().Where(rd=>rd.RoleDescriptionId>10).Take(20).ToList();
            foreach(var roleDescription in samples) {
                var record = trimSrv.GetRecordByRecordNumber(roleDescription.RolePositionDescription.DocNumber);
                if (record == null)
                {
                    Console.WriteLine($"Record {roleDescription} has null TRIM RECORD");
                    continue;
                }
                PrintRecord(record, true);

                var record2 = trimSrv.GetRecordByUri(record.Uri);
                PrintRecord(record2, true);
                }

            }


        }
    }
