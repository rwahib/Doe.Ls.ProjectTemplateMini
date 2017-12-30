using System;
using System.Linq;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Workflow;
using Doe.Ls.ProjectTemplate.Core.Test.Domain.SecurityModule;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow
    {
    public abstract class WorkflowBaseTest : SecurityBase, IWorkflowTest
        {
        public bool BoolIntegrateWithTrim { get; set; }

        [TestMethod]
        public abstract void ActionsForDraftTest();

        [TestMethod]
        public abstract void ActionsForDraftButHasNoAccessTest();

        [TestMethod]
        public abstract void ActionsForSubmittedTest();

        [TestMethod]
        public abstract void ActionsForSubmittedButHasNoAccessTest();

        [TestMethod]
        public abstract void ActionsForEndorsedTest();

        [TestMethod]
        public abstract void ActionsForEndorsedButHasNoAccessTest();

        [TestMethod]
        public abstract void ActionsForImportedTest();

        [TestMethod]
        public abstract void ActionsForImportedButHasNoAccessTest();

        [TestMethod]
        public abstract void ActionsForApprovedTest();

        [TestMethod]
        public abstract void ActionsForApprovedButHasNoAccessTest();
        
        }
    
    }