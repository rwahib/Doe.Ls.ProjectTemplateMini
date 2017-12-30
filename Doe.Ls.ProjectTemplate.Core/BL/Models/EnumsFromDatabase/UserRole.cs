


using System.ComponentModel;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{              
	public partial class Enums 
	{
		public enum UserRole 
		{
			[Description("System Administrator")]
			SystemAdministrator = 1,
			[Description("Administrator")]
			Administrator = 2,
			[Description("Power User")]
			PowerUser = 3,
			[Description("DoE User")]
			DoEUser = 7,
			[Description("Guest")]
			Guest = 8,
			[Description("Business Unit Data Entry")]
			BusinessUnitDataEntry = 10,
			[Description("Business Unit Author")]
			BusinessUnitAuthor = 20,
			[Description("Directorate Data Entry")]
			DirectorateDataEntry = 30,
			[Description("Directorate Endorser")]
			DirectorateEndorser = 40,
			[Description("Division Editor")]
			DivisionEditor = 50,
			[Description("Division Approver")]
			DivisionApprover = 60,
			[Description("HR DataEntry")]
			HRDataEntry = 70,
	 
		}

		public class UserRoleValues
		{
 
			public const string SystemAdministrator = "SystemAdministrator";
 
			public const string Administrator = "Administrator";
 
			public const string PowerUser = "PowerUser";
 
			public const string DoEUser = "DoEUser";
 
			public const string Guest = "Guest";
 
			public const string BusinessUnitDataEntry = "BusinessUnitDataEntry";
 
			public const string BusinessUnitAuthor = "BusinessUnitAuthor";
 
			public const string DirectorateDataEntry = "DirectorateDataEntry";
 
			public const string DirectorateEndorser = "DirectorateEndorser";
 
			public const string DivisionEditor = "DivisionEditor";
 
			public const string DivisionApprover = "DivisionApprover";
 
			public const string HRDataEntry = "HRDataEntry";
 
		}
	} 
}

