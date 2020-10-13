using System;

namespace DFM.BusinessLogic.Response
{
	public class PasswordResetInfo : IPasswordForm
	{
		public String Token { get; set; }
		public String Password { get; set; }
		public String RetypePassword { get; set; }
	}
}
