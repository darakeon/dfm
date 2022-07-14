using System;
using System.IO;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic.Settings
{
	public class Smtp
	{
		public Smtp(IConfiguration smtp)
		{
			var port = smtp["port"];

			Enum.TryParse(
				smtp["deliveryMethod"],
				true,
				out SmtpDeliveryMethod method
			);

			From = smtp["from"];
			DeliveryMethod = method;
			Host = smtp["host"];
			Port = port == null ? 0 : Int32.Parse(port);
			EnableSsl = smtp["enableSsl"] == "1";
			DefaultCredentials = smtp["defaultCredentials"] == "1";
			UserName = smtp["userName"];
			Password = smtp["password"];

			PickupDirectory = Path.Combine(
				Directory.GetCurrentDirectory(),
				smtp["pickupDirectoryLocation"]
			);
		}

		public readonly String From;
		public readonly SmtpDeliveryMethod DeliveryMethod;
		public readonly String Host;
		public readonly Int32 Port;
		public readonly Boolean EnableSsl;
		public readonly Boolean DefaultCredentials;
		public readonly String UserName;
		public readonly String Password;
		public readonly String PickupDirectory;
	}
}
