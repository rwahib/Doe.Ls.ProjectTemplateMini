

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices.PdfService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.TrimBase;
using HP.HPTRIM.ServiceModel;

using ServiceStack;
using Unity.Attributes;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories
{
    public partial class TrimRecordRepository : BaseRepository<TrimRecord>
    {
        public TrimRecordRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<TrimRecord> List()
        {
            return base.List()
                    .Include(ent => ent.RolePositionDescription).Include(ent => ent.RolePositionDescription.StatusValue)
                    .OrderBy(ent => ent.RolePositionDescId);
        }
        public TrimRecord GetTrimRecordById(int rolePositionDescId)
        {
            return List().SingleOrDefault(r => r.RolePositionDescId == rolePositionDescId);

        }
        public TrimRecord GetTrimRecordByUri(int uri)
        {
            return List().SingleOrDefault(r => r.Uri == uri);

        }
        public void InsertOrUpdate(TrimRecord entity)
        {
            var obj = GetTrimRecordById(entity.RolePositionDescId);
            if (obj == null) base.Insert(entity);
            else
            {
                entity.To(obj);
                Update(obj);
            }

        }



        public RecordInfoModel SynchRolePosDescription(int rolePositionDescId, string comment, bool force = false)
        {
            if (force) ExpireToken(rolePositionDescId);
            var record = GetTrimRecordById(rolePositionDescId);

            if (record == null)
            {
                var rdpd =
                    this.PositionRepository.RolePositionDescriptionRepository.GetRolePositionDescById(rolePositionDescId);
                var remoteRecord = TrimService.GetRecordByRecordNumber(rdpd.DocNumber);
                if (remoteRecord == null) throw new ObjectNotFoundException($"Trim object not found for {rdpd.DocNumber})");

                Insert(new TrimRecord
                {
                    RolePositionDescId = rolePositionDescId,
                    Uri = remoteRecord.Uri

                });
                record = GetTrimRecordById(rolePositionDescId);
            }

            switch (record.RolePositionDescription.StatusValue.GetEnum())
            {
                case Enums.StatusValue.Approved:
                case Enums.StatusValue.Imported:

                    var model = TrimService.GetRecordModel(record.Uri.Value);
                    if (!string.IsNullOrWhiteSpace(model.Token) && !string.IsNullOrWhiteSpace(record.Token))
                    {
                        if (model.Token == record.Token) return GetRecordInfoModel(rolePositionDescId);  // already in sync
                    }

                    var position = PositionRepository.GetGenericPositionForTrim(rolePositionDescId);
                    var token = Guid.NewGuid().ToString();
                    var result = PdfService.GeneratePdf(position);

                    TrimService.UploadDocumnet(model.Uri, new FileInfo(result.OutputFileFullPath), comment, token);
                    TrimService.UpdateRecord(model.Uri, new TrimPropVal(PropertyIds.RecordTitle, record.RolePositionDescription.Title));

                    model = TrimService.GetRecordModel(record.Uri.Value);

                    record.Token = token;
                    record.LastRevisionNumber = model.Revision;
                    Update(record);
                    break;
                default:
                    if (record.Token != null)
                    {
                        if (!force)// force means already expired
                        {
                            ExpireToken(rolePositionDescId);
                        }
                    }
                    break;
            }

            return GetRecordInfoModel(rolePositionDescId);

        }

        public TrimRecordInfoModel PublishRolePosDescription(int rolePositionDescId, string comment)
        {
            ExpireToken(rolePositionDescId);
            return SynchRolePosDescription(rolePositionDescId, comment);
        }

        public void SynchAllPdsRds(string comment)
        {
            throw new NotImplementedException();
        }
        public void PublishAllPdsRds(string comment)
        {
            throw new NotImplementedException();
        }

        public void ExpireToken(int rolePositionDescId)
        {
            var record = GetTrimRecordById(rolePositionDescId);
            if (record != null)
            {
                record.Token = null;
                record.LastPublishedDate = null;
                record.LastRevisionNumber = null;
                base.Update(record);
            }
        }

        public RecordInfoModel GetRecordInfoModel(int rolePositionDescId, bool requestRemoteFileName = false)
        {
            var status = OnlineRecordStatus.NotPublished;
            string message;
            RolePositionDescription rpd = null;
            TrimRecordInfoModel model = null;
            var record = GetTrimRecordById(rolePositionDescId);
            if (record == null)
            {
                rpd =
                   PositionRepository.RolePositionDescriptionRepository.GetRolePositionDescById(rolePositionDescId);
                var trimRecord = TrimService.GetRecordByRecordNumber(rpd.DocNumber);
                if (trimRecord != null) model = TrimService.GetRecordModel(trimRecord.Uri);
            }
            else
            {
                model = TrimService.GetRecordModel(record.Uri.Value, true);
            }

            if (model == null || model.TrimRecordStatus == TrimRecordStatus.RecordNotExists)
            {

                status = OnlineRecordStatus.NoMatchingRecord;

                if (record == null)
                {
                    message = $"There is no 'HP RM record' matching {rpd.DocNumber} ";
                }
                else
                {
                    message = $"There is no 'HP RM record' matching {record.RolePositionDescription.DocNumber} ";
                }



                return RecordInfoModel.Map(null, status, message);
            }




            if (record == null)
            {
                return new RecordInfoModel
                {
                    OnlineRecordStatus = OnlineRecordStatus.NotPublished,
                    StatusMessage = "Attachment is not published yet"
                };
            }





            if (!record.RolePositionDescription.StatusValue.IsLive())
            {


                status = OnlineRecordStatus.OutOfSync;

                message = $"Record is modified and not synced yet.";

                return RecordInfoModel.Map(model, status, message);
            }
            if (model.Token == record.Token)
            {
                status = OnlineRecordStatus.UpToDate;

                message = $"Trim attachment is 'up to date' with revision {model.Revision}";

                return RecordInfoModel.Map(model, status, message);
            }

            if (model.Token != record.Token)
            {
                status = OnlineRecordStatus.OutOfSync;

                message = $"Trim attachment is out of sync.";

                return RecordInfoModel.Map(model, status, message);
            }

            return RecordInfoModel.Map(model);
        }

        [Unity.Attributes.Dependency]
        public ITrimService TrimService { get; set; }

        [Unity.Attributes.Dependency]
        public PdfService PdfService { get; set; }

        [Unity.Attributes.Dependency]
        public PositionRepository PositionRepository { get; set; }
    }
}
