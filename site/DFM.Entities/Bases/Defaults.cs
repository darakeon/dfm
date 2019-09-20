using System;
using DFM.Entities.Enums;

namespace DFM.Entities.Bases
{
	public static class Defaults
	{
		public const String CONFIG_LANGUAGE = "pt-BR";
		public const String CONFIG_TIMEZONE = "E. South America Standard Time";

		public const Boolean CONFIG_SEND_MOVE_EMAIL = false;
		public const Boolean CONFIG_USE_CATEGORIES = false;
		public const Boolean CONFIG_MOVE_CHECK = false;

		public const BootstrapTheme DEFAULT_THEME = BootstrapTheme.Slate;

		public const Int32 TICKET_SHOWED_PART = 5;

	}
}
