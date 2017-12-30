using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test.Domain.Workflow
    {
    public interface IWorkflowTest
        {
        bool  BoolIntegrateWithTrim { get; set; }
        void ActionsForDraftTest();
        void ActionsForDraftButHasNoAccessTest();
        void ActionsForSubmittedTest();
        void ActionsForSubmittedButHasNoAccessTest();
        void ActionsForEndorsedTest();
        void ActionsForEndorsedButHasNoAccessTest();
        void ActionsForImportedTest();
        void ActionsForImportedButHasNoAccessTest();
        void ActionsForApprovedTest();
        void ActionsForApprovedButHasNoAccessTest();
        

        }
    }