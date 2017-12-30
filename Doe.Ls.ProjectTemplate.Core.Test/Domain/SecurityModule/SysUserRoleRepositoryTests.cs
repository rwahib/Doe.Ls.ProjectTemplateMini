using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule
    {
    [TestClass]
    public class SysUserRoleRepositoryTests : SecurityBase
        {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }

        [TestMethod]
        public void List()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRoleRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach(var model in rep.List().Take(10))
                {
                Console.WriteLine("{0}", model.ToString());
                }
            }
        [TestMethod]
        public void Create()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRoleRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }
        [TestMethod]
        public void Edit()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRoleRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }
        [TestMethod]
        public void Delete()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRoleRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }

        [TestMethod]
        public void Detail()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRoleRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }

        [TestMethod]
        public void Search()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SysUserRoleRepository();
            if(!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
            }

        [TestMethod]
        [Ignore]
        public void ImportUserRoles()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            factory.RegisterType<IUserIdentityService, DecLsUserIdentityService>();

            var service = new ServiceRepository(factory);

            var sysUserRoleRep = service.SysUserRoleRepository();
            var loginRep = service.LoginService();

            var userList = LoadUserList();

            for(var i = 0; i < userList.Count; i++)
                {
                var userName = userList[i];
                try
                    {
                    loginRep.UserIdentityService.GetUserByPortalId(userName);
                    }
                catch
                    {
                    // Not valid user
                    continue;
                    }


                loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);
                var userInfio = loginRep.GetUserAndCacheItInDb(userName);
                if(i < 10)
                    {

                    if(userInfio.IsRoleInitialised && !userInfio.IsSystemAdmin)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRoleById(userInfio.UserName,
                            Enums.UserRole.SystemAdministrator, Enums.OrgLevel.Application);

                        Assert.IsNull(sysRole, "SysRole must be null for non SysAdmin");
                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);
                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.SystemAdministrator,Enums.OrgLevel.Application);
                        }
                    continue;
                    }
                if(i < 20)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsAdministrator)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRoleById(userInfio.UserName,
                            Enums.UserRole.Administrator, Enums.OrgLevel.Application);

                        Assert.IsNull(sysRole, "SysRole must be null for non Admin");
                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);
                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.Administrator, Enums.OrgLevel.Application);
                        }

                    continue;
                    }


                if(i < 30)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsPowerUser)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRoleById(userInfio.UserName,
                            Enums.UserRole.PowerUser, Enums.OrgLevel.Application);

                        Assert.IsNull(sysRole, "SysRole must be null for non Power user");
                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);
                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.PowerUser, Enums.OrgLevel.Application);
                        }

                    continue;
                    }

                if(i < 40)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsBusinessUnitDataEntry)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRoleById(userInfio.UserName,
                            Enums.UserRole.BusinessUnitDataEntry, Enums.OrgLevel.BusinessUnit);

                        Assert.IsNull(sysRole, "SysRole must be null for non Business unit dataEntry");

                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);

                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.BusinessUnitDataEntry, Enums.OrgLevel.BusinessUnit, service.UnitRepository().List().ToList().First().UnitId.ToString());
                        }

                    continue;
                    }
                if(i < 50)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsBusinessUnitAuthor)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRoleById(userInfio.UserName,
                            Enums.UserRole.BusinessUnitAuthor, Enums.OrgLevel.BusinessUnit);

                        Assert.IsNull(sysRole, "SysRole must be null for non Business unit author");

                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);

                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.BusinessUnitAuthor, Enums.OrgLevel.BusinessUnit, service.UnitRepository().List().ToList().Last().UnitId.ToString());
                        }

                    continue;
                    }


                if(i < 60)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsDirectorateDataEntry)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRoleById(userInfio.UserName,
                            Enums.UserRole.DirectorateDataEntry, Enums.OrgLevel.Directorate);

                        Assert.IsNull(sysRole, "SysRole must be null for non Director data entry");

                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);

                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.DirectorateDataEntry, Enums.OrgLevel.Directorate, service.DirectorateRepository().List().ToList().Last().DirectorateId.ToString());
                        }

                    continue;
                    }

                if(i < 70)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsDirectorateEndorser)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRoleById(userInfio.UserName,
                            Enums.UserRole.DirectorateEndorser, Enums.OrgLevel.Directorate);

                        Assert.IsNull(sysRole, "SysRole must be null for non Director endorser");

                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);

                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.DirectorateEndorser, Enums.OrgLevel.Directorate, service.DirectorateRepository().List().ToList().First().DirectorateId.ToString());
                        }

                    continue;
                    }

                }


            }
        [Ignore]
        [TestMethod]
        public void ImpoertUserRole2()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            factory.RegisterType<IUserIdentityService, DecLsUserIdentityService>();

            var srv = new ServiceRepository(factory);


            var usersWithoutAnyRoles = new List<string>();
            var counter = 0;
            foreach(var user in srv.SysUserRepository().List().ToArray())
                {
                if(
                    !user.SysUserRoles.ToArray()
                        .Any(
                            ur =>
                                ur.RoleId != Enums.UserRole.Guest.ToInteger() &&
                                ur.RoleId != Enums.UserRole.DoEUser.ToInteger()))
                    {
                    usersWithoutAnyRoles.Add(user.UserId);

                    }

                if(++counter > 100) break;
                }
            var skip = 0;
            foreach(var role in srv.SysRoleRepository().List().ToArray().Where(ur =>
                           ur.RoleId != Enums.UserRole.Guest.ToInteger() &&
                           ur.RoleId != Enums.UserRole.DoEUser.ToInteger()).ToArray())
                {
                if(!role.SysUserRoles.Any())
                    {
                    foreach(var user in usersWithoutAnyRoles.Skip(skip))
                        {
                        if(role.GetEnum() == Enums.UserRole.DivisionEditor || role.GetEnum() == Enums.UserRole.DivisionApprover)
                            srv.SysUserRoleRepository().Grant(user, role.GetEnum(), Enums.OrgLevel.Division);
                        else
                        if(role.GetEnum() == Enums.UserRole.DirectorateDataEntry || role.GetEnum() == Enums.UserRole.DirectorateEndorser)
                            srv.SysUserRoleRepository().Grant(user, role.GetEnum(), Enums.OrgLevel.Directorate);

                        else
                        if(role.GetEnum() == Enums.UserRole.BusinessUnitAuthor || role.GetEnum() == Enums.UserRole.BusinessUnitDataEntry)
                            srv.SysUserRoleRepository().Grant(user, role.GetEnum(), Enums.OrgLevel.BusinessUnit);


                        else srv.SysUserRoleRepository().Grant(user, role.GetEnum(), Enums.OrgLevel.NA);
                        }
                    }

                   


                    }
                skip += 10;
                }

        [Ignore]
        [TestMethod]
        public void ImpoertUserRole3()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            factory.RegisterType<IUserIdentityService, DecLsUserIdentityService>();

            var srv = new ServiceRepository(factory);


            var usersWithoutAnyRoles = new List<string>();
            var counter = 0;
            foreach(var user in srv.SysUserRepository().List().OrderByDescending(u=>u.CreatedDate).ToArray())
                {
                if(
                    !user.SysUserRoles.ToArray()
                        .Any(
                            ur =>
                                ur.RoleId != Enums.UserRole.Guest.ToInteger() &&
                                ur.RoleId != Enums.UserRole.DoEUser.ToInteger()))
                    {
                    usersWithoutAnyRoles.Add(user.UserId);

                    }

                if(++counter > 100) break;
                }
            var skip = 0;
            foreach(var role in srv.SysRoleRepository().List().ToArray().Where(ur =>
                           ur.RoleId != Enums.UserRole.Guest.ToInteger() &&
                           ur.RoleId != Enums.UserRole.DoEUser.ToInteger()).ToArray())
                {
                if(!role.SysUserRoles.Any())
                    {
                    foreach(var user in usersWithoutAnyRoles.Skip(skip))
                        {
                        if(role.GetEnum() == Enums.UserRole.HRDataEntry)
                            srv.SysUserRoleRepository().Grant(user, role.GetEnum(), Enums.OrgLevel.NA);                        
                        }
                    }




                }
            skip += 10;
            }

        [TestMethod]
        public void Grant()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var service = new ServiceRepository(factory);

            var userRepo = service.SysUserRepository();

            var sampleSysUser = GetSampleSysUser();
            userRepo.Insert(sampleSysUser);

            userRepo.SysUserRoleRepository.Grant(sampleSysUser.UserId.ToLower(), Enums.UserRole.Administrator, Enums.OrgLevel.Application);

            sampleSysUser = userRepo.GetSysUserByUserName(sampleSysUser.UserId);

            var userExt = UserInfoExtension.MapSysUser(sampleSysUser, factory);

            Assert.IsTrue(userExt.IsAdministrator, "userExt.IsAdministrator");

            }
        [TestMethod]

        public void Deny()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var service = new ServiceRepository(factory);

            var userRepo = service.SysUserRepository();

            var sampleSysUser = GetSampleSysUser();
            userRepo.Insert(sampleSysUser);

            userRepo.SysUserRoleRepository.Grant(sampleSysUser.UserId.ToLower(), Enums.UserRole.Administrator, Enums.OrgLevel.Application);
            userRepo.SysUserRoleRepository.Deny(sampleSysUser.UserId.ToLower(), Enums.UserRole.Administrator, Enums.OrgLevel.Application);

            sampleSysUser = userRepo.GetSysUserByUserName(sampleSysUser.UserId);

            var userExt = UserInfoExtension.MapSysUser(sampleSysUser, factory);

            Assert.IsTrue(!userExt.IsAdministrator, "!userExt.IsAdministrator");

            }

        [TestMethod]
        public void TestDisplayRole()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);

            var userRepo = srv.SysUserRepository();
            var users = userRepo.GetSysUsersByRole(Enums.UserRole.BusinessUnitAuthor, true).Take(10).ToList();
            users.AddRange(userRepo.GetSysUsersByRole(Enums.UserRole.DivisionEditor, true).Take(10) );
            users.AddRange(userRepo.GetSysUsersByRole(Enums.UserRole.DirectorateDataEntry, true).Take(10));
            foreach (var sysUser in users)
            {
                var uInfo = UserInfoExtension.MapSysUser(sysUser, factory);
                Console.WriteLine(uInfo);
                Console.WriteLine(uInfo.DisplayRoles());
            }

            }

        [TestMethod]
        public void TestCurrentRole()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var userRoleRep = srv.SysUserRoleRepository();
            var userRep = srv.SysUserRepository();

            var listOfRoles = Enum.GetValues(typeof(Enums.UserRole)).Cast<Enums.UserRole>();
            foreach (var role in listOfRoles)
            {
                var userIds = userRoleRep.GetSysUserRoleList(role).Take(2).Where(r=>r.IsActive).Select(r=>r.UserId).Distinct().ToArray();
                foreach (var id in userIds)
                {
                    var userInfo=UserInfoExtension.MapSysUser(userRep.GetSysUserByUserName(id),factory);
                    var roles = userInfo.ActiveRoleOrgLevelList.Select(ar => ar.UserRole.ToString()).ToValue();
                     Console.WriteLine(roles);
                    Console.WriteLine(userInfo.CurrentRole);  

                    Assert.IsTrue(roles.Contains(userInfo.CurrentRole),"roles.Contains(userInfo.CurrentRole)");

                }


            }







            }

        [TestMethod]
        public void GetUsersPerRoleTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var service = new ServiceRepository(factory);

            var userRepo = service.SysUserRepository();

            var result = userRepo.GetSysUsersByRole(Enums.UserRole.Administrator, true).Take(10);

            result.ForEach(Console.WriteLine);

            }

        [TestMethod]
        public void GetSysUserRoleListTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var service = new ServiceRepository(factory);

            var userRoleRepo = service.SysUserRoleRepository();

            var result = userRoleRepo.GetSysUserRoleList(Enums.UserRole.SystemAdministrator);

            result.ForEach(Console.WriteLine);

            }

        [TestMethod]
        public void UserInfoPropertiesTest()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var service = new ServiceRepository(factory);
            foreach (var role in service.SysRoleRepository().List().ToArray())
            {
                foreach (var user in service.SysUserRepository().List().Where(u=>u.SysUserRoles.Any(su=>su.RoleId==role.RoleId)).Take(5).ToArray())
                {
                    var uInfo = UserInfoExtension.MapSysUser(user, factory);
                    Console.WriteLine($"{uInfo }-{uInfo.CurrentRoleEnum}");
                }


            }


            }
        private List<string> LoadUserList()
            {
            var list = new List<string>
            {
                "aparna.lishajyothi",
                "che.eyles2",
                "grant.focas",
                "mary.wang8",
                "refky.wahib",
                "sangeetha.madheswaran",
                "robyn.l.burke",
                "michael.boller",
                "donna.lane",
                "peter.smith7",
                "lance.mccabe",
                "paul.abbott",
                "wayne.dunlop",
                "michelle.somers2",
                "monique.christie-johnston",
                "maureen.dickson",
                "peter.banks",
                "steven.mead",
                "andrew.doyle15",
                "david.paget",
                "robert.cormack",
                "liane.sharp",
                "andrew.j.marshall",
                "james.dean5",
                "joanne.williamson",
                "luke.fleming5",
                "laraine.mealing",
                "mathew.egan7",
                "jorg.knoflack",
                "ross.angus",
                "lisa.holmes2",
                "amy.page5",
                "wayne.e.smith",
                "marcus.farah",
                "martin.field5",
                "julie.flavell",
                "dianne.cawston",
                "mark.holden1",
                "matthew.kerr3",
                "brad.harford2",
                "glen.hines",
                "kylie.archer3",
                "joseph.vitale",
                "paul.tindall",
                "brendan.sexty1",
                "julie.bernhardt",
                "kylie.spencer",
                "lockie.lock",
                "craig.holmes",
                "nerida.noble",
                "timothy.laws1",
                "tony.vosnakis",
                "lauryn.curtin1",
                "michaela.clark2",
                "sidney.moncrieff",
                "vicki.c.smith",
                "peter.slater",
                "daniel.cullen",
                "karen.chalk",
                "alexandra.lupton",
                "leah.moon3",
                "monika.dimaio",
                "matthew.hewett",
                "michelle.edgtton1",
                "ian.moore20",
                "peter.c.george",
                "gregory.westwood",
                "craig.marland",
                "timothy.j.porter",
                "matthew.wood8",
                "michael.kay",
                "susan.snedden",
                "karl.liessmann",
                "mitchell.scott13",
                "fay.hammoud",
                "mandy.robertson",
                "karen.goman",
                "christopher.dowd6",
                "michael.seymour",
                "alicia.burns1",
                "dianne.maddern",
                "jeffrey.buxton",
                "carey.binks",
                "steven.richard",
                "kevin.greaves",
                "kristy.bultitude",
                "peter.r.allen",
                "rachean.boyce",
                "drew.janetzki",
                "ruth.c.ellis",
                "alison.lochrin",
                "nathan.r.may",
                "rachael.halpin6",
                "brendan.g.gray",
                "craig.atkins7",
                "michelle.maier",
                "karen.pontt",
                "jason.b.gemmell",
                "jonathan.russell7",
                "paul.c.john",
                "paul.tracey",
                "richard.j.allen",
                "sandy.jolly",
                "matthew.collins24",
                "peter.riley",
                "nathan.honeyman",
                "robert.c.harrison",
                "reece.mastellotto1",
                "william.a.turner",
                "fiona.smith44",
                "susan.brasier",
                "craig.j.maher",
                "chris.kane4",
                "brett.lambkin",
                "emma.wilson9",
                "duncan.adams",
                "david.s.willis",
                "dimity.stewart",
                "jodie.mason4",
                "nicholas.bower",
                "louise.purss-semple",
                "michelle.maher",
                "anthony.stringer",
                "brett.austine",
                "robert.jovanovski",
                "stephen.ross",
                "gregory.gillard",
                "richard.oconnell",
                "charmaine.gillies",
                "terry.willis",
                "timothy.sanson",
                "terrence.bourke",
                "jane.willott",
                "patrick.edmunds",
                "ronnie.gajewski",
                "melanie.yorke2",
                "andrew.johnson11",
                "melinda.hyland",
                "craig.smailes",
                "paul.anderson39",
                "aaron.bolte",
                "brendan.sims4",
                "jean.duma1",
                "melissa.carroll",
                "nathan.brookes2",
                "scott.jacklin",
                "gordon.davy",
                "samantha.doust",
                "graham.smith70",
                "greg.king8",
                "luke.disalvia",
                "stuart.morrison",
                "johanna.shead",
                "ian.groth",
                "peta.wykes",
                "timothy.wykes",
                "stephen.sergeant",
                "craig.a.brooker",
                "peter.oriordan",
                "stacy.hoare",
                "stacey.cox8",
                "lisa.schmetzer",
                "kellie.dawes2",
                "vicki.breust",
                "amanda.munro-jones",
                "lea-anne.vaughan",
                "steven.markham",
                "gina.watt",
                "andrew.curry4",
                "wendy.blaker",
                "sally.staniforth",
                "tristan.jones8",
                "gordon.rae",
                "gillian.mckenzie1",
                "david.willis11",
                "kimberley.kelly15",
                "dale.t.smith",
                "lisa.panton1",
                "timothy.polson",
                "christopher.kirkland1",
                "chase.magner1",
                "nathan.terangi1",
                "jade.bradley8",
                "rozina.broderick",
                "karen.a.cooper",
                "linley.ryan",
                "brooke.handsaker",
                "jeremy.budda-deen",
                "ian.w.ryan",
                "scott.miller58",
                "jenny.steepe",
                "jennifer.alcorn",
                "nicole.wilson67",
                "jonathan.willis",
                "peter.mead7",
                "edward.mcguirk",
                "leonie.macgregor",
                "darrel.mole",
                "andrew.paul.mcdonald",
                "justine.davies",
                "belinda.meppem",
                "mitchell.hardy6",
                "peter.brown187",
                "kevin.squires",
                "andrew.watson",
                "wayne.hardcastle",
                "judith.thomas",
                "felicity.bagshaw",
                "leigh.bogan",
                "james.waters11",
                "kerry.floyd",
                "wolly.negroh",
                "jacqueline.charlton",
                "maxwell.foord",
                "jacqueline.patrick",
                "cassandra.richards5",
                "leif.smith",
                "david.m.arthur",
                "ian.hinton1",
                "grant.clowes",
                "courtney.dean2",
                "melissa.ellis13",
                "damian.toohey",
                "grant.newell",
                "debra.m.young",
                "ron.pratt",
                "mark.skein",
                "margot.brissenden",
                "bryony.gerofi",
                "david.n.gale",
                "ross.dummett",
                "rcarter7",
                "alan.lane",
                "andrew.mitton",
                "kelly.lawson3",
                "paul.roger",
                "alexander.mclellan2",
                "sandra.holden",
                "barry.mccoll",
                "jess.absolum",
                "thomas.mccarney4",
                "eliza.baddock",
                "ross.hallaways",
                "robert.hudson"
            };






            return list;

            }

        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();
            }


        }

    }

