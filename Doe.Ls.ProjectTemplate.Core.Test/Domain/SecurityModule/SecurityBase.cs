using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Castle.Core.Logging;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using HP.HPTRIM.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule
    {
    public abstract class SecurityBase : TestBase
        {
        protected Enums.UserRole CurrentUserRole { get; set; }
        protected IEnumerable<SysUser> GetSampleSysUsers(Enums.UserRole role, IRepositoryFactory factory, bool only = true, int max = 10)
            {
            var srv = new ServiceRepository(factory);
            var roleId = role.ToInteger();
            var guestId = Enums.UserRole.Guest.ToInteger();
            var doeId = Enums.UserRole.DoEUser.ToInteger();
            if(role == Enums.UserRole.Guest) throw new InvalidOperationException("Guest role is not allowed");

            if(only)
                {
                if(roleId == doeId)
                    {

                    return
                        srv.SysUserRepository().List()
                            .Where(u => u.SysUserRoles.Count == 0 || u.SysUserRoles.All(su => su.RoleId == doeId)).Take(max);
                    }

                return srv.SysUserRepository()
                    .List()
                    .Where(
                        u => u.SysUserRoles.Any(ur => ur.RoleId == roleId) && u.SysUserRoles.Count == 1).Take(max);


                }
            else
                {
                return srv.SysUserRepository().List().Where(u => u.SysUserRoles.Any(ur => ur.RoleId == roleId)).Take(max);
                }


            }
        protected IEnumerable<UserInfoExtension> GetSampleUsers(Enums.UserRole role, IRepositoryFactory factory, bool only = true, int max = 10)
            {
            var result = GetSampleSysUsers(role, factory, only, max).ToList();
           
            if (!result.Any())
            {
                GenerateSampleUser(role, factory);
                result = GetSampleSysUsers(role, factory, only, max).ToList();
                }
            foreach(var sysUser in result)
                {
                yield return UserInfoExtension.MapSysUser(sysUser, factory);
                }

            }

        protected UserInfoExtension GenerateSampleUser(Enums.UserRole role, IRepositoryFactory factory)
            {
            var srv = new ServiceRepository(factory);
            var srvUser = srv.SysUserRepository();
            var sysUser = srvUser.GetSysUserByUserName(role.ToString());
            if(sysUser == null)
                {
                srvUser.Insert(new SysUser
                    {
                    UserId = role.ToString(),
                    Email = $"{role}@det.unit.test.com",
                    Active = true,
                    FirstName = $"{role} FN",
                    LastName = $"{role} LN",
                    Note = "Generated from unit test",
                    CreatedBy = "Unit.Test",
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = "Unit.Test",
                    LastModifiedDate = DateTime.Now
                    });
                sysUser = srvUser.GetSysUserByUserName(role.ToString());
                }

            if(sysUser.SysUserRoles.Any(r => r.RoleId == (int)role) && sysUser.SysUserRoles.Count == 1)
                {
                if(sysUser.SysUserRoles.First().IsActive()) return UserInfoExtension.MapSysUser(sysUser, factory);

                }

            sysUser.SysUserRoles.Clear();
            srvUser.Update(sysUser);
            var validUnit = srv.UnitRepository().List().Skip(10).FirstOrDefault();

            var sysUserRole = new SysUserRole
                {
                RoleId = (int)role,
                UserId = sysUser.UserId,
                ActiveFrom = DateTime.Now.AddYears(-1),
                CreatedBy = "unit.test",
                CreatedDate = DateTime.Now
                };
            sysUserRole.UpdateSignature(factory);
            switch(role)
                {
                case Enums.UserRole.SystemAdministrator:
                    sysUserRole.OrgLevelId = (int)Enums.OrgLevel.Application;
                    sysUserRole.StructureId = "-1";
                    break;
                case Enums.UserRole.Administrator:
                    sysUserRole.OrgLevelId = (int)Enums.OrgLevel.Application;
                    sysUserRole.StructureId = "-1";
                    break;
                case Enums.UserRole.PowerUser:
                    sysUserRole.OrgLevelId = (int)Enums.OrgLevel.Application;
                    sysUserRole.StructureId = "-1";
                    break;
                case Enums.UserRole.DoEUser:
                    sysUserRole.OrgLevelId = (int)Enums.OrgLevel.NA;
                    sysUserRole.StructureId = "-1";
                    break;
                case Enums.UserRole.Guest:
                    sysUserRole.OrgLevelId = (int)Enums.OrgLevel.NA;
                    sysUserRole.StructureId = "-1";
                    break;
                case Enums.UserRole.BusinessUnitDataEntry:
                case Enums.UserRole.BusinessUnitAuthor:

                    sysUserRole.OrgLevelId = (int)Enums.OrgLevel.BusinessUnit;
                    sysUserRole.StructureId = validUnit.BUnitId.ToString();
                    break;
                case Enums.UserRole.DirectorateDataEntry:

                case Enums.UserRole.DirectorateEndorser:
                    sysUserRole.OrgLevelId = (int)Enums.OrgLevel.Directorate;
                    sysUserRole.StructureId = validUnit.BusinessUnit.DirectorateId.ToString();

                    break;
                case Enums.UserRole.DivisionEditor:
                case Enums.UserRole.DivisionApprover:
                    sysUserRole.OrgLevelId = (int)Enums.OrgLevel.Directorate;
                    sysUserRole.StructureId = validUnit.BusinessUnit.Directorate.ExecutiveCod;

                    break;
                case Enums.UserRole.HRDataEntry:
                    sysUserRole.OrgLevelId = (int)Enums.OrgLevel.NA;
                    sysUserRole.StructureId = "-1";
                    break;
                }


            sysUser.SysUserRoles.Add(sysUserRole);
            srvUser.Update(sysUser);

            sysUser = srvUser.GetSysUserByUserName(sysUser.UserId);
            return UserInfoExtension.MapSysUser(sysUser, factory);
            }

        protected UserInfoExtension GetSampleUser(Enums.UserRole role, IRepositoryFactory factory, bool only = true)
            {
            var result = GetSampleUsers(role, factory, only, 1);

            return result.FirstOrDefault();
            }

        protected IWorkflowEngine GetWorkflowEngine(Enums.StatusValue status, bool userHasTheSameStructure,bool isPositionDescription=true)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var admin = GenerateSampleUser(this.CurrentUserRole, factory);


            Position pos = null;

            pos = userHasTheSameStructure ? CreateDatabaseTestPositionThatUserHasAccess(factory, admin, status) : CreateDatabaseTestPositionThatUserHasNoAccess(factory, admin, status);
                if (pos == null) return null;
            return WorkflowEngineFactory.CreatEngine(pos, admin, factory);

            }
        protected IWorkflowEngine GetWorkflowEngine(Enums.StatusValue positionStatus, Enums.StatusValue positionDescriptionStatus, bool isPositionDescription = true)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var admin = GenerateSampleUser(this.CurrentUserRole, factory);


            Position pos = null;

            pos = CreateDatabaseTestPositionThatUserHasAccess(factory, admin, positionStatus, positionDescriptionStatus, isPositionDescription);
            if (pos == null) return null;
            var wf= WorkflowEngineFactory.CreatEngine(pos, admin, factory);

            return wf;

            }
        
        protected IWorkflowEngine GetTrimtWorkflowEngine(bool isPositionDescription = true) {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);
            var trimRecordSrv = service.TrimRecordRepository();
            var index = new Random(DateTime.Now.Millisecond).Next(0, 5);
            var docNumber =GetTrimDocNumberList(isPositionDescription)[index];

            
            var rpd =
                trimRecordSrv.PositionRepository.RolePositionDescriptionRepository.GetRolePositionDescByDocumentNumber(
                    docNumber).FirstOrDefault();

            if (isPositionDescription)
            {
                if (rpd == null)
                {
                    var trimRecordInfo = trimRecordSrv.TrimService.GetRecordByRecordNumber(docNumber);
                    TestCreatePositionDescriptionForTrim(service, docNumber, trimRecordInfo.Title);
                }
                var pd = service.PositionDescriptionRepository().GetPositionDescriptionById(rpd.RolePositionDescId);
                var wf = WorkflowEngineFactory.CreatEngine(pd,
                    GetSampleUser(Enums.UserRole.Administrator, factory, true), factory);

                if (!wf.PositionRdPdTasks.IsValid())
                {
                    CompleteStepsUntoLive(pd.RolePositionDescription,service);
                    }



                return wf;
            }
            else
            {
                if(rpd == null) {
                    var trimRecordInfo = trimRecordSrv.TrimService.GetRecordByRecordNumber(docNumber);
                    TestCreateRoleDescriptionForTrim(service, docNumber, trimRecordInfo.Title);

                    rpd =trimRecordSrv.PositionRepository.RolePositionDescriptionRepository.GetRolePositionDescByDocumentNumber(
                    docNumber).FirstOrDefault();
                    }
                var rd = service.RoleDescriptionRepository().GetRoleDescriptionById(rpd.RolePositionDescId);
                var wf = WorkflowEngineFactory.CreatEngine(rd,
                    GetSampleUser(Enums.UserRole.Administrator, factory, true), factory);

                if(!wf.PositionRdPdTasks.IsValid()) {

                    CompleteStepsUntoLive(rd.RolePositionDescription, service);
                    }



                return wf;

                }
        }

        protected Position CreateDatabaseTestPositionThatUserHasAccess(MockRepositoryFactory factory, UserInfoExtension userInfo, Enums.StatusValue positionStatus, Enums.StatusValue positionDescriptionStatus, bool isPositionDescription=true)
            {
            var srv = new ServiceRepository(factory);            
            Unit unit = null;
            if(userInfo.HasAdminOrPowerRole())
                {
                unit = srv.UnitRepository().List().FirstOrDefault();

                }
            else if(userInfo.IsBusinessUnitAuthor || userInfo.IsBusinessUnitDataEntry)
                {
                var structureId =
                        userInfo.ActiveRoleOrgLevelList.Where(
                            r => r.IsActive && (r.RoleId == (int)Enums.UserRole.BusinessUnitAuthor || r.RoleId == (int)Enums.UserRole.BusinessUnitDataEntry))
                            .Select(r => r.StructureId).FirstOrDefault();
                var bu = srv.BusinessUnitRepository().GetBUnitById(structureId.ToInteger());
                unit = bu.Units.FirstOrDefault();
                if(unit == null) Assert.Inconclusive($"Business unit  {bu} has no units");
                }
            else if(userInfo.IsDirectorateDataEntry || userInfo.IsDirectorateEndorser)
                {
                var structureId =
                    userInfo.ActiveRoleOrgLevelList.Where(
                        r =>
                            r.IsActive &&
                            (r.RoleId == (int)Enums.UserRole.DirectorateEndorser ||
                             r.RoleId == (int)Enums.UserRole.DirectorateDataEntry))
                        .Select(r => r.StructureId).FirstOrDefault();

                var dir = srv.DirectorateRepository()
                    .GetDirectorateById(structureId.ToInteger());
                unit =
                    dir.BusinessUnits.SelectMany(bu => bu.Units)
                        .FirstOrDefault();
                if(unit == null) Assert.Inconclusive($" Directorate {dir} has no units");
                }
            else if(userInfo.IsDivisionApprover || userInfo.IsDivisionEditor)
                {
                var structureId =
                    userInfo.ActiveRoleOrgLevelList.Where(
                        r =>
                            r.IsActive &&
                            (r.RoleId == (int)Enums.UserRole.DivisionEditor ||
                             r.RoleId == (int)Enums.UserRole.DivisionApprover))
                        .Select(r => r.StructureId).FirstOrDefault();
                var div = srv.ExecutiveRepository()
                    .GetExecutiveByCode(structureId);
                unit =

                        div.Directorates.SelectMany(d => d.BusinessUnits.SelectMany(bu => bu.Units))
                        .FirstOrDefault();
                if(unit == null) Assert.Inconclusive($" Division {div} has no units");

                }


            return CreateDatabaseTestPositionWithPositionDescription(factory, unit, userInfo, positionStatus, positionDescriptionStatus, isPositionDescription);
            }

        private Position CreateDatabaseTestPositionWithPositionDescription(IRepositoryFactory factory, Unit unit, UserInfoExtension userInfo, Enums.StatusValue positionStatus, Enums.StatusValue positionDescriptionStatus, bool isPositionDescription=true)
        {
            var srv = new ServiceRepository(factory);
           
            var rep =
                srv.PositionRepository();
            unit = srv.UnitRepository().GetUnitById(unit.UnitId);
            var parentPos =
                rep
                    .BaseList(
                        )
                    .FirstOrDefault(p => p.StatusId == (int)Enums.StatusValue.Approved ||
                            p.StatusId == (int)Enums.StatusValue.Imported && p.PositionId > 10);
            IEnumerable<RolePositionDescription> rolePosList=null;
            if (isPositionDescription)
            {
                rolePosList =
                    rep.RolePositionDescriptionRepository.ListForPositionDescriptions()
                        .Where(rp => rp.StatusId == (int) positionDescriptionStatus && rp.PositionDescription != null);
            }
            else
            {
                rolePosList =
                    rep.RolePositionDescriptionRepository.ListForRoleDescriptions()
                        .Where(rp => rp.StatusId == (int) positionDescriptionStatus && rp.RoleDescription != null);
            }
            RolePositionDescription rolPos = null;
            switch (positionDescriptionStatus)
            {
                case Enums.StatusValue.Deleted:
                    break;
                case Enums.StatusValue.Approved:                    
                case Enums.StatusValue.Imported:
                case Enums.StatusValue.Submitted:                    
                case Enums.StatusValue.Endorsed:
                    rolPos = rolePosList.FirstOrDefault();
                    break;
                case Enums.StatusValue.Draft:
                    foreach (var rolePositionDescription in rolePosList.Take(300).ToArray())
                    {
                        var pd =
                            srv.PositionDescriptionRepository()
                                .LoadPositionDescById(rolePositionDescription.PositionDescription.PositionDescriptionId);
                        if (rolePositionDescription.PositionDescription != null)
                        {
                            if (
                                
                                WorkflowEngineFactory.CreatEngine(pd, userInfo,
                                    factory).PositionRdPdTasks.IsValid())
                            {
                                rolPos = rolePositionDescription;
                                break;
                                }
                        }
                    }


                    break;

            }

            if (rolPos == null)
            {
                return null;
            }

            var locId = unit.BusinessUnit.Directorate.Locations.Any() ? unit.BusinessUnit.Directorate.Locations.First().LocationId : Enums.Cnt.Na;

            var pos = new Position {
                RolePositionDescriptionId = rolPos.RolePositionDescId,
                Description = GenerateString("Description", 300),
                CreatedBy = userInfo.UserName,
                CreatedDate = DateTime.Now,
                UnitId = unit.UnitId,
                LocationId = locId,
                ReportToPositionId = parentPos.PositionId,
                PositionLevelId = (int)Enums.PositionLevel.Position,
                PositionNumber = GenerateString("POS-NO").Substring(0, 9),
                };

            pos = rep.CreateOrUpdatePosition(pos);
            pos.StatusId = (int) positionStatus;
            FillTestDataToMakeItValid(pos, factory);
            pos = rep.CreateOrUpdatePosition(pos);
            return rep.GetPositionById(pos.PositionId);
        }

        protected Position CreateDatabaseTestPosition(IRepositoryFactory factory, Unit unit, UserInfoExtension user, Enums.StatusValue status = Enums.StatusValue.Draft, bool validPosition = true)
        {
            var srv = new ServiceRepository(factory);
            var rep = srv.PositionRepository();
            unit = srv.UnitRepository().GetUnitById(unit.UnitId);
            var parentPos = rep.BaseList().FirstOrDefault(p => p.StatusId == (int) Enums.StatusValue.Approved || p.StatusId == (int) Enums.StatusValue.Imported && p.PositionId > 10);
            var rolPos = rep.RolePositionDescriptionRepository.ListForPositionDescriptions().FirstOrDefault(rp => rp.StatusId == (int) Enums.StatusValue.Imported || rp.StatusId == (int) Enums.StatusValue.Approved);

            var locId = unit.BusinessUnit.Directorate.Locations.Any() ? unit.BusinessUnit.Directorate.Locations.First().LocationId : Enums.Cnt.Na;

            var pos = new Position
            {
                RolePositionDescriptionId = rolPos.RolePositionDescId, Description = GenerateString("Description", 300), CreatedBy = user.UserName, CreatedDate = DateTime.Now, UnitId = unit.UnitId, LocationId = locId, ReportToPositionId = parentPos.PositionId, PositionLevelId = (int) Enums.PositionLevel.Position, PositionNumber = GenerateString("POS-NO").Substring(0, 9),
            };

            pos = rep.CreateOrUpdatePosition(pos);
            pos.StatusId = (int) status;
            if (validPosition)
            {
                FillTestDataToMakeItValid(pos, factory);
            }


            pos = rep.CreateOrUpdatePosition(pos);
            return rep.GetPositionById(pos.PositionId);
        }

        private void FillTestDataToMakeItValid(Position pos, IRepositoryFactory factory)
        {
            var srv = new ServiceRepository(factory);
            if (string.IsNullOrWhiteSpace(pos.PositionNumber))
                pos.PositionNumber = GenerateString("POS-NO").Substring(0, 9);
            if (string.IsNullOrWhiteSpace(pos.Description))
                pos.Description = GenerateString("Description", 300);
            if (pos.UnitId == Enums.Cnt.Na)
            {
                var unit = srv.UnitRepository().List().FirstOrDefault(t => !t.UnitName.ToLower().Contains("UNIT_TEST".ToLower()));
                if (unit != null)
                    pos.UnitId = unit.UnitId;
            }
            if (pos.LocationId == Enums.Cnt.Na)
            {
                var location = srv.LocationRepository().GetLocationsByUnit(pos.UnitId).FirstOrDefault();
                if (location != null)
                    pos.LocationId = location.LocationId;
                else
                {
                    location = srv.LocationRepository().BaseList().Skip(5).FirstOrDefault();
                    if (location != null)
                        pos.LocationId = location.LocationId;
                }
            }

            if (pos.ReportToPositionId == Enums.Cnt.Na)
            {
                var parent = srv.PositionRepository().ListForReportTo().FirstOrDefault(p => p.PositionId != Enums.Cnt.Na);
                if (parent != null)
                    pos.ReportToPositionId = parent.PositionId;
            }

            if (pos.CostCentreDetail == null)
            {
                pos.CostCentreDetail = new CostCentreDetail
                {
                    PositionId = pos.PositionId, CostCentre = "312312", Fund = "2344", GLAccount = "231234", LastUpdatedBy = pos.LastModifiedBy, LastUpdatedDate = pos.LastModifiedDate, RCCJDEPayrollCode = "4234453", PayrollWBS = "4r23253"
                };
            }
            if (pos.PositionInformation == null)
            {
                pos.PositionInformation = new PositionInformation
                {
                    PositionTypeCode = "O", PositionStatusCode = "V", EmployeeTypeCode = "PF", OccupationTypeCode = "E", PositionFTE = 1.25,
                };
            }
        }

        protected Position CreateDatabaseTestPositionThatUserHasAccess(IRepositoryFactory factory, UserInfoExtension userInfo, Enums.StatusValue status = Enums.StatusValue.Draft)
        {
            var srv = new ServiceRepository(factory);
            Unit unit = null;
            if (userInfo.HasAdminOrPowerRole())
            {
                unit = srv.UnitRepository().List().FirstOrDefault();
            }
            else if (userInfo.IsBusinessUnitAuthor || userInfo.IsBusinessUnitDataEntry)
            {
                var structureId = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && (r.RoleId == (int) Enums.UserRole.BusinessUnitAuthor || r.RoleId == (int) Enums.UserRole.BusinessUnitDataEntry)).Select(r => r.StructureId).FirstOrDefault();
                var bu = srv.BusinessUnitRepository().GetBUnitById(structureId.ToInteger());
                unit = bu.Units.FirstOrDefault();
                if (unit == null) Assert.Inconclusive($"Business unit  {bu} has no units");
            }
            else if (userInfo.IsDirectorateDataEntry || userInfo.IsDirectorateEndorser)
            {
                var structureId = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && (r.RoleId == (int) Enums.UserRole.DirectorateEndorser || r.RoleId == (int) Enums.UserRole.DirectorateDataEntry)).Select(r => r.StructureId).FirstOrDefault();

                var dir = srv.DirectorateRepository().GetDirectorateById(structureId.ToInteger());
                unit = dir.BusinessUnits.SelectMany(bu => bu.Units).FirstOrDefault();
                if (unit == null) Assert.Inconclusive($" Directorate {dir} has no units");
            }
            else if (userInfo.IsDivisionApprover || userInfo.IsDivisionEditor)
            {
                var structureId = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && (r.RoleId == (int) Enums.UserRole.DivisionEditor || r.RoleId == (int) Enums.UserRole.DivisionApprover)).Select(r => r.StructureId).FirstOrDefault();
                var div = srv.ExecutiveRepository().GetExecutiveByCode(structureId);
                unit = div.Directorates.SelectMany(d => d.BusinessUnits.SelectMany(bu => bu.Units)).FirstOrDefault();
                if (unit == null) Assert.Inconclusive($" Division {div} has no units");
            }


            return CreateDatabaseTestPosition(factory, unit, userInfo, status);
        }

        protected Position CreateDatabaseTestPositionThatUserHasNoAccess(IRepositoryFactory factory, UserInfoExtension userInfo, Enums.StatusValue status = Enums.StatusValue.Draft)
        {
            var srv = new ServiceRepository(factory);
            Unit unit = null;
            if (userInfo.HasAdminOrPowerRole())
            {
                throw new InvalidOperationException("Administrator have access for all positions");
            }
            else if (userInfo.IsBusinessUnitAuthor || userInfo.IsBusinessUnitDataEntry)
            {
                var structureIdList = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && (r.RoleId == (int) Enums.UserRole.BusinessUnitAuthor || r.RoleId == (int) Enums.UserRole.BusinessUnitDataEntry)).Select(r => r.StructureId).CastToIntegerList();
                unit = srv.BusinessUnitRepository().List().Where(bu => !structureIdList.Contains(bu.BUnitId)).SelectMany(bu => bu.Units).FirstOrDefault();
            }
            else if (userInfo.IsDirectorateDataEntry || userInfo.IsDirectorateEndorser)
            {
                var structureIdList = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && (r.RoleId == (int) Enums.UserRole.DirectorateEndorser || r.RoleId == (int) Enums.UserRole.BusinessUnitDataEntry)).Select(r => r.StructureId).CastToIntegerList();

                unit = srv.DirectorateRepository().List().Where(d => !structureIdList.Contains(d.DirectorateId)).SelectMany(d => d.BusinessUnits.SelectMany(bu => bu.Units)).FirstOrDefault();
            }
            if (userInfo.IsDivisionApprover || userInfo.IsDivisionEditor)
            {
                var structureIdList = userInfo.ActiveRoleOrgLevelList.Where(r => r.IsActive && (r.RoleId == (int) Enums.UserRole.DivisionEditor || r.RoleId == (int) Enums.UserRole.DivisionApprover)).Select(r => r.StructureId).FirstOrDefault();

                unit = srv.ExecutiveRepository().List().Where(e => !structureIdList.Contains(e.ExecutiveCod)).SelectMany(e => e.Directorates.SelectMany(d => d.BusinessUnits.SelectMany(bu => bu.Units))).FirstOrDefault();
            }

            if (unit == null) return null;
            return CreateDatabaseTestPosition(factory, unit, userInfo, status);
        }


        protected void TraceWorkflowScenario(int positionId, string userName)
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var userRepo = srv.SysUserRepository();
            var posRepo = srv.PositionRepository();

            var sysAdmin = userRepo.GetSysUserByUserName(userName);
            var admin = UserInfoExtension.MapSysUser(sysAdmin, factory);

            var position = posRepo.GetPositionById(positionId);
            var engine = WorkflowEngineFactory.CreatEngine(position, UserTaskFactory.GetTask(admin, factory));
            var actions = engine.GetWorkflowObjectActions();
            var priv = engine.GetWorkflowObjectPrivilege(new PrivilegeOptions {GetDownloadPrivilege = true});

            Console.WriteLine($"---- start test user {admin} for position {position} ");

            Console.WriteLine($"User: {admin}");
            Console.WriteLine($"User roles: {admin.DisplayRoles()}");

            Console.WriteLine($"Position: {position}");
            Console.WriteLine($"Position organisation Div. :{position.Unit.BusinessUnit.Directorate.Executive}");
            Console.WriteLine($"Position organisation Directorate. :{position.Unit.BusinessUnit.Directorate}");
            Console.WriteLine($"Position organisation BusinessUnit. :{position.Unit.BusinessUnit}");
            Console.WriteLine($"Position organisation Unit. :{position.Unit}");

            Console.WriteLine($"Privileges: {priv}");
            Console.WriteLine($"Actions:");

            if (actions.Any())
            {
                foreach (var action in actions)
                {
                    Console.WriteLine(action);
                }
            }
            else
            {
                Console.WriteLine("User has no actions");
            }
        }

        protected List<RecordRef> GetChildRecords(string parentDocumentNumber) {
            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();

            var record = trimSrv.GetRecordByRecordNumber(parentDocumentNumber, PropertyIds.RecordRelationshipRelationNumber, PropertyIds.RecordRelationshipRelationType,
                PropertyIds.DatabaseAddHoldToContainedRecord
                );

            if(record != null) {
                return record.ChildRelationships.Select(c => c.RelatedRecord).ToList();


                }




            return null;
            }

        protected Record GetSampleRecord() {



            var srv = this.GetServiceRepository();

            var trimSrv = srv.TrimService();

            var record = trimSrv.GetRecordByUri(3594979);
            if(record != null) return record;

            var recordList = trimSrv.GetRecordList("VLE-TEST-POSITION", max: 5);
            if(recordList.FirstOrDefault() != null) return recordList.FirstOrDefault();

            recordList = trimSrv.GetRecordList("VLE-TEST", max: 5);
            if(recordList.FirstOrDefault() != null) return recordList.FirstOrDefault();

            recordList = trimSrv.GetRecordList("TEST", max: 5);
            if(recordList.FirstOrDefault() != null) return recordList.FirstOrDefault();


            throw new InvalidOperationException("There is no suitable sample ");
            }

        protected string[] GetTrimDocNumberList(bool isPosition=true)
        {
            var pdRdSuffex = isPosition ? "pd" : "rd";
            var doc = XDocument.Load(GetSampleFile($"Samples_{pdRdSuffex}.xml").FullName);
            return doc.Descendants("record").Select(el => el.Attribute("docNumber").Value).ToArray();
            }
        protected FileInfo GetSampleFile(string fileName) {
            return new FileInfo(GetDataFilePath(fileName));
            }

        protected void CompleteStepsUntoLive(RolePositionDescription rdp, ServiceRepository service) {
            var admin = GenerateSampleUser(Enums.UserRole.Administrator, service.RepositoryFactory);

            if (rdp.IsPositionDescription)
            {
                var posDesc = service.PositionDescriptionRepository().GetPositionDescriptionById(rdp.RolePositionDescId);

                var wf = WorkflowEngineFactory.CreatEngine(posDesc, admin, service.RepositoryFactory);

                if (!wf.PositionRdPdTasks.IsValid())
                {
                    posDesc.StatementOfDuties =
                        @"<p><ul><li>Test Statement of duties for Trim</li><li>2</li><li>3</li><li>4</li><li>5</li><li>6</li><li>7</li><li>8</li></ul></p>";


                    var exisitsLookIds = posDesc.PositionFocusCriterias.Select(lgc => lgc.LookupId).ToArray();
                    var result = service.LookupFocusGradeCriteriaRepository()
                        .List()
                        .Where(
                            lgc =>
                                lgc.GradeCode == posDesc.RolePositionDescription.GradeCode &&
                                !exisitsLookIds.Contains(lgc.LookupId)).Take(9 - posDesc.PositionFocusCriterias.Count());

                    foreach (var lgc in result)
                    {
                        posDesc.PositionFocusCriterias.Add(new PositionFocusCriteria
                        {
                            PositionDescriptionId = posDesc.PositionDescriptionId,
                            LookupId = lgc.LookupId,
                            LastModifiedBy = Enums.Cnt.System,
                            PositionDescription = posDesc,
                            LastModifiedDate = DateTime.Now,

                        });

                    }
                    service.PositionDescriptionRepository().Update(posDesc);

                }

                posDesc = service.PositionDescriptionRepository().GetPositionDescriptionById(rdp.RolePositionDescId);
                wf = WorkflowEngineFactory.CreatEngine(posDesc, admin, service.RepositoryFactory);

                if (wf.PositionRdPdTasks.IsValid())
                {

                    var workflowAction = WorkflowAction.MarkAsImported;
                    WorkflowAction.Populate(service.RepositoryFactory, workflowAction);

                    var model = new WorkflowActionModel
                    {
                        ActionId = workflowAction.ActionId,
                        WfObjectId = posDesc.PositionDescriptionId,
                        ObjectType = WorkflowObjectType.PositionDescription,
                        Comment = "From unit test"
                    };



                    wf.ApplyAction(model);

                }



            }
            else
            {
                admin = GenerateSampleUser(Enums.UserRole.Administrator, service.RepositoryFactory);
                var rolDesc = service.RoleDescriptionRepository().GetRoleDescriptionById(rdp.RolePositionDescId);

                var wf = WorkflowEngineFactory.CreatEngine(rolDesc, admin, service.RepositoryFactory);

                var workflowAction = WorkflowAction.MarkAsImported;
                WorkflowAction.Populate(service.RepositoryFactory, workflowAction);

                var model = new WorkflowActionModel {
                    ActionId = workflowAction.ActionId,
                    WfObjectId = rolDesc.RoleDescriptionId,
                    ObjectType = WorkflowObjectType.PositionDescription,
                    Comment = "From unit test"
                    };


                wf.ApplyAction(model);
                }


            }
        }
    }