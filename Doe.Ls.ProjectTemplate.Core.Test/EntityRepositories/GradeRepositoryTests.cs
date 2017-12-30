

using System;
using System.Linq;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class GradeRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.GradeRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");

            foreach (var model in rep.List().Take(10)) {
                Console.WriteLine("{0}", model.ToString());
            }
        }
        [TestMethod]
        public void Create()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.GradeRepository();
           
            var grade = GetGrade(Enums.GradeType.NSBTS);
            rep.Insert(grade);
            var insertedrec = rep.List().Where(l => l.GradeTitle.Contains(UnitTestToken));
            Assert.IsFalse(insertedrec==null,"Grade insert failed");
        }

   

    [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.GradeRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.GradeRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.GradeRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.GradeRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }


    [TestMethod]
    public void TestGetPsse1()
    {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.GradeRepository();

        var gradePsse = rep.List().FirstOrDefault(g => g.GradeCode == "PSSE1");

        var gradePsse1 = CommonHelper.GetPSSE1Code();
          Assert.AreEqual(gradePsse.GradeCode, gradePsse1);
    }

       
        [TestMethod]
        public void TestGetPsse2()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.GradeRepository();

            var gradePsse = rep.List().FirstOrDefault(g => g.GradeCode == "PSSE2");

            var gradePsse2 = CommonHelper.GetPSSE2Code();
            Assert.AreEqual(gradePsse.GradeCode.Replace(" ", ""), gradePsse2);
        }

        [TestMethod]
        public void TestGetPsse3()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.GradeRepository();

            var gradePsse = rep.List().FirstOrDefault(g => g.GradeCode == "PSSE3");

            var gradePsse3 = CommonHelper.GetPSSE3Code();
            Assert.AreEqual(gradePsse.GradeCode.Replace(" ", ""), gradePsse3);
        }


        [TestMethod]
        public void TestGetPsseType()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.GradeRepository();
            
            var gradePsseType = CommonHelper.GetNameOfPSSEType();
            Assert.AreEqual("PSSE", gradePsseType);
        }

        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

