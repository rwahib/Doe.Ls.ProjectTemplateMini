using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Doe.Ls.EntityBase;
using Doe.Ls.EntityBase.BLLBase;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.DomainServices;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.UI.UserTasks;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule
    {
    [TestClass]
    public class UserRoleTests : SecurityBase
        {
        
        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {
            }

        [TestMethod]
        public void SearchListPropList()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            
            var sysAdmin = GetSampleUser(Enums.UserRole.SystemAdministrator,factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.Administrator, FormType.Search);

            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsHidden(),"UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsHidden(),"UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");


            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");

            }

        [TestMethod]
        public void DetailListPropList()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var sysAdmin = GetSampleUser(Enums.UserRole.SystemAdministrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.Administrator, FormType.Details);

            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");


            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");

            }

        [TestMethod]
        public void DeleteListPropList()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var sysAdmin = GetSampleUser(Enums.UserRole.SystemAdministrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.Administrator, FormType.Delete);

            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");


            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");

            }

        #region System admin user for Admin role
        
        [TestMethod]
        public void EditPropListForSysAdmin()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var sysAdmin = GetSampleUser(Enums.UserRole.SystemAdministrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.Administrator, FormType.Edit);

            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");
            
            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains("disabled"), "UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains('disabled')");

            }

        [TestMethod]
        public void CreatePropListForSysAdmin()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var sysAdmin = GetSampleUser(Enums.UserRole.SystemAdministrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.Administrator, FormType.Create);

            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");


            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");
            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains("disabled"), "UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains('disabled')");
            }

        #endregion

        #region System admin user for Division Editor

        [TestMethod]
        public void DivisionEditorEditPropList()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var sysAdmin = GetSampleUser(Enums.UserRole.SystemAdministrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.DivisionEditor, FormType.Edit);

            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");

            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains("disabled"), "UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains('disabled')");

            }

        [TestMethod]
        public void DivisionEditorCreatePropList()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var divList=new ServiceRepository(factory).ExecutiveRepository().List().ToArray();

            var sysAdmin = GetSampleUser(Enums.UserRole.SystemAdministrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.DivisionEditor, FormType.Create);

            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");

            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");
            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains("disabled"), "UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains('disabled')");


            var structProp = UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp);
            foreach (var div in divList)
            {
                Assert.IsTrue(structProp.PropertyValueList.Any(item => item.Value == div.ExecutiveCod),
                    "structProp.PropertyValueList.Any(item=>item.Value==div.ExecutiveCod)");
            }

            }

        #endregion

        #region Admin user for Power role

        [TestMethod]
        public void EditPropListForPower()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var sysAdmin = GetSampleUser(Enums.UserRole.Administrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.PowerUser, FormType.Edit);

            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");

            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains("disabled"), "UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains('disabled')");

            }

        [TestMethod]
        public void CreatePropListForPower()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var sysAdmin = GetSampleUser(Enums.UserRole.Administrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.PowerUser, FormType.Create);

            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");


            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");
            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains("disabled"), "UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains('disabled')");
            }

        #endregion

        #region Admin user for Division Editor

        [TestMethod]
        public void DivisionEditorEditPropListForAdmin()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var divList = new ServiceRepository(factory).ExecutiveRepository().List().ToArray();

            var sysAdmin = GetSampleUser(Enums.UserRole.Administrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.DivisionEditor, FormType.Edit);

            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsVisible(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsVisible(), "UiPropertyItem.Hidden(list,UserRoleModel.StructureIdProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsVisible(), "UiPropertyItem.Hidden(list,UserRoleModel.OrgLevelNameProp)");

            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains("disabled"), "UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains('disabled')");
            var structProp = UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp);
            foreach(var div in divList)
                {
                Assert.IsTrue(structProp.PropertyValueList.Any(item => item.Value == div.ExecutiveCod),
                    "structProp.PropertyValueList.Any(item=>item.Value==div.ExecutiveCod)");
                }
            }

        [TestMethod]
        public void DivisionEditorCreatePropListForAdmin()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var divList = new ServiceRepository(factory).ExecutiveRepository().List().ToArray();

            var sysAdmin = GetSampleUser(Enums.UserRole.Administrator, factory);
            var task = UserTaskFactory.GetTask(sysAdmin, factory);
            var list = task.GetPropertySettings(Enums.UserRole.DivisionEditor, FormType.Create);

            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgObjetcNameProp).IsVisible(), "UiPropertyItem.IsVisible(list,UserRoleModel.OrgObjetcNameProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp).IsVisible(), "UiPropertyItem.IsVisible(list,UserRoleModel.StructureIdProp)");
            Assert.IsTrue(UiPropertyItem.GetProperty(list, UserRoleModel.OrgLevelNameProp).IsVisible(), "UiPropertyItem.IsVisible(list,UserRoleModel.OrgLevelNameProp)");


            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).IsHidden(), "UiPropertyItem.Hidden(list,UserRoleModel.EmailProp)");
            Assert.IsFalse(UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains("disabled"), "UiPropertyItem.GetProperty(list, UserRoleModel.EmailProp).PropertyAttributes.Contains('disabled')");
            var structProp = UiPropertyItem.GetProperty(list, UserRoleModel.StructureIdProp);
            foreach(var div in divList)
                {
                Assert.IsTrue(structProp.PropertyValueList.Any(item => item.Value == div.ExecutiveCod),
                    "structProp.PropertyValueList.Any(item=>item.Value==div.ExecutiveCod)");
                }
            }

        #endregion

        [TestCleanup]
        public void CleanUp()
            {
            TestBase.CleanUnitTestData();
            }


        }

    }

