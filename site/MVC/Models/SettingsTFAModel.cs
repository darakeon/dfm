using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using Keon.TwoFactorAuth;

namespace DFM.MVC.Models
{
	public class SettingsTFAModel : BaseSiteModel, SettingsModel
	{
		public SettingsTFAModel()
		{
			TFA = new TFAInfo
			{
				Secret = Secret.Generate()
			};

			IsActive = current.HasTFA;
		}

		public TFAInfo TFA { get; set; }

		private String identifier => current.Email;
		private String key => Base32.Convert(TFA.Secret);
		public String UrlPath => $"otpauth://totp/DfM:{identifier}?secret={key}";
		public Boolean IsActive { get; set; }
		public Boolean TFAPassword => current.TFAPassword;

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				if (IsActive)
					auth.RemoveTFA(TFA.Password);
				else
					auth.UpdateTFA(TFA);

				errorAlert.Add("TFAAuthenticated");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}

		public String BackTo { get; set; }
	}
}
