

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories
{
    public partial class PositionInformationRepository : BaseRepository<PositionInformation>
    {
        public PositionInformationRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService) {
        }

        public override IQueryable<PositionInformation> List() {
            return base.List()
                    .Include(ent => ent.EmployeeType)
                    .Include(ent => ent.Position)
                    .Include(ent => ent.PositionType)
                    .Include(ent => ent.PositionNotes)
                    .OrderBy(ent => ent.PositionId);
        }

        public PositionInformation GetPositionInformationById(int positionId) {
            return this.List().FirstOrDefault(l => l.PositionId == positionId);
        }
        public override void Insert(PositionInformation entity) {

            if (ValidateEntity(entity).Count > 0) {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }

        public override void Update(PositionInformation entity, bool refresh = true) {

            if (ValidateEntity(entity).Count > 0) {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
            AppCacheHelper.Expire(Enums.CacheRegion.Position);
        }





        public IQueryable<PositionInformation> FilterPositionInformations(IQueryable<PositionInformation> positionInformations, SearchArg searchArg) {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionInformation = positionInformations.Where(ent =>
                    (!string.IsNullOrEmpty(ent.Position.PositionTitle) && ent.Position.PositionTitle.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OlderPositionNumber3) && ent.OlderPositionNumber3.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OlderPositionNumber1) && ent.OlderPositionNumber1.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.OlderPositionNumber2) && ent.OlderPositionNumber2.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.SchNumber) && ent.SchNumber.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.PositionType.PositionTypeName) && ent.PositionType.PositionTypeName.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.EmployeeType.EmployeeTypeName) && ent.EmployeeType.EmployeeTypeName.ToLower().Contains(searchWord))


);

            return filteredPositionInformation.OrderBy(e => e.PositionId);
        }


        public void UpdateOrInsertPositionInformation(PositionInformation positionInformation, string note = "") {
            var oldPositionInformation = GetPositionInformationById(positionInformation.PositionId);

            if (oldPositionInformation == null) {
                var position = PositionRepository.GetPositionById(positionInformation.PositionId) != null;
                if (!position) {
                    var msg = MessageHelper.NotFoundMessage($"position ({positionInformation.PositionId})");
                    throw new InvalidOperationException(msg);
                    //throw new InvalidOperationException("Position doesn't exists");
                }
                ValidatePositionEndDate(positionInformation);
                this.Insert(positionInformation);

            } else {
                ValidatePositionEndDate(positionInformation);
                this.SetPropertyValuesFrom(ref oldPositionInformation, positionInformation);

                if (string.IsNullOrWhiteSpace(positionInformation.OtherOverview))
                    oldPositionInformation.OtherOverview = "";

                ValidatePositionEndDate(oldPositionInformation);
                if (positionInformation.PositionTypeCode != Enums.PositionType.T.ToString()) {
                    oldPositionInformation.PositionEndDate = null;
                }
                this.Update(oldPositionInformation);
            }
            AddNotes(positionInformation, note);
        }
        private void AddNotes(PositionInformation positionInformation, string note) {
            if (note != null && note.Length > 1) {
                var positionNote = new PositionNote() {
                    Notes = note,
                    PositionId = positionInformation.PositionId,
                    CreatedBy = SessionService.GetCurrentUser().UserName,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                };
                RepositoryFactory.GetService<PositionNoteRepository>().Insert(positionNote);
            }
        }
        private static void ValidatePositionEndDate(PositionInformation positionInformation) {
            if (positionInformation.PositionTypeCode == Enums.PositionType.T.ToString()) {
                if (positionInformation.PositionEndDate == null) {
                    var msg = MessageHelper.PositionEndDateRequired();
                    throw new Exception(msg);
                    //throw new Exception("Position end date is mandatory for temporary position type");
                }
            }

        }

        [Unity.Attributes.Dependency]
        public PositionRepository PositionRepository { get; set; }

    }
}



