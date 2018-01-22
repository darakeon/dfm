using System;

namespace DFM.BusinessLogic.ObjectInterfaces
{
	public interface IMainConfig
	{
		Boolean? UseCategories { get; set; }
		Boolean? SendMoveEmail { get; set; }
		Boolean? MoveCheck { get; set; }
		Boolean? Wizard { get; set; }

		String Language { get; set; }
		String TimeZone { get; set; }
	}

	public class MainConfig : IMainConfig
	{
		public Boolean? UseCategories { get; set; }
		public Boolean? SendMoveEmail { get; set; }
		public Boolean? MoveCheck { get; set; }
		public Boolean? Wizard { get; set; }

		public String Language { get; set; }
		public String TimeZone { get; set; }
	}
}
