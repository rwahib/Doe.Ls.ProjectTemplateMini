using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
{
    [TestClass]
    public class DatabaseTests : TestBase
    {
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }

        [TestMethod]
        public void TestMars()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gSrv = new ServiceRepository(factory);
            var rpdRep = gSrv.RolePositionDescriptionRepository();
            var rdRep = gSrv.RoleDescriptionRepository();
            var roleCapRep = gSrv.RoleCapabilityRepository();

            if (!rpdRep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var rdp in rpdRep.List().Take(10))
            {
                Console.WriteLine($"{rdp.Title}");
            var rdList =rdRep.ListForCapabilityFramework().Where(rd => rd.RoleDescriptionId == rdp.RolePositionDescId);
                foreach (var rd in rdList)
                {
                    Console.WriteLine($"\t\t{rd.KeyChallenges}");
                    var roleCapList = roleCapRep.List().Where(rc => rc.RoleDescriptionId == rdp.RolePositionDescId);
                    foreach (var roleCapability in roleCapList)
                    {
                        Console.WriteLine($"\t\t\t{roleCapability.CapabilityName}");
                    }

                }

            }
        }

        //[TestMethod]
        //public void TestMars()
        //{
        //    var factory = new MockRepositoryFactory();
        //    factory.RegisterAllDependencies();

        //    var gSrv = new ServiceRepository(factory);
        //    var rep = gSrv.GeneralLogRepository();
        //    if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

        //    foreach (var model in rep.List().Take(10).ToArray())
        //    {
        //        var log = rep.List().SingleOrDefault(ent => ent.LogId == model.LogId);
        //        var oldAction = log.Action;
        //        var newAction= oldAction+"_new";
        //        log.Action = newAction;
        //        try
        //        {
        //            rep.Update(log);
        //            Console.WriteLine("This dataabse support MARS Multiple Active Result Sets");
        //            Console.WriteLine("https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/enabling-multiple-active-result-sets ");
        //        }
        //        catch (Exception)
        //        {
        //            Console.WriteLine("This dataabse does not support MARS Multiple Active Result Sets");
        //            Console.WriteLine("https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/enabling-multiple-active-result-sets ");
        //            throw;
        //        }

        //        rep.Refresh(log);

        //        Assert.IsTrue(log.Action==newAction,"log.Action==newAction");

        //        log.Action = oldAction;
                
        //        rep.Update(log);
        //        rep.Refresh(log);
        //        Assert.IsTrue(log.Action == oldAction, "log.Action==oldAction");

        //    }
        //}

        [TestMethod]
        public void TestNestedTransaction()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gSrv = new ServiceRepository(factory);
            var rep = gSrv.GeneralLogRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var model in rep.List().Take(10))
            {
                
                
                try
                {

                    var f2 = new MockRepositoryFactory();
                    f2.RegisterAllDependencies();

                    var gSrv2 = new ServiceRepository(f2);
                    var rep2 = gSrv2.GeneralLogRepository();
                    var log = rep2.List().SingleOrDefault(ent => ent.LogId == model.LogId);
                    var oldAction = log.Action;
                    var newAction = oldAction + "_new";
                    log.Action = newAction;
                    rep2.Update(log);
                    rep2.Refresh(log);

                    Assert.IsTrue(log.Action == newAction, "log.Action==newAction");

                    log.Action = oldAction;

                    rep2.Update(log);
                    rep2.Refresh(log);
                    Assert.IsTrue(log.Action == oldAction, "log.Action==oldAction");

                    Console.WriteLine("This dataabse support nested trans");
                    
                }
                catch (Exception exception)
                {
                    Console.WriteLine("This dataabse does not support nested trans");
                    Console.WriteLine(exception.Message);
                    throw;
                }

               

            }
        }
        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

    }
}
