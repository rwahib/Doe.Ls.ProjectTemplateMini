


using System.ComponentModel;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{              
	public partial class Enums 
	{
		public enum StatusValue 
		{
			[Description("For any database records")]
			Deleted = -1,
			[Description("For any database records")]
			Approved = 10,
			[Description("For any database records")]
			Imported = 20,
			[Description("For any database workflow records")]
			Draft = 30,
			[Description("For any database workflow records")]
			Submitted = 40,
			[Description("Endorsed")]
			Endorsed = 50,
			[Description("Archived")]
			Archived = 60,
			[Description("")]
			Active = 1000,
			[Description("")]
			Inactive = 1010,
	 
		}

		
	} 
}

