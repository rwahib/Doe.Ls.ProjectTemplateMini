using System;
using Doe.Ls.EntityBase.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain
    {
    [TestClass]
    public class DataTypesAndExtensionsTests
        {

        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }


        [TestMethod]
        public void CleanPropertyTest()
            {
            var val = "DoEUser";
            var result = val.CleanVariable();
            Console.WriteLine(result);
            Assert.IsTrue(val == result);
            val = "Hello world";
             result = val.CleanVariable();
            Console.WriteLine(result);
            Assert.IsTrue("Helloworld" == result);
            }

        [TestMethod]
        public void DateFormat()
            {
            var day = DateTime.Now;
            var result = day.ToLongDateString();

            Console.WriteLine(result);
            }

        /// <summary>
        /// need to be move to Entity base test
        /// </summary>
        [TestMethod]
        public void GetAppUrlTest()
            {
            var result = HttpHelper.GetActionUrl("A", "B", new { aa = "aa", bb = "bb", cc = "cc" });

            Console.WriteLine(result);

            }

        [TestMethod]
        public void Wordify()
            {
            var msg = "DoE";

            Assert.IsTrue(msg.Wordify() == msg, "msg.Wordify()==msg");
            Assert.IsTrue(msg.WordifyOld() == msg, "msg.WordifyOld()==msg");

            msg = "DoE roles";
            Assert.IsTrue(msg.Wordify() == msg, "msg.Wordify()==msg");
            Assert.IsTrue(msg.WordifyOld() == msg, "msg.WordifyOld()==msg");

            msg = " DoE roles";
            Assert.IsTrue(msg.Wordify() == msg.Trim(), "msg.Wordify()==msg");
            Assert.IsTrue(msg.WordifyOld() == msg.Trim(), "msg.WordifyOld()==msg");

            msg = "DoE ";
            Assert.IsTrue(msg.Wordify() == msg.Trim(), "msg.Wordify()==msg");
            Assert.IsTrue(msg.WordifyOld() == msg.Trim(), "msg.WordifyOld()==msg");

            }
        [TestMethod]
        public void Wordify2()
            {
            var wordsNotWordified = new[]
            {"DoE ", "DoE", "DoE users ","DoE Users ", "HRDataEntry ", "HelloWorldFromSydney",
                "helloWorldFromSydney","PositionDescriptionId","BriefRoleStatement" ,
                "RolePositionDescription","PositionFocusCriterias","PositionDescriptionMetadata"

                };

            foreach(var s in wordsNotWordified)
                {
                Console.WriteLine($"{s} ==> {s.Wordify()}");
                if(s.Equals("DoE")) Assert.IsTrue(s.Wordify() == "DoE", "s.Wordify()=='DoE'");
                if(s.Equals("DoE users ")) Assert.IsTrue(s.Wordify() == "DoE users", "s.Wordify()=='DoE'");
                if(s.Equals("DoE Users "))
                    Assert.IsTrue(s.Wordify() == "DoE users", "s.Wordify()=='DoE'");
                if(s.Equals("PositionDescriptionMetadata")) Assert.IsTrue(s.Wordify() == "Position description metadata", "s.Wordify()=='DoE'");
                if(s.Equals("HRDataEntry")) Assert.IsTrue(s.Wordify() == "HR data entry", "s.Wordify()=='DoE'");
                }



            }
        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();
            }

        }
    }
