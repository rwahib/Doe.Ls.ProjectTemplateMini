


using System.ComponentModel;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{              
	public partial class Enums 
	{
		public enum TeamType
		{
			[Description("Regular")]
			Regular = -1,
			[Description("Assistant")]
			Assistant = 10,
			[Description("Pool")]
			Pool = 20,
	 
		}

		
	} 
}

