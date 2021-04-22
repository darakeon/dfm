using DFM.BusinessLogic;
using DFM.BusinessLogic.Response;
using DFM.Generic;

namespace DFM.Tests.Util
{
	public class ConfigHelper
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
			var mainConfig = new ConfigInfo { SendMoveEmail = true };
			sa.Admin.UpdateConfig(mainConfig);
		}

		public static void DeactivateMoveEmailForUser(ServiceAccess sa)
		{
			var mainConfig = new ConfigInfo { SendMoveEmail = false };
			sa.Admin.UpdateConfig(mainConfig);
		}
	}
}
