using System;

namespace DFM.BusinessLogic.Response
{
	public class ConfigInfo
	{
		public Boolean? UseCategories { get; set; }
		public Boolean? SendMoveEmail { get; set; }
		public Boolean? MoveCheck { get; set; }
		public Boolean? Wizard { get; set; }

		public String Language { get; set; }
		public String TimeZone { get; set; }
	}
}
