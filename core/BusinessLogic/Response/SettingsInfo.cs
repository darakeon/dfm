using System;

namespace DFM.BusinessLogic.Response
{
	public class SettingsInfo
	{
		public Boolean? UseCategories { get; set; }
		public Boolean? UseAccountsSigns { get; set; }
		public Boolean? SendMoveEmail { get; set; }
		public Boolean? MoveCheck { get; set; }
		public Boolean? Wizard { get; set; }

		public String Language { get; set; }
		public String TimeZone { get; set; }
	}
}
