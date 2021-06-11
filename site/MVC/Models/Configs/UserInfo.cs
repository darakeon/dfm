using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Services;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Models.Configs
{
	public class UserInfo
	{
		public UserInfo(SafeService safe, Translator translator, ErrorAlert errorAlert)
		{
			this.safe = safe;
			this.translator = translator;
			this.errorAlert = errorAlert;
		}

		private readonly SafeService safe;
		private readonly Translator translator;
		private readonly ErrorAlert errorAlert;

		public String Email { get; set; }
		public String CurrentPassword { get; set; }

		public ChangePasswordInfo Password { get; set; }

		public IList<String> ChangePassword()
		{
			var errors = new List<String>();

			try
			{
				safe.ChangePassword(Password);
				errorAlert.Add("PasswordChanged");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}

		public IList<String> UpdateEmail()
		{
			var errors = new List<String>();

			try
			{
				safe.UpdateEmail(CurrentPassword, Email);

				errorAlert.Add("EmailUpdated");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
