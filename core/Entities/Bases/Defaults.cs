﻿using System;
using DFM.Generic;

namespace DFM.Entities.Bases
{
	public static class Defaults
	{
		public const Int32 PasswordMinimumLength = 9;
		public const Int32 PasswordMaximumLength = 273;

		public const Int32 PasswordErrorLimit = 5;
		public const Int32 TFAErrorLimit = 7;
		public const Int32 TFARemovedWarningDays = 7;

		public const String SettingsLanguage = "pt-BR";
		public const String SettingsTimeZone = "UTC-03:00";

		public const Boolean SettingsUseCategories = false;
		public const Boolean SettingsUseAccountsSigns = false;
		public const Boolean SettingsMoveCheck = false;
		public const Boolean SettingsSendMoveEmail = false;
		public const Boolean SettingsUseCurrency = false;

		public const Theme DefaultTheme = Theme.LightSober;

		public const Int32 TicketShowedPart = 5;
		public const Int32 TicketExpirationDays = 30;

	}
}
