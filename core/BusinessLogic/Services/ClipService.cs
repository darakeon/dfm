using DFM.BusinessLogic.Repositories;
using System;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Response;
using DFM.Entities;
using DFM.Generic.Datetime;
using DFM.Language;
using DFM.Generic;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Services
{
	public class ClipService : Service
	{
		internal ClipService(ServiceAccess serviceAccess, Repos repos)
			: base(serviceAccess, repos) { }

		public void ReMisc(String password)
		{
			parent.Auth.VerifyUser();

			inTransaction("ReMisc", () =>
			{
				var user = parent.Auth.GetCurrent();

				var validPassword =
					repos.User.VerifyPassword(user, password);

				if (!validPassword)
					throw Error.WrongPassword.Throw();

				repos.Control.ReMisc(user);
			});
		}

		public void UpdateSettings(SettingsInfo info)
		{
			parent.Auth.VerifyUser();

			inTransaction("UpdateSettings", () =>
			{
				var user = parent.Auth.GetCurrent();
				updateSettings(info, user.Settings);
			});
		}

		private void updateSettings(SettingsInfo info, Settings settings)
		{
			if (info.Language != null && !PlainText.AcceptLanguage(info.Language))
				throw Error.LanguageUnknown.Throw();

			if (info.TimeZone != null && !TZ.IsValid(info.TimeZone))
				throw Error.TimeZoneUnknown.Throw();

			if (!String.IsNullOrEmpty(info.Language))
				settings.Language = info.Language;

			if (!String.IsNullOrEmpty(info.TimeZone))
				settings.TimeZone = info.TimeZone;

			if (info.UseCategories.HasValue)
				settings.UseCategories = info.UseCategories.Value;

			if (info.UseAccountsSigns.HasValue)
				settings.UseAccountsSigns = info.UseAccountsSigns.Value;

			if (info.MoveCheck.HasValue)
				settings.MoveCheck = info.MoveCheck.Value;

			if (info.SendMoveEmail.HasValue)
				settings.SendMoveEmail = info.SendMoveEmail.Value;

			if (info.Wizard.HasValue)
				settings.Wizard = info.Wizard.Value;

			repos.Settings.Update(settings);
		}

		public void EndWizard()
		{
			parent.Auth.VerifyUser();

			inTransaction("EndWizard", () =>
			{
				var settings = parent.Auth.GetCurrent().Settings;
				settings.Wizard = false;
				repos.Settings.Update(settings);
			});
		}

		public void ChangeTheme(Theme theme)
		{
			parent.Auth.VerifyUser();

			if (theme == Theme.None)
				throw Error.InvalidTheme.Throw();

			var user = parent.Auth.GetCurrent();

			var settings = user.Settings;
			settings.Theme = theme;

			inTransaction("ChangeTheme", () =>
			{
				repos.Settings.Update(settings);
			});
		}

		private static Int16 countdown = (Int16)(Cfg.Tips.Countdown - 1);
		private static Int16 repeat = Cfg.Tips.Repeat;
		private static Int16 reset = (Int16)(Cfg.Tips.Reset - 1);

		public String ShowTip()
		{
			var tips = getTips();
			String result = null;

			if (tips.Countdown > 0)
			{
				tips.Countdown--;
			}
			else
			{
				if (tips.Repeat > 0)
				{
					tips.Repeat--;
					result = tips.LastGiven() ?? tips.Random();
				}

				if (tips.Repeat == 0)
				{
					resetTip(tips);
				}
			}

			inTransaction(
				"ShowTip",
				() => repos.Tips.SaveOrUpdate(tips)
			);

			return result;
		}

		private Tips getTips()
		{
			var user = parent.Auth.VerifyUser();
			var type = parent.Current.TipType;

			return repos.Tips.By(user, type)
			       ?? createTip(user, type);
		}

		private static Tips createTip(User user, TipType type)
		{
			return new()
			{
				User = user,
				Type = type,
				Countdown = countdown,
				Repeat = repeat,
			};
		}

		private static void resetTip(Tips tips)
		{
			tips.Repeat = repeat;
			tips.Last = 0;

			if (tips.IsFull())
			{
				tips.Temporary = 0;
				tips.Countdown = reset;
			}
			else
			{
				tips.Countdown = countdown;
			}
		}

		public void DismissTip()
		{
			var tips = getTips();

			if (tips == null || tips.Last == 0)
				return;

			resetTip(tips);
			inTransaction(
				"DismissTip",
				() => repos.Tips.SaveOrUpdate(tips)
			);
		}

		public void DisableTip(TipTests tip)
		{
			DisableTip((UInt64)tip);
		}

		public void DisableTip(TipBrowser tip)
		{
			DisableTip((UInt64)tip);
		}

		public void DisableTip(TipMobile tip)
		{
			DisableTip((UInt64)tip);
		}

		internal void DisableTip(UInt64 tip)
		{
			var tips = getTips();

			tips.Permanent += tip;

			inTransaction(
				"DisableTip",
				() => repos.Tips.SaveOrUpdate(tips)
			);
		}
	}
}
