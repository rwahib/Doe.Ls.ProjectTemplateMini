using System;
using System.ComponentModel.DataAnnotations;
using Doe.Ls.TrimBase;
using iTextSharp.tool.xml.pipeline.ctx;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models {
    public class RecordInfoModel : TrimRecordInfoModel {

        [Display(Name = "PSNSW Online record status")]
        public OnlineRecordStatus OnlineRecordStatus { get; set; }
        [Display(Name = "PSNSW Online status message")]
        public string StatusMessage { get; set; }

        public static RecordInfoModel Map(TrimRecordInfoModel model, OnlineRecordStatus onlineRecordStatus=OnlineRecordStatus.UpToDate, string statusMessage="Uptodate")
        {
            var newModel = new RecordInfoModel
            {
                OnlineRecordStatus = onlineRecordStatus,
                StatusMessage = statusMessage
            };
            if (model != null)
            {
                model.To(newModel);
            }
            else
            {
                model = new TrimRecordInfoModel {
                    Uri = -1,Title = "",Token = "",FileName = "",DocumentUrl = "",RecordNumber = "",Note = "Record not exists",Revision = -1,TrimRecordStatus = TrimRecordStatus.RecordNotExists

                    };
                model.To(newModel);
                }
            return newModel;
        }

        public override string ToString()
        {
            return $"{Uri}-{RecordNumber}-{Title}-{OnlineRecordStatus}-{TrimRecordStatus}-{FileName} - published token {Token}";
        }
    }
}