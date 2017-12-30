

using System;
using System.Collections.Generic;
using System.Linq;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.UI;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories
{
    [TestClass]
    public class PositionDescriptionRepositoryTests : TestBase
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
            var rep = globalServiceRepository.PositionDescriptionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var model in rep.List().Take(10))
            {
                Console.WriteLine("{0}", model.ToString());
            }
        }
        [TestMethod]
        // [Ignore]
        public void Create()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionDescriptionRepository();
            var roleposrep = globalServiceRepository.RolePositionDescriptionRepository();

            var positioDesc = TestCreatePositionDescription(globalServiceRepository,100);
            var posdesc = rep.List().Where(l => l.PositionDescriptionId == positioDesc.PositionDescriptionId);
            Assert.IsFalse(posdesc == null, "Insert position description failed");
            var rolePosdesc = roleposrep.List().Where(l => l.RolePositionDescId == positioDesc.PositionDescriptionId);
            Assert.IsFalse(rolePosdesc == null, "Insert role position description failed");

        }


        [TestMethod]
        public void Edit()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionDescriptionRepository();
            var roleposrep = globalServiceRepository.RolePositionDescriptionRepository();

            var positionDescription = TestCreatePositionDescription(globalServiceRepository,166);
            var posdesc = rep.List().FirstOrDefault(l => l.PositionDescriptionId == positionDescription.PositionDescriptionId);
            Assert.IsFalse(posdesc == null, "Insert position description failed");
            var rolePosdesc = roleposrep.List().FirstOrDefault(l => l.RolePositionDescId == positionDescription.PositionDescriptionId);
            Assert.IsFalse(rolePosdesc == null, "Insert role position description failed");

            var grade1 = GetGrade(Enums.GradeType.NSBTS);
            globalServiceRepository.GradeRepository().Insert(grade1);
            
            posdesc.BriefRoleStatement = UnitTestToken + "Test role statement update";
            posdesc.RolePositionDescription.GradeCode = grade1.GradeCode;
            posdesc.RolePositionDescription.Title = UnitTestToken + "Test update";
            rep.UpdatePositionDescription(rolePosdesc);

            var posdesc1 = rep.List().FirstOrDefault(l => l.PositionDescriptionId == positionDescription.PositionDescriptionId);
            Assert.IsFalse(posdesc1 == null || !posdesc1.BriefRoleStatement.Contains("update"), "Update position description failed");
            var rolePosdesc1 = roleposrep.List().FirstOrDefault(l => l.RolePositionDescId == positionDescription.PositionDescriptionId);
            Assert.IsFalse(rolePosdesc == null || !rolePosdesc1.Title.Contains("update"), "Update role position description failed");
        }

     
        [TestMethod]
        public void Delete()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionDescriptionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionDescriptionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionDescriptionRepository();
            var positionDescription = TestCreatePositionDescription(globalServiceRepository,220);

            var arg = new SearchArg()
            {
                Search = positionDescription.BriefRoleStatement
            };
            var result = rep.FilterPositionDescriptions(rep.List(), arg);
            Assert.IsFalse(!rep.List().Any(), "Search Failed");
            Assert.IsFalse(!result.Any(l => l.BriefRoleStatement.Contains(UnitTestToken)), "Search Failed");
        }

       

        [TestMethod]
        public void AddSelectionCriteriaToPositiondescription()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            
            var posdesc = TestCreatePositionDescription(globalServiceRepository,10);
            Assert.IsFalse(posdesc == null, "Insert position description failed");
            var rolePosdesc = globalServiceRepository.RolePositionDescriptionRepository().List().Where(l => l.RolePositionDescId == posdesc.PositionDescriptionId);
            Assert.IsFalse(rolePosdesc == null, "Insert role position description failed");
            var selectionCriteriaItems =
                globalServiceRepository.LookupFocusGradeCriteriaRepository()
                    .List()
                    .Where(l => l.GradeCode == posdesc.RolePositionDescription.GradeCode)
                    .ToArray();
            var selFocusList = new List<PositionFocusCriteria>();
            foreach (var criteria in selectionCriteriaItems)
            {
                
                if ( criteria.IsMandatory)
                {
                    var positionFocusCriteria = new PositionFocusCriteria
                    {
                        LookupId = criteria.LookupId,
                        PositionDescriptionId = posdesc.PositionDescriptionId,
                        LastModifiedDate = DateTime.Now,
                        LastModifiedBy =UnitTestToken+"test"
                    };
                    selFocusList.Add(positionFocusCriteria);
                }

            }
            globalServiceRepository.PositionFocusCriteriaRepository().UpdatePositionFocusCriteria(posdesc.PositionDescriptionId, selFocusList);

            var posFocusList = globalServiceRepository.PositionFocusCriteriaRepository()
                .List()
                .Where(p => p.PositionDescriptionId == posdesc.PositionDescriptionId);
            var mandatoryitems=selectionCriteriaItems.Where(s => s.IsMandatory);
            Assert.IsFalse(posFocusList.Count()!=mandatoryitems.Count(),"Mandatory items insert failed");

        }

        //[Ignore]
        [TestMethod]
        public void TestClone()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var gsr = new ServiceRepository(factory);

            var sourcePd = gsr.PositionDescriptionRepository().LiveList()
                .OrderByDescending(p => p.PositionDescriptionId).FirstOrDefault();

            Random r = new Random();
            int rInt = r.Next(100000, 999999);

            var newDocNum = "DOC17/" + rInt;
            var docNumExists = gsr.RolePositionDescriptionRepository().Exists(newDocNum);

            if (!docNumExists)
            {
                var rolePosDesc = new RolePositionDescription
                {
                    DocNumber = newDocNum,
                    Title = sourcePd.RolePositionDescription.Title,
                    GradeCode = sourcePd.RolePositionDescription.GradeCode
                };
                var newPd = new PositionDescription();
                gsr.PositionDescriptionRepository().ClonePositionDescrition(newPd, 
                    rolePosDesc, sourcePd.RolePositionDescription.DocNumber, sourcePd.PositionDescriptionId, "UnitTest");
                
             }
            else
            {
                Console.WriteLine("The Doc number already exists. No cloning has been run.");
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }


    }

}

