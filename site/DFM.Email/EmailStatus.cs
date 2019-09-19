using System;

namespace DFM.Email
{
	[Flags]
	public enum EmailStatus
	{
		None = 0,
		EmailDisabled = 1,
		EmailSent = 2,
		InvalidSubject = 4,
		InvalidBody = 8,
		InvalidAddress = 16,
		EmailNotSent = 32,
	}

	public static class EmailStatusExtension
	{
		public static Boolean IsWrong(this EmailStatus emailStatus)
		{
			return emailStatus > EmailStatus.EmailSent;
		}
	}

}
