using System;
using System.Collections.Generic;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.BusinessLogic.Services;
using DFM.MVC.Helpers.Global;
using Keon.TwoFactorAuth;

namespace DFM.MVC.Models.UserConfig
{
	public class TFAForm
	{
		public TFAForm(SafeService safe, Current current, Translator translator, ErrorAlert errorAlert)
		{
			this.safe = safe;
			this.current = current;
			this.translator = translator;
			this.errorAlert = errorAlert;

			TFA = new TFAInfo
			{
				Secret = Secret.Generate()
			};
		}

		private readonly SafeService safe;
		private readonly Current current;
		private readonly Translator translator;
		private readonly ErrorAlert errorAlert;

		public TFAInfo TFA { get; set; }

		private String identifier => current.Email;
		private String key => Base32.Convert(TFA.Secret);
		public String UrlPath => $"otpauth://totp/DfM:{identifier}?secret={key}";

		public IList<String> Activate()
		{
			var errors = new List<String>();

			try
			{
				safe.UpdateTFA(TFA);
				errorAlert.Add("TFAAuthenticated");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}
	}
}
