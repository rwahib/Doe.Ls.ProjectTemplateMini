


using System.ComponentModel;

namespace Doe.Ls.ProjectTemplate.Core.BL.Models
{              
	public partial class Enums 
	{
		public enum PositionLevel 
		{
			[Description("TBC")]
			TBC=-1,
			[Description("Position")]
			Position=10,
			[Description("Support")]
			Support=20,
			[Description("Manager")]
			Manager=30,
			[Description("Executive")]
			Executive=40,
	 
		}
	} 
}

