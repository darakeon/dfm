using System;

namespace DFM.Entities.Bases
{
	public class MaxLen
	{
		public const Int16 AccountName = 20;
		public const Int16 AccountUrl = 20;
		public const Int16 CategoryName = 20;
		public const Int16 DetailDescription = 50;
		public const Int16 MoveDescription = DetailDescription;
		public const Int16 ScheduleDescription = DetailDescription;
		public const Int16 SecurityToken = 50;
		public const Int16 TicketKey = 52;
		public const Int16 UserPassword = 60;

		public const Int16 UserEmailUsername = 64;
		public const Int16 UserEmailDomain = 255;
		public const Int16 UserEmail = UserEmailUsername + 1 + UserEmailDomain;

		public const Int16 SettingsLanguage = 5;

	}
}
