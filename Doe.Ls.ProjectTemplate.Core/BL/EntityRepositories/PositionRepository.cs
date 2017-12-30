using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml.Linq;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.PositionDescriptions.Core.BL.Models;
using ServiceStack;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories {
    public partial class PositionRepository : BaseRepository<Position> {
        public PositionRepository(IUnitOfWork unitOfWork, ILoggerService loggerService,
            ISessionService sessionService) : base(unitOfWork, loggerService, sessionService) {
            }

        public IQueryable<Position> BaseList() {
            return base.List()
                .Include(ent => ent.StatusValue)
                .Include(ent => ent.RolePositionDescription)
                .Include(ent => ent.RolePositionDescription.StatusValue)
                .Include(ent => ent.RolePositionDescription.Grade)
                .Include(ent => ent.ParentPosition);
            }

        public Position GetGenericPositionForTrim(int rolepositionDescid) {

            var rolePosDesc = this.RolePositionDescriptionRepository.GetRolePositionDescById(rolepositionDescid);
            var position = new Position {
                PositionId = Enums.Cnt.GenericCode,
                PositionNumber = Enums.Cnt.Generic,
                DivisionOverview = Enums.Cnt.Generic,
                StatusId = (int)Enums.StatusValue.Imported,
                StatusValue = new StatusValue {
                    StatusId = (int)Enums.StatusValue.Imported,
                    StatusName = Enums.StatusValue.Imported.ToString(),
                    },
                Location = new Location {
                    LocationId = Enums.Cnt.GenericCode,
                    Name = Enums.Cnt.Generic,

                    },
                UnitId = Enums.Cnt.GenericCode,
                Unit = new Unit {
                    StatusId = (int)Enums.StatusValue.Active,
                    StatusValue = new StatusValue {
                        StatusId = (int)Enums.StatusValue.Active,
                        StatusName = Enums.StatusValue.Active.ToString(),
                        },
                    UnitId = Enums.Cnt.GenericCode,
                    UnitName = Enums.Cnt.Generic,
                    BusinessUnit = new BusinessUnit {
                        StatusId = (int)Enums.StatusValue.Active,
                        StatusValue = new StatusValue {
                            StatusId = (int)Enums.StatusValue.Active,
                            StatusName = Enums.StatusValue.Active.ToString(),
                            },
                        BUnitId = Enums.Cnt.GenericCode,
                        BUnitName = Enums.Cnt.Generic,
                        DirectorateId = Enums.Cnt.GenericCode,
                        Directorate = new Directorate {
                            StatusId = (int)Enums.StatusValue.Active,
                            StatusValue = new StatusValue {
                                StatusId = (int)Enums.StatusValue.Active,
                                StatusName = Enums.StatusValue.Active.ToString(),
                                },
                            DirectorateId = Enums.Cnt.GenericCode,
                            DirectorateName = Enums.Cnt.Generic,
                            ExecutiveCod = Enums.Cnt.Generic,
                            Executive = new Executive {
                                StatusId = (int)Enums.StatusValue.Active,
                                StatusValue = new StatusValue {
                                    StatusId = (int)Enums.StatusValue.Active,
                                    StatusName = Enums.StatusValue.Active.ToString(),
                                    },
                                ExecutiveCod = Enums.Cnt.Generic,
                                ExecutiveTitle = Enums.Cnt.Generic
                                }
                            }
                        }
                    },
                PositionInformation = new PositionInformation
                {
                    OtherOverview = Enums.Cnt.Generic,
                    PositionId = Enums.Cnt.GenericCode,                    
                    }
                ,
                RolePositionDescriptionId = rolePosDesc.RolePositionDescId,
                RolePositionDescription = rolePosDesc,
                PositionTitle = rolePosDesc.Title
                };
            return position;
            }

        public IQueryable<Position> GetPositionsByStatus(Enums.StatusValue status) {
            return List().Where(p => p.StatusId == (int)status);
            }

        public int[] GetRootPositionsIds() {
            var live = new List<int>
            {
                    (int) Enums.StatusValue.Approved,
                    (int) Enums.StatusValue.Imported,
                };
            return
                base.List()
                    .Where(
                        p =>
                            p.ReportToPositionId == -1 && live.Contains(p.StatusId) &&
                            p.PositionLevelId == (int)Enums.PositionLevel.Executive)
                    .Select(p => p.PositionId).ToArray();
            }

        public override IQueryable<Position> List() {
            return BaseList()
                .Include(ent => ent.CostCentreDetail)
                .Include(ent => ent.CostCentreDetail)
                .Include(ent => ent.PositionLevel)
                .Include(ent => ent.ParentPosition)
                .Include(ent => ent.Unit.BusinessUnit.Directorate.Executive)
                .Include(ent => ent.Unit)
                .Include(ent => ent.PositionInformation)
                .OrderBy(ent => ent.PositionId);
            }

        public IQueryable<Position> ListForChart() {
            return BaseList()
                .Include(ent => ent.PositionLevel)
                .Include(ent => ent.Unit)
                .Include(ent => ent.Location)
                .Include(ent => ent.RolePositionDescription)
                .Include(ent => ent.Unit.BusinessUnit)
                .Include(ent => ent.Unit.BusinessUnit.Directorate)
                .Include(ent => ent.PositionInformation)
                .Include(ent => ent.PositionInformation.EmployeeType)
                .Include(ent => ent.PositionInformation.PositionType)
                .OrderBy(ent => ent.PositionId)
                .Where(
                    ent =>
                        ent.StatusId == (int)Enums.StatusValue.Approved ||
                        ent.StatusId == (int)Enums.StatusValue.Imported)
                .AsNoTracking();
            }

        public IQueryable<Position> ListForPositionInfo() {
            return List().Include(ent => ent.PositionInformation)
                .Include(ent => ent.PositionInformation.EmployeeType)
                .Include(ent => ent.StatusValue)

                .Include(ent => ent.PositionInformation.PositionType)
                .Include(ent => ent.PositionInformation.OccupationType)
                .Include(ent => ent.PositionInformation.PositionStatusValue)
                .Include(ent => ent.PositionInformation.PositionNotes)
                .OrderBy(ent => ent.PositionId);
            }


        public IQueryable<Position> ListForReportTo() {
            return BaseList()
                .Include(ent => ent.RolePositionDescription)
                .OrderBy(ent => ent.PositionId);
            }


        public IQueryable<Position> ListForPdRoleDesc() {
            return BaseList()
                .Include(ent => ent.Unit)
                .Include(ent => ent.Unit.BusinessUnit)
                .Include(ent => ent.Unit.BusinessUnit.Directorate)
                .Include(ent => ent.Unit.BusinessUnit.Directorate.Executive)
                .Include(ent => ent.CostCentreDetail)
                .Include(ent => ent.PositionInformation)
                .Include(ent => ent.PositionInformation.PositionStatusValue)
                .Include(ent => ent.PositionInformation.EmployeeType)
                .Include(ent => ent.PositionInformation.PositionType)
                .Include(ent => ent.PositionInformation.OccupationType)
                .Include(ent => ent.Location)
                .Include(ent => ent.CostCentreDetail)
                .Include(ent => ent.Positions)
                .Include(ent => ent.RolePositionDescription)
                .Include(ent => ent.RolePositionDescription.Grade)
                .Include("Positions.StatusValue");



            }

        public IEnumerable<Position> ListForUpdateImportedData() {
            return
                BaseList()
                    .Include(ent => ent.CostCentreDetail)
                    .Include(ent => ent.PositionInformation)
                    .Include(ent => ent.RolePositionDescription)
                    .Include(ent => ent.RolePositionDescription.RoleDescription)
                    .Include(ent => ent.RolePositionDescription.RoleDescription.KeyRelationships)
                    .Include(ent => ent.RolePositionDescription.RoleDescription.RoleCapabilities)
                    .Include(ent => ent.RolePositionDescription.PositionDescription)
                    .Include(ent => ent.RolePositionDescription.PositionDescription.PositionFocusCriterias)
                    .Where(p => p.StatusId == (int)Enums.StatusValue.Draft
                                && p.RolePositionDescriptionId != -1 && p.CreatedBy == "System").ToList();
            }

        public override void Insert(Position position) {
            position.UpdateSignature(this.RepositoryFactory);
            if(position.PositionLevelId == Enums.Cnt.Na) {
                position.PositionLevelId = (int)Enums.PositionLevel.Position;
                }
            if(ValidateEntity(position).Count > 0) {
                throw new EntityValidationException { Errors = ValidateEntity(position) };
                }

            if(string.IsNullOrWhiteSpace(position.DivisionOverview)) {
                var srv = new ServiceRepository(this.RepositoryFactory);
                if(position.UnitId != Enums.Cnt.Na) {
                    try {
                        var div =
                            srv.UnitRepository().GetUnitById(position.UnitId).BusinessUnit.Directorate.Executive;
                        position.DivisionOverview = div.DefaultExecutiveOverview;
                        }
                    catch(Exception exception) {
                        LoggerService.Log(exception);
                        }
                    }


                }

            base.Insert(position);
            Refresh(position);
            }

        private void UpdatePositionHeirarchyPath(int positionId, bool newDbInstance = false) {
            if(!newDbInstance) {
                var ctx = (this.UnitOfWork.DbContext as SampleProjectTemplateEntities);

                ctx.UpdatePositionHierarchy(positionId);
                return;
                }
            else
            {
                var newUnitOfWork = this.RepositoryFactory.GetService<IUnitOfWork>("new-instance");
                using (var ctx = newUnitOfWork.DbContext as SampleProjectTemplateEntities) {
                    ctx.UpdatePositionHierarchy(positionId);

                    }
                }

            }

        public override void Update(Position entity, bool refresh = true) {
            entity.LastModifiedDate = DateTime.Now;
            if(SessionService.GetCurrentUser() != null) {
                entity.LastModifiedBy = SessionService.GetCurrentUser().UserName;
                }
            if(ValidateEntity(entity).Count > 0) {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
                }

            base.Update(entity, refresh);
            }

        private IQueryable<Position> FilterPositions(IQueryable<Position> positions, SearchArg searchArg) {
            var searchWord = searchArg.Search.ToLower();
            var filteredPosition = positions.Where(ent =>
                (!string.IsNullOrEmpty(ent.PositionNumber) && ent.PositionNumber.ToLower().Contains(searchWord))
                || (!string.IsNullOrEmpty(ent.PositionTitle) && ent.PositionTitle.ToLower().Contains(searchWord))

                || (!string.IsNullOrEmpty(ent.Unit.UnitName) && ent.Unit.UnitName.ToLower().Contains(searchWord))
                || ent.RolePositionDescription.GradeCode.Contains(searchWord)
                || ent.RolePositionDescription.DocNumber.ToString().Contains(searchWord)
                );

            return filteredPosition.OrderBy(e => e.PositionId);
            }

        public List<Position> LoadPositionListForChart(PositionChartFilterParams positionFilterParam) {
            List<Position> positionList;
            var positionFullList = ApplyFilterForChart(out positionList, positionFilterParam);

            return GenerateListFromPath(positionFullList, positionList);
            }

        private List<Position> GenerateListFromPath(List<Position> positionFullList, List<Position> positionList) {
            var rootIds = GetRootPositionsIds();
            var hierarchyPaths = positionList.Select(pl => pl.PositionPath).ToList();

            foreach(var path in hierarchyPaths) {
                int count = path.Count(f => f == '/');
                if(count > 2) {
                    var intArr = SplitStringToIntArray(path);

                    for(int i = 1; i < intArr.Length; i++) {


                        if(positionList.All(p => p.PositionId != intArr[i])) {
                            var tmp = intArr[i];
                            var pos = positionFullList.FirstOrDefault(pf => pf.PositionId == tmp);
                            if(pos != null) {
                                positionList.Add(pos);
                                }
                            }
                        }

                    }


                }
            //This if condition make sure that the relevant root is added in the chart list

            if(positionList.Any() && positionList.All(p => !rootIds.Contains(p.PositionId))) {
                var arr = SplitStringToIntArray(positionList.FirstOrDefault().PositionPath);
                //var t = positionFullList.FirstOrDefault(pl => pl.PositionId == (int)Enums.Position.RootEdsServices || pl.PositionId == (int)Enums.Position.Root1);
                var t = positionFullList.FirstOrDefault(pl => pl.PositionId == arr[arr.Length - 1]);
                if(t != null) {
                    if(positionList.All(p => p.PositionId != t.PositionId)) {
                        positionList.Add(t);
                        }
                    }
                }
            return positionList;
            }

        private static int[] SplitStringToIntArray(string path) {
            char[] separatingChars = { '/' };
            var arr = path.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
            int[] intArr = arr.Select(int.Parse).ToArray();
            return intArr;
            }


        private List<Position> ApplyFilterForChart(out List<Position> positionList,
            PositionChartFilterParams positionFilterParam) {
            var cachedPositionList = CachedPositionListForChart();
            var rootIds = GetRootPositionsIds();
            positionList =
                cachedPositionList.Where(p => rootIds.Contains(p.PositionId) || p.ReportToPositionId != -1).ToList();

            if(positionFilterParam.UnitId > 0) {
                positionList = positionList.Where(p => p.UnitId == positionFilterParam.UnitId).ToList();

                }
            else if(positionFilterParam.BusinessUnitId > 0) {
                positionList =
                    positionList.Where(p => p.Unit.BUnitId == positionFilterParam.BusinessUnitId).ToList();

                }
            else if(positionFilterParam.DirectorateId > 0) {
                positionList =
                    positionList.Where(p => p.Unit.BusinessUnit.DirectorateId == positionFilterParam.DirectorateId)
                        .ToList();

                }
            else if(!string.IsNullOrEmpty(positionFilterParam.DivisionCode)) {
                positionList =
                    positionList.Where(
                        p => p.Unit.BusinessUnit.Directorate.ExecutiveCod == positionFilterParam.DivisionCode)
                        .ToList();

                }
            //if(positionFilterParam.NoOfLevels >= 2)
            //    {
            //    //var levels = positions.Select(p => p.PositionPath.Count(pp => pp == '/')).Max();
            //    positionList = positionList.Where(p => p.PositionPath.Count(pp => pp == '/') <= positionFilterParam.NoOfLevels + 1).ToList();

            //    }

            return cachedPositionList.ToList();
            }

        public List<Position> CachedPositionListForChart() {
            var cachedPositionList =
                AppCacheHelper.GetResult<List<Position>>(Enums.CacheRegion.Position.ToString());
            if(cachedPositionList == null) {
                cachedPositionList = this.ListForChart().ToList();

                AppCacheHelper.Cache(Enums.CacheRegion.Position.ToString(), cachedPositionList);

                }
            return cachedPositionList;
            }

        //This method can be removed
        public int GetTopPositionToPrint(int unitId) {
            var query = "SELECT top(1)(SELECT COUNT(*) FROM dbo.fnSplitString(PositionPath, '/')) AS levels," +
                        "* FROM dbo.Position WHERE UnitId =" + unitId + " AND PositionId != " + -1 +
                        "ORDER BY 1";

            DbRawSqlQuery<Position> data = SqlQuery(query);

            var idToprint = data.FirstOrDefaultAsync().Result.PositionId;
            return idToprint;
            }

        public List<Position> GetChartObjForGeneratePdf(int positionId) {
            var fullList =
                CachedPositionListForChart().Where(p => (p.PositionId != -1 || p.ReportToPositionId != -1));

            //var fullList = this.ListForChart().Where(p => (p.PositionId != -1 || p.ReportToPositionId != -1));//LoadPositionListFromCache();

            var selectedPosition = fullList.FirstOrDefault(p => p.PositionId == positionId);
            var topPosition = fullList.FirstOrDefault(p => p.PositionId == 1);
            var filteredList = new List<Position>();
            if(positionId == 1) {
                var directReportList = fullList.Where(p => p.ReportToPositionId == positionId);
                filteredList.AddRange(directReportList);
                filteredList.Add(topPosition);
                return filteredList;
                }


            char[] separatingChars = { '/' };
            var pathArr = selectedPosition.PositionPath.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
            if(pathArr.Count() == 2) {
                //Executive directors
                var directReportList = fullList.Where(p => p.ReportToPositionId == positionId).ToList();
                directReportList.Add(selectedPosition);
                return directReportList;
                }
            if(pathArr.Count() >= 3) {
                //directors
                if(pathArr.Last().Contains("-1")) {
                    filteredList =
                        fullList.Where(
                            p =>
                                p.PositionPath.EndsWith(selectedPosition.PositionPath) ||
                                p.PositionPath.EndsWith(selectedPosition.PositionPath.Replace("/-1", "")) ||
                                p.PositionPath.EndsWith(selectedPosition.PositionPath.Replace("-1", ""))).ToList();
                    }
                else {
                    filteredList =
                        fullList.Where(p => p.PositionPath.EndsWith(selectedPosition.PositionPath)).ToList();
                    }


                }
            return filteredList;


            }

        public List<PositionChartModel> LoadChartJson(List<Position> positionList, bool forPrint = false,
            int positionId = 0) {
            var rootIdis = GetRootPositionsIds();
            var items = new List<PositionChartModel>();
            try {
                items.AddRange(positionList.Select(p => new PositionChartModel {
                    id = Convert.ToInt32(p.PositionId),
                    parent =
                        rootIdis.Contains(p.PositionId) || (positionId > 1 && p.PositionId == positionId)
                            ? null
                            : (int?)p.ReportToPositionId,
                    title = p.PositionNumber + "", //p.PositionNumber + "",
                    description =
                        string.Format("{0}{5}{1}|{2}/{3}|{4}  ", p.PositionTitle,
                            p.RolePositionDescription != null ? p.RolePositionDescription.GradeCode : "",
                            p.PositionInformation != null ? p.PositionInformation.PositionTypeCode : "",
                            p.PositionInformation != null ? p.PositionInformation.EmployeeTypeCode : "",
                            p.Location != null ? p.Location.Name : "", "\n"),

                    unitId = p.UnitId,
                    unitName = p.Unit.UnitName,
                    positionTitle = p.PositionTitle,
                    itemType =
                        p.PositionLevelId == (int)Enums.PositionLevel.Support
                            ? (int)Enums.NodeType.Assistant
                            : (int)Enums.NodeType.Regular,
                    templateName = "CursorTemplate",
                    itemTitleColor = p.Unit.BusinessUnit.Directorate.DirectorateCustomClass,
                    groupTitleColor = p.Unit.BusinessUnit.Directorate.DirectorateCustomClass,
                    label = p.PositionTitle,
                    positionNumber = p.PositionNumber,
                    groupTitle = p.PositionLevel.PositionLevelName,

                    DescUrl = p.RolePositionDescriptionId > 0 ? "/Position/ManageSummary?Id=" + p.PositionId : "",

                    }));
                }
            catch(Exception e) {
                Console.WriteLine(e.Message);
                }

            return items;
            }


        public void AttachRolePositionDescToPosition(int positionId, int rolePositionDescId) {
            var position = this.List().FirstOrDefault(l => l.PositionId == positionId);
            if(position == null) {
                var msg = MessageHelper.NotFoundMessage($"position ({positionId})");
                throw new InvalidOperationException(msg);
                }
            position.RolePositionDescriptionId = rolePositionDescId;
            this.Update(position);
            }

        public Position CreateNewPosition(int rolePositionDescId, string title) {
            if(rolePositionDescId <= 0) {
                throw new InvalidOperationException(
                    "Before create a new Position, its Role/Position Description must be created first");
                }
            var position = new Position {
                RolePositionDescriptionId = rolePositionDescId,
                ReportToPositionId = -1,
                PositionNumber = Enums.Cnt.DefaultPositionNo,
                UnitId = -1,
                PositionPath = "-1",
                PositionTitle = title,
                PositionLevelId = -1,
                StatusId = (int)Enums.StatusValue.Draft,
                LocationId = -1,

                };
            this.Insert(position);

            return position;
            }

        public bool Exists(string positionNumber) {
            const int delStatus = (int)Enums.StatusValue.Deleted;

            return this.List().Where(p => p.StatusId != delStatus).Any(p => p.PositionNumber == positionNumber);
            }

        public Position CreatePositionAndRolePositionDescription(RolePositionDescription rolePos) {
            var guid = UnitOfWork.StratTransction();

            try {
                var rolePosdesc = RolePositionDescriptionRepository.CreateRolePositionDescription(rolePos);
                var position = CreateNewPosition(rolePosdesc.RolePositionDescId, rolePosdesc.Title);
                UnitOfWork.CommitTransaction(guid);
                UpdatePositionHeirarchyPath(position.PositionId);
                return position;
                }
            catch(Exception ex) //error occurred
            {
                UnitOfWork.RollbackTransaction(guid);
                throw new Exception(ex.Message);
                }
            //Handel error
            }

        public Position CreateOrUpdatePosition(Position newPosition) {
            var oldPosition = this.List().FirstOrDefault(p => p.PositionId == newPosition.PositionId);
            ValidatePositionObj(newPosition);


            if(oldPosition == null) {
                var rpDescription =
                    RolePositionDescriptionRepository.BaseList()
                        .FirstOrDefault(l => l.RolePositionDescId == newPosition.RolePositionDescriptionId);
                if(rpDescription == null) {
                    var msg = MessageHelper.RdPdMustExistsBeforePositionMessage();
                    throw new InvalidOperationException(msg);
                    }
                newPosition.PositionTitle = rpDescription.Title;
                newPosition.PositionPath = "-1";
                newPosition.StatusId = (int)Enums.StatusValue.Draft;

                this.Insert(newPosition);
                UpdatePositionHeirarchyPath(newPosition.PositionId);
                AppCacheHelper.Expire(Enums.CacheRegion.Position);
                return newPosition;

                }
            else {

                oldPosition.PositionNumber = newPosition.PositionNumber;
                oldPosition.Description = newPosition.Description;
                oldPosition.ReportToPositionId = newPosition.ReportToPositionId;
                oldPosition.UnitId = newPosition.UnitId;
                oldPosition.PositionLevelId = newPosition.PositionLevelId;
                oldPosition.LocationId = newPosition.LocationId;
                //  oldPosition.UnitChiefPositionId = newPosition.UnitChiefPositionId;

                this.Update(oldPosition);
                UpdatePositionHeirarchyPath(oldPosition.PositionId);
                AppCacheHelper.Expire(Enums.CacheRegion.Position);
                return oldPosition;
                }
            }


        public Position CreateOrUpdatePositionWithHistory(Position newPosition, string userName) {
            var oldPosition = this.List().FirstOrDefault(p => p.PositionId == newPosition.PositionId);
            ValidatePositionObj(newPosition);
            var reportToChanged = false;
            StringBuilder sb = new StringBuilder();
            var history = new PositionHistory();
            if(oldPosition == null) {
                //never comes here!!
                var rpDescription =
                    RolePositionDescriptionRepository.BaseList()
                        .FirstOrDefault(l => l.RolePositionDescId == newPosition.RolePositionDescriptionId);
                if(rpDescription == null) {
                    var msg = MessageHelper.RdPdMustExistsBeforePositionMessage();
                    throw new InvalidOperationException(msg);
                    }
                newPosition.PositionTitle = rpDescription.Title;
                newPosition.PositionPath = "-1";
                newPosition.StatusId = (int)Enums.StatusValue.Draft;

                //add to history
                history = PositionHistoryRepository.GetHistoryOnCreatePosition(newPosition, userName, sb.ToString());

                newPosition.PositionHistories.Add(history);
                this.Insert(newPosition);
                AppCacheHelper.Expire(Enums.CacheRegion.Position);
                return newPosition;

                }
            else {

                var oldUnitId = oldPosition.UnitId;
                var oldStatusId = oldPosition.StatusId;
                reportToChanged = oldPosition.ReportToPositionId != newPosition.ReportToPositionId;
                var factory = new RepositoryFactory();
                factory.RegisterAllDependencies();
                var srv = new ServiceRepository(factory);
                var newUnitDetails = srv.UnitRepository().GetUnitById(newPosition.UnitId);
                var newReportsTo = srv.PositionRepository().GetPositionById(newPosition.ReportToPositionId);
                var newLocation = srv.LocationRepository().GetLocationById(newPosition.LocationId);
                var newPositionLevel =
                    srv.PositionLevelRepository().GetPositionLevelById(newPosition.PositionLevelId);
                //add to history
                if(oldUnitId == Enums.Cnt.Na) {
                    //this is new created position

                    sb.Clear();
                    sb.Append("Basic details have been added. ");
                    sb.Append("Position#=" + newPosition.PositionNumber + ", ");
                    sb.Append("Position title =" + oldPosition.PositionTitle + ", ");
                    if(newUnitDetails != null) {
                        sb.Append("Division =" + newUnitDetails.BusinessUnit.Directorate.ExecutiveCod + ", ");
                        sb.Append("Directorate =" + newUnitDetails.BusinessUnit.Directorate.DirectorateName + ", ");
                        sb.Append("Business unit =" + newUnitDetails.BusinessUnit.BUnitName + ", ");
                        sb.Append("Team =" + newUnitDetails.UnitName + ", ");
                        sb.Append("ReportsTo =" + newReportsTo.PositionTitle + " (" + newReportsTo.PositionNumber +
                                  ")" + ", ");
                        }
                    if(newLocation != null) {
                        sb.Append("Location =" + newLocation.Name + ", ");
                        }

                    sb.Append("Position level =" + newPositionLevel.PositionLevelName + ".");

                    history = PositionHistoryRepository.GetHistoryOnCreatePosition(newPosition, userName,
                        sb.ToString());

                    }
                else {

                    if(oldStatusId != (int)Enums.StatusValue.Draft) {
                        var oldLocation = srv.LocationRepository().GetLocationById(oldPosition.LocationId);
                        var oldUnit = srv.UnitRepository().GetUnitById(oldUnitId);
                        var oldPositionLevel =
                            srv.PositionLevelRepository().GetPositionLevelById(oldPosition.PositionLevelId);

                        var displayOldPos = new Position {
                            PositionNumber = oldPosition.PositionNumber,
                            PositionTitle = oldPosition.PositionTitle,
                            Unit = oldUnit,
                            Location = oldLocation,
                            PositionLevel = oldPositionLevel
                            };

                        newPosition.Unit = newUnitDetails;
                        newPosition.Location = newLocation;
                        newPosition.PositionLevel = newPositionLevel;
                        newPosition.ParentPosition = newReportsTo;

                        sb.Clear();
                        sb.Append(PositionHistoryRepository.GetPositionChanges(displayOldPos, newPosition));

                        history = srv.PositionHistoryRepository().GetHistoryWhenUpdated(oldPosition.PositionId,
                            oldPosition.StatusId, oldPosition.StatusId,
                            sb, "BasicDetails", userName);
                        }
                    }

                //save to db
                oldPosition.PositionNumber = newPosition.PositionNumber;
                oldPosition.Description = newPosition.Description;
                oldPosition.ReportToPositionId = newPosition.ReportToPositionId;
                oldPosition.UnitId = newPosition.UnitId;
                oldPosition.PositionLevelId = newPosition.PositionLevelId;
                oldPosition.LocationId = newPosition.LocationId;

                oldPosition.DivisionOverview = newPosition.DivisionOverview;

                if(!string.IsNullOrWhiteSpace(history.Action)) {
                    //attach history
                    oldPosition.PositionHistories.Add(history);
                    }

                this.Update(oldPosition);

                if(reportToChanged) {
                    var allChilds = GetChildTree(oldPosition.PositionId);
                    if(allChilds.Count() <= 200) {
                        foreach(var childId in allChilds) {
                            this.UpdatePositionHeirarchyPath(childId);
                            }
                        }
                    AppCacheHelper.Expire(Enums.CacheRegion.Position);
                    }


                return oldPosition;
                }
            }

        private void ValidatePositionObj(Position newPosition) {
            if(string.IsNullOrEmpty(newPosition.PositionNumber)) {
                var msg = MessageHelper.NullPleaseEnterMessage("valid position number");
                throw new InvalidOperationException(msg);
                }
            /*if (newPosition.UnitChiefPositionId==null || newPosition.UnitChiefPositionId <= 0)
        {
            throw new InvalidOperationException("Sorry, Unit chief cann't be null");
        }*/
            /* else if (newPosition.UnitId <=0)
        {
            throw new InvalidOperationException("Sorry, Unit cann't be null");
        }
        else if (newPosition.PositionLevelId<=0)
        {
            throw new InvalidOperationException("Sorry, Position level can't be null");
        }
        else if (newPosition.LocationId<=0)
        {
            throw new InvalidOperationException("Sorry, Location can't be null");
        }*/

            }

        public IQueryable<Position> FilterDisplayedPositions(JQueryDatatableParamPositionExtension arg,
            IQueryable<Position> positions) {
            IQueryable<Position> displayedPositions;
            SearchArg searchArgs;

            if(arg.StatusCode == null || arg.StatusCode.Length == 0)
                if(arg.StatusId != 0) {
                    arg.StatusCode = new string[] { arg.StatusId.ToString() };
                    }


            if(arg.DirectorateId != 0 || arg.UnitId != 0
                || arg.BusinessUnitId != 0 || !string.IsNullOrWhiteSpace(arg.DivisionCode) ||
                arg.PosStatusCode != null || arg.StatusCode != null) {


                if(arg.PosStatusCode != null) {

                    arg.PosStatusCode = arg.PosStatusCode[0] != "" ? arg.PosStatusCode[0].Split(',') : null;
                    }

                if(arg.StatusCode != null) {
                    arg.StatusCode = arg.StatusCode[0] != "" ? arg.StatusCode[0].Split(',') : null;
                    }



                displayedPositions = this.AdvancedFilterPositions(positions, arg);

                if(!string.IsNullOrWhiteSpace(arg.sSearch)) {
                    searchArgs = new SearchArg { Search = arg.sSearch, };
                    displayedPositions = this.FilterPositions(displayedPositions, searchArgs);
                    }

                displayedPositions = CustomOrderBy.CustomSort(displayedPositions, arg);
                }
            else if(!string.IsNullOrWhiteSpace(arg.sSearch)) {
                searchArgs = new SearchArg { Search = arg.sSearch };
                displayedPositions = this.FilterPositions(positions, searchArgs);
                }
            else {
                displayedPositions = CustomOrderBy.CustomSort(positions, arg);
                }
            return displayedPositions;
            }

        public IQueryable<Position> FilterDisplayedApprovalListPositions(JQueryDatatableParamPositionExtension arg,
            IQueryable<Position> positions) {
            IQueryable<Position> displayedPositions;
            SearchArg searchArgs;
            if(arg.DirectorateId != 0 || arg.UnitId != 0
                || arg.BusinessUnitId != 0 || !string.IsNullOrWhiteSpace(arg.DivisionCode) ||
                arg.PosStatusCode != null || arg.StatusCode != null) {


                if(arg.PosStatusCode != null) {

                    arg.PosStatusCode = arg.PosStatusCode[0] != "" ? arg.PosStatusCode[0].Split(',') : null;
                    }

                if(arg.StatusCode != null) {
                    arg.StatusCode = arg.StatusCode[0] != "" ? arg.StatusCode[0].Split(',') : null;
                    }



                displayedPositions = this.AdvancedFilterPositions(positions, arg);

                if(!string.IsNullOrWhiteSpace(arg.sSearch)) {
                    searchArgs = new SearchArg { Search = arg.sSearch, };
                    displayedPositions = this.FilterPositions(displayedPositions, searchArgs);
                    }

                displayedPositions = CustomOrderBy.CustomSort(displayedPositions, arg);
                }
            else if(!string.IsNullOrWhiteSpace(arg.sSearch)) {
                searchArgs = new SearchArg { Search = arg.sSearch };
                displayedPositions = this.FilterPositions(positions, searchArgs);
                }
            else {
                displayedPositions = CustomOrderBy.CustomSort(positions, arg);
                }
            return displayedPositions;
            }

        private IQueryable<Position> AdvancedFilterPositions(IQueryable<Position> positions,
            JQueryDatatableParamPositionExtension positionArg) {
            var filteredPosition = positions;
            if(!string.IsNullOrWhiteSpace(positionArg.DivisionCode)) {
                filteredPosition =
                    filteredPosition.Where(
                        ent => ent.Unit.BusinessUnit.Directorate.ExecutiveCod == positionArg.DivisionCode);
                }
            if(positionArg.DirectorateId.ToString() != "0") {
                filteredPosition =
                    filteredPosition.Where(ent => ent.Unit.BusinessUnit.DirectorateId == positionArg.DirectorateId);
                }
            if(positionArg.BusinessUnitId.ToString() != "0") {
                filteredPosition =
                    filteredPosition.Where(
                        ent => ent.Unit.BUnitId == positionArg.BusinessUnitId);
                }

            if(positionArg.UnitId.ToString() != "0") {
                filteredPosition =
                    filteredPosition.Where(ent => ent.Unit.UnitId == positionArg.UnitId);
                }


            if(positionArg.StatusCode != null) {
                var intarr = positionArg.StatusCode.Select(Int32.Parse).ToList();
                filteredPosition =
                    filteredPosition.Where(
                        ent => intarr.Contains(ent.StatusId));
                }
            if(positionArg.PosStatusCode != null) {
                filteredPosition =
                    filteredPosition.Where(
                        ent =>
                            positionArg.PosStatusCode.Contains(
                                ent.PositionInformation.PositionStatusValue.PosStatusCode));
                }
            return filteredPosition.OrderBy(e => e.PositionId);
            }

        public SelectListItemExtension[] GetTeamMembersOrderByMaxRate(int unitId) {

            var posList =
                base.List()
                    .Include(ent => ent.RolePositionDescription.Grade)
                    .Where(
                        p =>
                            p.UnitId == unitId && p.StatusId != (int)Enums.StatusValue.Inactive &&
                            p.StatusId != (int)Enums.StatusValue.Deleted &&
                            p.StatusId != (int)Enums.StatusValue.Draft)
                    .OrderByDescending(l => l.RolePositionDescription.Grade.AwardMaxRates);
            var topPosition = posList.FirstOrDefault();
            var unitPositions = posList
                .ToArray()
                .Select(
                    p =>
                        new SelectListItemExtension {
                            Value = p.PositionId.ToString(),
                            Text = p.PositionNumber.ToString() + " - " + p.PositionTitle,
                            Selected = p.PositionId == topPosition.PositionId
                            })
                .ToArray();
            return unitPositions;
            }

        public IEnumerable<Position> ListForLinkedPositions() {
            return this.List()
                .Include(ent => ent.RolePositionDescription).ToList();

            }

        public IEnumerable<Position> GetAllLinkedPositionsById(int rolePositionDescId) {
            return ListForLinkedPositions().Where(r => r.RolePositionDescriptionId == rolePositionDescId);
            }

        public int GetAllLinkedPositionCount(int rolePositionDescId) {
            return ListForLinkedPositions().Count(l => l.RolePositionDescriptionId == rolePositionDescId);
            }

        [Unity.Attributes.Dependency]
        public PositionDescriptionRepository PositionDescriptionRepository { get; set; }

        [Unity.Attributes.Dependency]
        public RoleDescriptionRepository RoleDescriptionRepository { get; set; }

        [Unity.Attributes.Dependency]
        public RolePositionDescriptionRepository RolePositionDescriptionRepository { get; set; }

        [Unity.Attributes.Dependency]
        public PositionHistoryRepository PositionHistoryRepository { get; set; }


        public Position GetPositionById(int id) {
            return ListForPositionInfo().SingleOrDefault(l => l.PositionId == id);
            }

        public Position GetBasePositionById(int id) {
            return base.List().SingleOrDefault(l => l.PositionId == id);
            }


        public Position GetPositionForSummary(int id) {
            return ListForPdRoleDesc().FirstOrDefault(p => p.PositionId == id);
            }


        #region Clone

        public IQueryable<Position> ListForClonePosition() {
            return base.List()
                .Include(ent => ent.PositionInformation)
                .Include(ent => ent.RolePositionDescription)
                .Include(ent => ent.CostCentreDetail);
            }

        public Position GetPositionToClone(int id) {
            return ListForClonePosition().SingleOrDefault(p => p.PositionId == id);
            }

        public void ClonePosition(Position position, PositionInformation posInfo,
            CostCentreDetail costCentre, string sourcePositionNumber) {
            //create position, retrieve the new id, then create PositionInfo, CostCentre
            //History
            var srv = new ServiceRepository(this.RepositoryFactory);

            position.StatusId = (int)Enums.StatusValue.Draft;

            this.Insert(position);

            posInfo.PositionId = position.PositionId;
            srv.PositionInformationRepository().Insert(posInfo);

            costCentre.PositionId = position.PositionId;
            srv.CostCentreDetailRepository().Insert(costCentre);

            //add to history
            srv.PositionHistoryRepository().LogHistoryOnClonePositioin(position, sourcePositionNumber, "Test");

            }

        public int ProcessClonePosition(Position sourcePos, string newPositionNumber, int newRolePositionDescid,
            string userName) {
            //clone the whole lot of the selected position (except use the new position#), positionInfo, 
            //costCentre, RPD,RD/PD, PositionHistory

            var newPosition = new Position();

            var newPosInfo = new PositionInformation();
            var newCostCentre = new CostCentreDetail();

            if(sourcePos != null) {
                newPosition.PositionNumber = newPositionNumber;
                newPosition.RolePositionDescriptionId = newRolePositionDescid;
                newPosition.ReportToPositionId = sourcePos.ReportToPositionId;
                newPosition.UnitId = sourcePos.UnitId;
                newPosition.PositionTitle = sourcePos.PositionTitle;
                newPosition.Description = sourcePos.Description;
                newPosition.PositionLevelId = sourcePos.PositionLevelId;
                newPosition.StatusId = (int)Enums.StatusValue.Draft;
                newPosition.PositionPath = sourcePos.PositionPath;
                newPosition.LocationId = sourcePos.LocationId;
                newPosition.CreatedDate = DateTime.Now;
                newPosition.LastModifiedDate = DateTime.Now;
                newPosition.CreatedBy = userName;
                newPosition.LastModifiedBy = userName;

                //PositionInfo
                newPosInfo.PositionId = newPosition.PositionId;
                newPosInfo.OccupationTypeCode = sourcePos.PositionInformation.OccupationTypeCode;
                newPosInfo.OlderPositionNumber1 = sourcePos.PositionInformation.OlderPositionNumber1;
                newPosInfo.OlderPositionNumber2 = sourcePos.PositionInformation.OlderPositionNumber2;
                newPosInfo.OlderPositionNumber3 = sourcePos.PositionInformation.OlderPositionNumber3;
                newPosInfo.SchNumber = sourcePos.PositionInformation.SchNumber;
                newPosInfo.PositionTypeCode = sourcePos.PositionInformation.PositionTypeCode;
                newPosInfo.EmployeeTypeCode = sourcePos.PositionInformation.EmployeeTypeCode;
                newPosInfo.TrimLink = string.Empty;
                newPosInfo.PositionEndDate = sourcePos.PositionInformation.PositionEndDate;
                newPosInfo.PositionFTE = sourcePos.PositionInformation.PositionFTE;
                newPosInfo.PositionStatusCode = sourcePos.PositionInformation.PositionStatusCode;
                newPosInfo.OccupationTypeCode = sourcePos.PositionInformation.OccupationTypeCode;

                //CostCentre
                newCostCentre.PositionId = newPosition.PositionId;
                newCostCentre.CostCentre = sourcePos.CostCentreDetail.CostCentre;
                newCostCentre.Fund = sourcePos.CostCentreDetail.Fund;
                newCostCentre.PayrollWBS = sourcePos.CostCentreDetail.PayrollWBS;
                newCostCentre.RCCJDEPayrollCode = sourcePos.CostCentreDetail.RCCJDEPayrollCode;
                newCostCentre.GLAccount = sourcePos.CostCentreDetail.GLAccount;
                newCostCentre.LastUpdatedBy = userName;
                newCostCentre.LastUpdatedDate = DateTime.Now;

                ClonePosition(newPosition, newPosInfo, newCostCentre, sourcePos.PositionNumber);
                return newPosition.PositionId;
                }
            return 0;
            }

        #endregion

        public bool HasPositionNumber(string positionNumber) {
            var position = base.List().FirstOrDefault(p => p.PositionNumber == positionNumber);
            if(position != null) {
                return true;
                }
            return false;
            }

        public List<int> GetChildTree(int parentId) {
            var result = new List<int>();
            GetChildList(parentId, List().Where(pos => pos.StatusId != (int)Enums.StatusValue.Deleted), ref result);
            return result;
            }

        private bool GetChildList(int parentId, IQueryable<Position> all, ref List<int> result) {

            var childList =
                all.Where(pos => pos.ReportToPositionId == parentId && (pos.ReportToPositionId != pos.PositionId))
                    .ToList();
            if(!childList.Any()) return false;

            foreach(var child in childList) {
                result.Add(child.PositionId);
                GetChildList(child.PositionId, all, ref result);

                }


            return true;
            }

        public void UpdateAllPositionHierarchy() {
            var ctx = this.UnitOfWork.DbContext as SampleProjectTemplateEntities;
            if(ctx != null) ctx.UpdateAllPositionHierarchy();

            Refresh(List());
            }

        public XDocument XmlSerialize(Position position) {
            if(position.RolePositionDescription.IsPositionDescription) {

                if (position.PositionId != Enums.Cnt.GenericCode)//Generic for Trim
                {
                    position =
                        this.ListForPdRoleDesc()
                            .Include(
                                p =>
                                    p.RolePositionDescription.PositionDescription.PositionFocusCriterias.Select(
                                        pf => pf.LookupFocusGradeCriteria))
                            .SingleOrDefault(p => p.PositionId == position.PositionId);
                }
                var doc = new XDocument(new XElement("Position",
                    new XAttribute("Division",
                        position.Unit.BusinessUnit.Directorate.Executive.ExecutiveTitle.Trim()),
                    new XAttribute("Directorate", position.Unit.BusinessUnit.Directorate.DirectorateName),
                    new XAttribute("BusinessUnit", position.Unit.BusinessUnit.BUnitName),
                    new XAttribute("Team", position.Unit.UnitName),
                    new XAttribute("PositionNumber", position.PositionNumber),
                    new XAttribute("Title", position.PositionTitle),
                    new XAttribute("GradeCode", position.RolePositionDescription.Grade.GradeCode),
                    new XAttribute("BriefRoleStatement", position.RolePositionDescription.PositionDescription.BriefRoleStatement),
                    new XElement("StatementOfDuties", position.RolePositionDescription.PositionDescription.StatementOfDuties),
                    new XElement("SelectionCriteria"),
                    new XElement("Admin")
                    ));

                var criteriaEl = doc.Descendants("SelectionCriteria").FirstOrDefault();
                var focusCriteriaList = GetfocusCriteriaList(position);

                foreach(var focusCriteria in focusCriteriaList) {
                    if (focusCriteria.LookupFocusGradeCriteria.Focus.FocusName.Equals("custom",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        criteriaEl.Add(new XElement("Criteria", focusCriteria.LookupCustomContent));

                        }
                    else
                    {
                    criteriaEl.Add(new XElement("Criteria", focusCriteria.LookupFocusGradeCriteria.SelectionCriteria.Criteria));

                        }

                    }

                var adminEl = doc.Descendants("Admin").FirstOrDefault();
                adminEl.Add(new XElement("Logo", ProjectTemplateSettings.Site.PdfTemplatePath + "dec-pdf.png"));
                adminEl.Add(new XElement("DocNumber", position.RolePositionDescription.DocNumber));
                adminEl.Add(new XElement("LastUpdate", position.LastModifiedDate.ToShortDateString()));
                if(position.RolePositionDescription.DateOfApproval.HasValue) {
                    adminEl.Add(new XElement("ApprovedDate", position.RolePositionDescription.DateOfApproval.Value.ToShortDateString()));
                    }


                return doc;
                }


            return null;
            }

        private IEnumerable<PositionFocusCriteria> GetfocusCriteriaList(Position position) {
            var serviceRep = new ServiceRepository(this.RepositoryFactory);
            var rolePosDesc = serviceRep.RolePositionDescriptionRepository()
                .ListForPositionDescriptions()
                .SingleOrDefault(rp => rp.RolePositionDescId == position.RolePositionDescriptionId);

            var focusCriteria =
                rolePosDesc.PositionDescription.PositionFocusCriterias
                    //.Where(c => c.LookupCustomContent == null)
                    .OrderBy(c => c.LookupFocusGradeCriteria.Focus.OrderList);
            return focusCriteria;
            }
        }
    }



