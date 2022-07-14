using System;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class SignUpInfo : IPasswordForm
	{
		public String Email { get; set; }
		public String Password { get; set; }
		public String RetypePassword { get; set; }

		public Boolean AcceptedContract { get; set; }
		public Boolean EnableWizard { get; set; }

		public String Language { get; set; }
		public String TimeZone { get; set; }

		public User GetEntity()
		{
			return new()
			{
				Email = Email,
				Password = Password,
				Settings = new Settings
				{
					Language = Language,
					Wizard = EnableWizard,
					TimeZone = TimeZone
				}
			};
		}
	}
}
