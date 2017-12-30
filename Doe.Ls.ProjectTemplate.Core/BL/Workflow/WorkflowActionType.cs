
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Doe.Ls.ProjectTemplate.Core.BL.Models;

namespace Doe.Ls.ProjectTemplate.Core.BL.Workflow
{              
	 public partial  class WorkflowAction  
    {
        public List<WorkflowAction> GetAllActions()
	     {
            var actions=new List<WorkflowAction>
    {
	    		Delete,		Reject,		BringToDraft,		Submit,		Endorse,		MarkAsImported,		Approve,		Archive,		CreateNewVersion,		Clone,		Rename,		UpdatePositionNumber,		BulkBringToDraft,		BulkMarkAsImported,		MovePositions,		SyncPdOrRdAttachmentToTrim,		SyncALLPdOrRdAttachmentToTrim, 
		};
	         return actions.OrderBy(a => a.ActionId).ToList();
	     }

		#region all static values

		public static WorkflowAction Delete = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.Delete,
		};
				

		public static WorkflowAction Reject = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.Reject,
		};
				

		public static WorkflowAction BringToDraft = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.BringToDraft,
		};
				

		public static WorkflowAction Submit = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.Submit,
		};
				

		public static WorkflowAction Endorse = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.Endorse,
		};
				

		public static WorkflowAction MarkAsImported = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.MarkAsImported,
		};
				

		public static WorkflowAction Approve = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.Approve,
		};
				

		public static WorkflowAction Archive = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.Archive,
		};
				

		public static WorkflowAction CreateNewVersion = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.CreateNewVersion,
		};
				

		public static WorkflowAction Clone = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.Clone,
		};
				

		public static WorkflowAction Rename = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.Rename,
		};
				

		public static WorkflowAction UpdatePositionNumber = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.UpdatePositionNumber,
		};
				

		public static WorkflowAction BulkBringToDraft = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.BulkBringToDraft,
		};
				

		public static WorkflowAction BulkMarkAsImported = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.BulkMarkAsImported,
		};
				

		public static WorkflowAction MovePositions = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.MovePositions,
		};
				

		public static WorkflowAction SyncPdOrRdAttachmentToTrim = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.SyncPdOrRdAttachmentToTrim,
		};
				

		public static WorkflowAction SyncALLPdOrRdAttachmentToTrim = new WorkflowAction
	{
		ActionId=(int)Enums.WorkflowActions.SyncALLPdOrRdAttachmentToTrim,
		};
				

	 
	#endregion
				
	} 
}

namespace Doe.Ls.ProjectTemplate.Core.BL.Models{
public partial class Enums 
	{
		public enum WorkflowActions 
		{
			[Description("Delete")]
			Delete = -20,
			[Description("Reject")]
			Reject = -10,
			[Description("Bring to draft")]
			BringToDraft = 0,
			[Description("Submit")]
			Submit = 10,
			[Description("Endorse")]
			Endorse = 20,
			[Description("Mark as imported")]
			MarkAsImported = 30,
			[Description("Approve")]
			Approve = 40,
			[Description("Archive")]
			Archive = 50,
			[Description("Create new version")]
			CreateNewVersion = 60,
			[Description("Clone")]
			Clone = 70,
			[Description("Rename")]
			Rename = 80,
			[Description("Update position number")]
			UpdatePositionNumber = 90,
			[Description("Bulk bring to draft")]
			BulkBringToDraft = 100,
			[Description("Bulk mark as imported")]
			BulkMarkAsImported = 110,
			[Description("Move Positions")]
			MovePositions = 120,
			[Description("Sync Pd Or Rd attachment to Trim")]
			SyncPdOrRdAttachmentToTrim = 130,
			[Description("Sync ALL Pd or Rd attachment to Trim")]
			SyncALLPdOrRdAttachmentToTrim = 140,
	 
		}
	}
}