


using System.ComponentModel;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{              
	public partial class Enums 
	{
		public enum OrgLevel 
		{
			[Description("NA")]
			NA = -1,
			[Description("System")]
			System = 10,
			[Description("Application")]
			Application = 20,
			[Description("Division")]
			Division = 30,
			[Description("Directorate")]
			Directorate = 40,
			[Description("Business Unit")]
			BusinessUnit = 50,
	 
		}

		
	} 
}

