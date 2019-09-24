using System;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Response;
using DFM.Generic;

namespace DFM.Tests.BusinessLogic.Helpers
{
	class ConfigHelper
	{
		private static String oldEmailConfig;

		public static void BreakTheEmailSystem()
		{
			oldEmailConfig = Cfg.EmailSender;
			Cfg.EmailSender = "MakeError";
		}

		internal static void FixTheEmailSystem()
		{
			Cfg.EmailSender = oldEmailConfig;
			oldEmailConfig = "";
		}



		internal static void ActivateEmailSystem()
		{
			Cfg.EmailSender = "";
		}

		internal static void DeactivateEmailSystem()
		{
			Cfg.EmailSender = "DontSend";
		}



		internal static void ActivateMoveEmailForUser(ServiceAccess sa)
		{
			var mainConfig = new ConfigInfo { SendMoveEmail = true };
			sa.Admin.UpdateConfig(mainConfig);
		}

		internal static void DeactivateMoveEmailForUser(ServiceAccess sa)
		{
			var mainConfig = new ConfigInfo { SendMoveEmail = false };
			sa.Admin.UpdateConfig(mainConfig);
		}
	}
}
