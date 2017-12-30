

using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.PosEstablishment.Data;
using Doe.Ls.PosEstablishment.Core.BL;
using Doe.Ls.PosEstablishment.Core.BL.DomainServices;
using Doe.Ls.PosEstablishment.Core.BL.Models;
using Doe.Ls.PosEstablishment.Core.BL.Models.Extensions;
using Doe.Ls.PosEstablishment.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.PosEstablishment.Core.Test.EntityRepositories
    {
    [TestClass]
    public class SysUserRoleRepositoryTests : TestBase
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
                        var sysRole = sysUserRoleRep.GetSysUserRole(userInfio.UserName,
                            Enums.UserRole.SystemAdministrator, Enums.OrgLevel.System);

                        Assert.IsNull(sysRole, "SysRole must be null for non SysAdmin");
                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);
                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.SystemAdministrator,
                                Enums.OrgLevel.System);
                        }
                    continue;
                    }
                if(i < 20)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsAdminstrator)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRole(userInfio.UserName,
                            Enums.UserRole.Adminstrator, Enums.OrgLevel.System);

                        Assert.IsNull(sysRole, "SysRole must be null for non Admin");
                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);
                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.Adminstrator, Enums.OrgLevel.System);
                        }

                    continue;
                    }


                if(i < 30)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsPowerUser)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRole(userInfio.UserName,
                            Enums.UserRole.PowerUser, Enums.OrgLevel.System);

                        Assert.IsNull(sysRole, "SysRole must be null for non Power user");
                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);
                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.PowerUser, Enums.OrgLevel.System);
                        }

                    continue;
                    }

                if(i < 40)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsBusinessUnitDataEntry)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRole(userInfio.UserName,
                            Enums.UserRole.BusinessUnitDataEntry, Enums.OrgLevel.Unit);

                        Assert.IsNull(sysRole, "SysRole must be null for non Business unit dataEntry");

                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);

                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.BusinessUnitDataEntry, Enums.OrgLevel.Unit, service.UnitRepository().List().ToList().First().UnitId);
                        }

                    continue;
                    }
                if(i < 50)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsBusinessUnitAuthor)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRole(userInfio.UserName,
                            Enums.UserRole.BusinessUnitAuthor, Enums.OrgLevel.Unit);

                        Assert.IsNull(sysRole, "SysRole must be null for non Business unit author");

                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);

                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.BusinessUnitAuthor, Enums.OrgLevel.Unit, service.UnitRepository().List().ToList().Last().UnitId);
                        }

                    continue;
                    }


                if(i < 60)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsDirectorateDataEntry)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRole(userInfio.UserName,
                            Enums.UserRole.DirectorateDataEntry, Enums.OrgLevel.Directorate);

                        Assert.IsNull(sysRole, "SysRole must be null for non Director data entry");

                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);

                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.DirectorateDataEntry, Enums.OrgLevel.Directorate, service.DirectorateRepository().List().ToList().Last().DirectorateId);
                        }

                    continue;
                    }

                if(i < 70)
                    {
                    if(userInfio.IsRoleInitialised && !userInfio.IsDirectorateEndorser)
                        {
                        var sysRole = sysUserRoleRep.GetSysUserRole(userInfio.UserName,
                            Enums.UserRole.DirectorateEndorser, Enums.OrgLevel.Directorate);

                        Assert.IsNull(sysRole, "SysRole must be null for non Director endorser");

                        loginRep.SessionService.AddToSession(Cnt.CurrentUserKey, UserInfoExtension.SystemUser);

                        sysUserRoleRep.Grant(userInfio.UserName, Enums.UserRole.DirectorateEndorser, Enums.OrgLevel.Directorate, service.DirectorateRepository().List().ToList().First().DirectorateId);
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
                            srv.SysUserRoleRepository().Grant(user, role.GetEnum(), Enums.OrgLevel.Unit);


                        else srv.SysUserRoleRepository().Grant(user, role.GetEnum(), Enums.OrgLevel.NA);
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

            userRepo.SysUserRoleRepository.Grant(sampleSysUser.UserId.ToLower(), Enums.UserRole.Adminstrator, Enums.OrgLevel.Application);

            sampleSysUser = userRepo.GetSysUserByUserName(sampleSysUser.UserId);

            var userExt = UserInfoExtension.MapSysUser(sampleSysUser);

            Assert.IsTrue(userExt.IsAdminstrator, "userExt.IsAdminstrator");

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

            userRepo.SysUserRoleRepository.Grant(sampleSysUser.UserId.ToLower(), Enums.UserRole.Adminstrator, Enums.OrgLevel.Application);
            userRepo.SysUserRoleRepository.Deny(sampleSysUser.UserId.ToLower(), Enums.UserRole.Adminstrator, Enums.OrgLevel.Application);

            sampleSysUser = userRepo.GetSysUserByUserName(sampleSysUser.UserId);

            var userExt = UserInfoExtension.MapSysUser(sampleSysUser);

            Assert.IsTrue(!userExt.IsAdminstrator, "!userExt.IsAdminstrator");

            }

        [TestMethod]
        public void GetUsersPerRole()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var service = new ServiceRepository(factory);

            var userRepo = service.SysUserRepository();

            var result = userRepo.GetSysUsersByRole(Enums.UserRole.Adminstrator, true).Take(10);

            result.ForEach(Console.WriteLine);

            }


        private List<string> LoadUserList()
            {
            var list = new List<string>();


            list.Add("aparna.lishajyothi");
            list.Add("che.eyles2");
            list.Add("grant.focas");
            list.Add("mary.wang8");
            list.Add("refky.wahib");
            list.Add("sangeetha.madheswaran");


            list.Add("robyn.l.burke");
            list.Add("michael.boller");
            list.Add("donna.lane");
            list.Add("peter.smith7");
            list.Add("lance.mccabe");
            list.Add("paul.abbott");
            list.Add("wayne.dunlop");
            list.Add("michelle.somers2");
            list.Add("monique.christie-johnston");
            list.Add("maureen.dickson");
            list.Add("peter.banks");
            list.Add("steven.mead");
            list.Add("andrew.doyle15");
            list.Add("david.paget");
            list.Add("robert.cormack");
            list.Add("liane.sharp");
            list.Add("andrew.j.marshall");
            list.Add("james.dean5");
            list.Add("joanne.williamson");
            list.Add("luke.fleming5");
            list.Add("laraine.mealing");
            list.Add("mathew.egan7");
            list.Add("jorg.knoflack");
            list.Add("ross.angus");
            list.Add("lisa.holmes2");
            list.Add("amy.page5");
            list.Add("wayne.e.smith");
            list.Add("marcus.farah");
            list.Add("martin.field5");
            list.Add("julie.flavell");
            list.Add("dianne.cawston");
            list.Add("mark.holden1");
            list.Add("matthew.kerr3");
            list.Add("brad.harford2");
            list.Add("glen.hines");
            list.Add("kylie.archer3");
            list.Add("joseph.vitale");
            list.Add("paul.tindall");
            list.Add("brendan.sexty1");
            list.Add("julie.bernhardt");
            list.Add("kylie.spencer");
            list.Add("lockie.lock");
            list.Add("craig.holmes");
            list.Add("nerida.noble");
            list.Add("timothy.laws1");
            list.Add("tony.vosnakis");
            list.Add("lauryn.curtin1");
            list.Add("michaela.clark2");
            list.Add("sidney.moncrieff");
            list.Add("vicki.c.smith");
            list.Add("peter.slater");
            list.Add("daniel.cullen");
            list.Add("karen.chalk");
            list.Add("alexandra.lupton");
            list.Add("leah.moon3");
            list.Add("monika.dimaio");
            list.Add("matthew.hewett");
            list.Add("michelle.edgtton1");
            list.Add("ian.moore20");
            list.Add("peter.c.george");
            list.Add("gregory.westwood");
            list.Add("craig.marland");
            list.Add("timothy.j.porter");
            list.Add("matthew.wood8");
            list.Add("michael.kay");
            list.Add("susan.snedden");
            list.Add("karl.liessmann");
            list.Add("mitchell.scott13");
            list.Add("fay.hammoud");
            list.Add("mandy.robertson");
            list.Add("karen.goman");
            list.Add("christopher.dowd6");
            list.Add("michael.seymour");
            list.Add("alicia.burns1");
            list.Add("dianne.maddern");
            list.Add("jeffrey.buxton");
            list.Add("carey.binks");
            list.Add("steven.richard");
            list.Add("kevin.greaves");
            list.Add("kristy.bultitude");
            list.Add("peter.r.allen");
            list.Add("rachean.boyce");
            list.Add("drew.janetzki");
            list.Add("ruth.c.ellis");
            list.Add("alison.lochrin");
            list.Add("nathan.r.may");
            list.Add("rachael.halpin6");
            list.Add("brendan.g.gray");
            list.Add("craig.atkins7");
            list.Add("michelle.maier");
            list.Add("karen.pontt");
            list.Add("jason.b.gemmell");
            list.Add("jonathan.russell7");
            list.Add("paul.c.john");
            list.Add("paul.tracey");
            list.Add("richard.j.allen");
            list.Add("sandy.jolly");
            list.Add("matthew.collins24");
            list.Add("peter.riley");
            list.Add("nathan.honeyman");
            list.Add("robert.c.harrison");
            list.Add("reece.mastellotto1");
            list.Add("william.a.turner");
            list.Add("fiona.smith44");
            list.Add("susan.brasier");
            list.Add("craig.j.maher");
            list.Add("chris.kane4");
            list.Add("brett.lambkin");
            list.Add("emma.wilson9");
            list.Add("duncan.adams");
            list.Add("david.s.willis");
            list.Add("dimity.stewart");
            list.Add("jodie.mason4");
            list.Add("nicholas.bower");
            list.Add("louise.purss-semple");
            list.Add("michelle.maher");
            list.Add("anthony.stringer");
            list.Add("brett.austine");
            list.Add("robert.jovanovski");
            list.Add("stephen.ross");
            list.Add("gregory.gillard");
            list.Add("richard.oconnell");
            list.Add("charmaine.gillies");
            list.Add("terry.willis");
            list.Add("timothy.sanson");
            list.Add("terrence.bourke");
            list.Add("jane.willott");
            list.Add("patrick.edmunds");
            list.Add("ronnie.gajewski");
            list.Add("melanie.yorke2");
            list.Add("andrew.johnson11");
            list.Add("melinda.hyland");
            list.Add("craig.smailes");
            list.Add("paul.anderson39");
            list.Add("aaron.bolte");
            list.Add("brendan.sims4");
            list.Add("jean.duma1");
            list.Add("melissa.carroll");
            list.Add("nathan.brookes2");
            list.Add("scott.jacklin");
            list.Add("gordon.davy");
            list.Add("samantha.doust");
            list.Add("graham.smith70");
            list.Add("greg.king8");
            list.Add("luke.disalvia");
            list.Add("stuart.morrison");
            list.Add("johanna.shead");
            list.Add("ian.groth");
            list.Add("peta.wykes");
            list.Add("timothy.wykes");
            list.Add("stephen.sergeant");
            list.Add("craig.a.brooker");
            list.Add("peter.oriordan");
            list.Add("stacy.hoare");
            list.Add("stacey.cox8");
            list.Add("lisa.schmetzer");
            list.Add("kellie.dawes2");
            list.Add("vicki.breust");
            list.Add("amanda.munro-jones");
            list.Add("lea-anne.vaughan");
            list.Add("steven.markham");
            list.Add("gina.watt");
            list.Add("andrew.curry4");
            list.Add("wendy.blaker");
            list.Add("sally.staniforth");
            list.Add("tristan.jones8");
            list.Add("gordon.rae");
            list.Add("gillian.mckenzie1");
            list.Add("david.willis11");
            list.Add("kimberley.kelly15");
            list.Add("dale.t.smith");
            list.Add("lisa.panton1");
            list.Add("timothy.polson");
            list.Add("christopher.kirkland1");
            list.Add("chase.magner1");
            list.Add("nathan.terangi1");
            list.Add("jade.bradley8");
            list.Add("rozina.broderick");
            list.Add("karen.a.cooper");
            list.Add("linley.ryan");
            list.Add("brooke.handsaker");
            list.Add("jeremy.budda-deen");
            list.Add("ian.w.ryan");
            list.Add("scott.miller58");
            list.Add("jenny.steepe");
            list.Add("jennifer.alcorn");
            list.Add("nicole.wilson67");
            list.Add("jonathan.willis");
            list.Add("peter.mead7");
            list.Add("edward.mcguirk");
            list.Add("leonie.macgregor");
            list.Add("darrel.mole");
            list.Add("andrew.paul.mcdonald");
            list.Add("justine.davies");
            list.Add("belinda.meppem");
            list.Add("mitchell.hardy6");
            list.Add("peter.brown187");
            list.Add("kevin.squires");
            list.Add("andrew.watson");
            list.Add("wayne.hardcastle");
            list.Add("judith.thomas");
            list.Add("felicity.bagshaw");
            list.Add("leigh.bogan");
            list.Add("james.waters11");
            list.Add("kerry.floyd");
            list.Add("wolly.negroh");
            list.Add("jacqueline.charlton");
            list.Add("maxwell.foord");
            list.Add("jacqueline.patrick");
            list.Add("cassandra.richards5");
            list.Add("leif.smith");
            list.Add("david.m.arthur");
            list.Add("ian.hinton1");
            list.Add("grant.clowes");
            list.Add("courtney.dean2");
            list.Add("melissa.ellis13");
            list.Add("damian.toohey");
            list.Add("grant.newell");
            list.Add("debra.m.young");
            list.Add("ron.pratt");
            list.Add("mark.skein");
            list.Add("margot.brissenden");
            list.Add("bryony.gerofi");
            list.Add("david.n.gale");
            list.Add("ross.dummett");
            list.Add("rcarter7");
            list.Add("alan.lane");
            list.Add("andrew.mitton");
            list.Add("kelly.lawson3");
            list.Add("paul.roger");
            list.Add("alexander.mclellan2");
            list.Add("sandra.holden");
            list.Add("barry.mccoll");
            list.Add("jess.absolum");
            list.Add("thomas.mccarney4");
            list.Add("eliza.baddock");
            list.Add("ross.hallaways");
            list.Add("robert.hudson");


            return list;

            }

        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();
            }


        }

    }

