using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.MVC.Helpers.Views;
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

			UseTFA = current.HasTFA;
			UseTFAPassword = current.TFAPassword;
		}

		public Boolean UseTFA { get; set; }
		public Boolean UseTFAPassword { get; set; }
		public TFAInfo TFA { get; set; }

		private String identifier => current.Email;
		private String key => Base32.Convert(TFA.Secret);
		public String UrlPath => $"otpauth://totp/DfM:{identifier}?secret={key}";
		public Boolean IsActive { get; set; }

		public IList<String> Save()
		{
			var errors = new List<String>();

			try
			{
				if (!IsActive)
					auth.UpdateTFA(TFA);

				else if (!UseTFA)
					auth.RemoveTFA(TFA);
				
				else
					auth.UseTFAAsPassword(UseTFAPassword, TFA);

				errorAlert.Add("TFASuccess");
			}
			catch (CoreError e)
			{
				errors.Add(translator[e]);
			}

			return errors;
		}

		public String BackTo { get; set; }

		public WizardHL HL;
	}
}
