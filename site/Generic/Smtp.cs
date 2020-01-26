using System;
using Microsoft.Extensions.Configuration;

namespace DFM.Generic
{
	public class Smtp
	{
		public Smtp(IConfiguration smtp)
		{
			From = smtp["from"];
			DeliveryMethod = smtp["deliveryMethod"];
			Host = smtp["host"];
			Port = Int32.Parse(smtp["port"]);
			EnableSsl = smtp["enableSsl"] == "1";
			DefaultCredentials = smtp["defaultCredentials"] == "1";
			UserName = smtp["userName"];
			Password = smtp["password"];
		}

		public readonly String From;
		public readonly String DeliveryMethod;
		public readonly String Host;
		public readonly Int32 Port;
		public readonly Boolean EnableSsl;
		public readonly Boolean DefaultCredentials;
		public readonly String UserName;
		public readonly String Password;
	}
}
