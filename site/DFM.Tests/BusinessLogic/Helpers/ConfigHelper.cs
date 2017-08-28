using System;
using DFM.BusinessLogic;
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
			sa.Admin.UpdateConfig(null, null, true, null);
		}

		internal static void DeactivateMoveEmailForUser(ServiceAccess sa)
		{
			sa.Admin.UpdateConfig(null, null, false, null);
		}

		internal static void ActivateCategoriesUseForUser(ServiceAccess sa)
		{
			sa.Admin.UpdateConfig(null, null, null, true);
		}

		internal static void DeactivateCategoriesUseForUser(ServiceAccess sa)
		{
			sa.Admin.UpdateConfig(null, null, null, false);
		}



	}
}
