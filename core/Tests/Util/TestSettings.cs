using DFM.BusinessLogic;
using DFM.BusinessLogic.Response;
using DFM.Generic;

namespace DFM.Tests.Util
{
	public class TestSettings
	{
		public static void BreakTheEmailSystem()
		{
			Cfg.ForceEmailError = true;
		}

		public static void FixTheEmailSystem()
		{
			Cfg.ForceEmailError = false;
		}

		public static void ActivateMoveEmailForUser(ServiceAccess sa)
		{
			var mainSettings = new SettingsInfo { SendMoveEmail = true };
			sa.Clip.UpdateSettings(mainSettings);
		}

		public static void DeactivateMoveEmailForUser(ServiceAccess sa)
		{
			var mainSettings = new SettingsInfo { SendMoveEmail = false };
			sa.Clip.UpdateSettings(mainSettings);
		}
	}
}
