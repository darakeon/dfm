using System;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Response;
using DFM.Generic;

namespace DFM.Tests.Util
{
	public class ConfigHelper
	{
		private static String oldEmailConfig;

		public static void BreakTheEmailSystem()
		{
			oldEmailConfig = Cfg.EmailSender;
			Cfg.EmailSender = "MakeError";
		}

		public static void FixTheEmailSystem()
		{
			Cfg.EmailSender = oldEmailConfig;
			oldEmailConfig = "";
		}



		public static void ActivateEmailSystem()
		{
			Cfg.EmailSender = "";
		}

		public static void DeactivateEmailSystem()
		{
			Cfg.EmailSender = "DontSend";
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
