using System;
using DFM.BusinessLogic.InterfacesAndBases;
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
			return new User
			{
				Email = Email,
				Password = Password,
				Config = new Config
				{
					Language = Language,
					Wizard = EnableWizard,
					TimeZone = TimeZone
				}
			};
		}
	}
}
