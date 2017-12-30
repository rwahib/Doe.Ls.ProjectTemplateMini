


using System.ComponentModel;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{              
	public partial class Enums 
	{
		public enum HierarchyLevel 
		{
			[Description("Not applicable")]
			NA = -1,
			[Description("Division")]
			Division = 10,
			[Description("Directorate")]
			Directorate = 20,
			[Description("Business Unit")]
			BusinessUnit = 30,
			[Description("Team")]
			Team = 40,
	 
		}

		
	} 
}

