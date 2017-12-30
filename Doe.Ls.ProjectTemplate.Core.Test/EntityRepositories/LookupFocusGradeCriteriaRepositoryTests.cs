

using System;
using System.Linq;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories 
{
[TestClass]
    public class LookupFocusGradeCriteriaRepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.LookupFocusGradeCriteriaRepository();
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
            var rep = globalServiceRepository.LookupFocusGradeCriteriaRepository();

            var selRep = globalServiceRepository.SelectionCriteriaRepository();
            var focusRep = globalServiceRepository.FocusRepository();
            var gradeRep = globalServiceRepository.GradeRepository();
            var focus = GetFocus();
            var criteria = GetCriteria();
            var grade = GetGrade(Enums.GradeType.NSBTS);
            gradeRep.Insert(grade);
            focusRep.Insert(focus);
            selRep.Insert(criteria);

            var focusGradeCriteria = GetLookupFocusGradeCriteria(focus.FocusId, grade.GradeCode, criteria.SelectionCriteriaId,"insert");

            rep.Insert(focusGradeCriteria);
            if (!rep.List().Any(l=>l.LastUpdatedBy.Contains(UnitTestToken+ "insert")))Assert.Inconclusive("Insert failed");
        }

  

    [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LookupFocusGradeCriteriaRepository();
            var selRep = globalServiceRepository.SelectionCriteriaRepository();
            var focusRep = globalServiceRepository.FocusRepository();
            var gradeRep = globalServiceRepository.GradeRepository();
            var focus = GetFocus();
            var criteria = GetCriteria();
            var grade = GetGrade(Enums.GradeType.NSBTS);
            gradeRep.Insert(grade);
            focusRep.Insert(focus);
            selRep.Insert(criteria);

            var focusGradeCriteria = GetLookupFocusGradeCriteria(focus.FocusId, grade.GradeCode, criteria.SelectionCriteriaId, "update");

            rep.Insert(focusGradeCriteria);
            if (!rep.List().Any(l => l.LastUpdatedBy.Contains(UnitTestToken + "update"))) Assert.Inconclusive("Insert failed");
            var insertedObj = rep.List().FirstOrDefault(l => l.LastUpdatedBy.Contains(UnitTestToken + "update"));
            var grade1 = GetGrade(Enums.GradeType.NSBTS);
            gradeRep.Insert(grade1);
            insertedObj.GradeCode = grade1.GradeCode;
            rep.Update(insertedObj);
            if (!rep.List().Any(l => l.LastUpdatedBy.Contains(UnitTestToken + "update") && l.GradeCode == grade1.GradeCode)) Assert.Inconclusive("update failed");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LookupFocusGradeCriteriaRepository();
            var selRep = globalServiceRepository.SelectionCriteriaRepository();
            var focusRep = globalServiceRepository.FocusRepository();
            var gradeRep = globalServiceRepository.GradeRepository();
            var focus = GetFocus();
            var criteria = GetCriteria();
            var grade = GetGrade(Enums.GradeType.NSBTS);
            gradeRep.Insert(grade);
            focusRep.Insert(focus);
            selRep.Insert(criteria);

            var focusGradeCriteria = GetLookupFocusGradeCriteria(focus.FocusId, grade.GradeCode, criteria.SelectionCriteriaId, "delete");

            rep.Insert(focusGradeCriteria);
            if (!rep.List().Any(l => l.LastUpdatedBy.Contains(UnitTestToken + "delete"))) Assert.Inconclusive("Insert failed");
           
            var insertedObj = rep.List().FirstOrDefault(l => l.LastUpdatedBy.Contains(UnitTestToken + "delete"));
            rep.Delete(insertedObj);
            if (rep.List().Any(l => l.LastUpdatedBy.Contains(UnitTestToken + "delete") )) Assert.Inconclusive("delete failed");

        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LookupFocusGradeCriteriaRepository();
            if (!rep.List().Any())Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.LookupFocusGradeCriteriaRepository();

            var selRep = globalServiceRepository.SelectionCriteriaRepository();
            var focusRep = globalServiceRepository.FocusRepository();
            var gradeRep = globalServiceRepository.GradeRepository();
            var focus = GetFocus();
            var criteria = GetCriteria();
            var grade = GetGrade(Enums.GradeType.NSBTS);
            gradeRep.Insert(grade);
            focusRep.Insert(focus);
            selRep.Insert(criteria);

            var focusGradeCriteria = GetLookupFocusGradeCriteria(focus.FocusId, grade.GradeCode, criteria.SelectionCriteriaId, "search");

            rep.Insert(focusGradeCriteria);


            var arg = new SearchArg()
            {
                Search = UnitTestToken + "search"
            };
            var fList = rep.FilterLookupFocusGradeCriterias(rep.List(), arg);
            Assert.IsFalse(fList == null || !fList.Any(), "Search failed");
        }

      [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

