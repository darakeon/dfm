using System;

namespace DFM.BusinessLogic.Response
{
	public class ChangePasswordInfo : IPasswordForm, ITFAForm
	{
		public String CurrentPassword { get; set; }
		public String Password { get; set; }
		public String RetypePassword { get; set; }
		public String TFACode { get; set; }
	}
}
