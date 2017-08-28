using System;
using DFM.BusinessLogic.ObjectInterfaces;

namespace DFM.Tests.Helpers
{
	class PasswordForm : IPasswordForm
	{
		public PasswordForm(String password) 
			: this(password, password) { }

		public PasswordForm(String password, String retypePassword)
		{
			Password = password;
			RetypePassword = retypePassword;
		}

		public string Password { get; set; }
		public string RetypePassword { get; set; }
	}
}
