using System;

namespace DFM.Email
{
	public class MailError : Exception
	{
		public static Int32 ErrorCounter { get; private set; }
		public EmailStatus Type { get; }

		public static MailError WithMessage(EmailStatus type)
		{
			throw new MailError(type);
		}

		public static MailError WithMessage(Exception exception)
		{
			throw new MailError(exception);
		}


		private MailError(EmailStatus type)
			: base(type.ToString())
		{
			ErrorCounter++;
			Type = type;
		}

		public MailError(Exception e)
			: base("Exception on sending e-mail", e)
		{
			ErrorCounter++;
			Type = EmailStatus.EmailNotSent;
		}
	}



}
