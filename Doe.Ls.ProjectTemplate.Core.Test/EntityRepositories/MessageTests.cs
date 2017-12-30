

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    public class MessageTests : TestBase
    {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }
        
        [TestMethod]
        public void TestNotFoundMsg()
        {

            var factory = base.GetServiceRepository().RepositoryFactory;

            var positionId = 123;
            var msg = MessageHelper.NotFoundMessage($"position description ({positionId})", factory);
            Console.WriteLine(msg);
            var msg1 = MessageHelper.NotFoundMessage("role description", factory);
            Console.WriteLine(msg1);

            var msg2 = MessageHelper.NotFoundMessage(null, factory);
            Console.WriteLine(msg2);


            var msg3 = MessageHelper.NotFoundMessage("team", factory);
            Console.WriteLine(msg3);


        }

        [TestMethod]
        public void TestNull_PleaseEnterMsg()
        {
            var factory = base.GetServiceRepository().RepositoryFactory;

            var msg = MessageHelper.NullPleaseEnterMessage("Position title", factory);
            Console.WriteLine(msg);
            var msg1 = MessageHelper.NullPleaseEnterMessage("valid position number", factory);
            Console.WriteLine(msg1);
            
              var msg2 = MessageHelper.NullPleaseEnterMessage(null, factory);
            Console.WriteLine(msg2);
            var msg3 = MessageHelper.NullPleaseEnterMessage("", factory);
            Console.WriteLine(msg3);
        }

        [TestMethod]
        public void TestNull_PleaseSelectMsg()
        {
            var factory = base.GetServiceRepository().RepositoryFactory;

            var msg = MessageHelper.NullPleaseSelectMessage("Grade", factory);
            Console.WriteLine(msg);

            var msg2 = MessageHelper.NullPleaseSelectMessage(null, factory);
            Console.WriteLine(msg2);
            var msg3 = MessageHelper.NullPleaseSelectMessage("", factory);
            Console.WriteLine(msg3);
        }

       

        [TestMethod]
        public void RdPdBeforePositionMessageTest()
        {
            var factory = base.GetServiceRepository().RepositoryFactory;

            var msg = MessageHelper.RdPdMustExistsBeforePositionMessage(factory);
            Console.WriteLine(msg);

        }

        [TestMethod]
        public void TestDocNumExistsMessage()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gsr = new ServiceRepository(factory);

            var rpDesc = gsr.RolePositionDescriptionRepository()
                .BaseList().OrderByDescending(r => r.RolePositionDescId).FirstOrDefault();

            if (rpDesc != null)
            {
                var msg = MessageHelper.DocNumExistsMessage();
                Console.WriteLine(rpDesc.DocNumber + " ~ " + msg);
            }
           
        }

        [TestMethod]
        public void TestSelectPublicServiceGrade()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gsr = new ServiceRepository(factory);

            var rpDesc = gsr.RolePositionDescriptionRepository().ListForPositionDescriptions()
                .OrderByDescending(r => r.RolePositionDescId).FirstOrDefault();

            var teachingBased = gsr.GradeRepository().IsTeachingBased(rpDesc.GradeCode);

            rpDesc.IsPositionDescription = false;
             if (teachingBased && !rpDesc.IsPositionDescription)
            {
                var msg = MessageHelper.SelectPublicServiceGrade();
                Console.WriteLine(msg);
                //("There is a mismatch in the selection, selected grade is not teaching based");
            }
             
        }


        [TestMethod]
        public void TestSelectTeachingServiceGrade()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gsr = new ServiceRepository(factory);

            var rpDesc = gsr.RolePositionDescriptionRepository().ListForRoleDescriptions()
                .OrderByDescending(r => r.RolePositionDescId).FirstOrDefault();

            var teachingBased = gsr.GradeRepository().IsTeachingBased(rpDesc.GradeCode);

            rpDesc.IsPositionDescription = true;
            if (!teachingBased && rpDesc.IsPositionDescription)
            {
                var msg = MessageHelper.SelectTeachingServiceGrade();
                Console.WriteLine(msg);
                //throw new InvalidOperationException("There is a mismatch in the selection, selected grade is teaching based");
            }

        }

        [TestMethod]
        public void TestPositionEndDate()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gsr = new ServiceRepository(factory);

            var position = gsr.PositionInformationRepository().List()
                .Where(po => po.PositionEndDate == null).FirstOrDefault();

            if (position != null)
            {
                var msg = MessageHelper.PositionEndDateRequired();
                Console.WriteLine(msg);
            }
        }

        [TestMethod]
        public void TestNoModifyPositionPermissionMsg()
        {
            var teamName = "vle";
            var msg = MessageHelper.NoModifyPositionPermission(teamName);
            Console.WriteLine(msg);

            var msg1 = MessageHelper.NoModifyPositionPermission("");
            Console.WriteLine(msg1);

            var msg2 = MessageHelper.NoModifyPositionPermission(null);
            Console.WriteLine(msg2);

        }

        [TestMethod]
        public void TestPositionNumAlreadyExistsMsg()
        {
            var msg = MessageHelper.PositionNumberExists("123456");
            Console.WriteLine(msg);
            
            var msg2 = MessageHelper.PositionNumberExists("");
            Console.WriteLine(msg2);
            
            var msg3 = MessageHelper.PositionNumberExists(null);
            Console.WriteLine(msg3);
        }


        [TestMethod]
        public void TestErrorOccured()
        {
            var msg = MessageHelper.ErrorOccured();
            Console.WriteLine(msg);
        }

        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }


    }

}

