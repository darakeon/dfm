using System;
using System.Globalization;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Authentication
{
	public class Current<SignInInfo, SessionInfo>
		where SignInInfo : ISignInInfo, new()
		where SessionInfo : class, ISessionInfo, new()
	{
		private IAuthService<SignInInfo, SessionInfo> userService { get; }

		protected Current(IAuthService<SignInInfo, SessionInfo> userService, GetTicket getTicket)
		{
			this.userService = userService;
			clientGetTicket = getTicket;
		}

		private readonly GetTicket clientGetTicket;
		public delegate ClientTicket GetTicket(Boolean remember);

		public delegate String GetUrl();

		private ClientTicket ticket => getTicket(false);
		public String TicketKey => ticket?.Key;
		public String SafeTicketKey => TicketKey[..Defaults.TicketShowedPart];
		public TipType TipType => (TipType)(ticket?.Type??TicketType.None);

		private ClientTicket getTicket(Boolean remember)
		{
			return clientGetTicket?.Invoke(remember);
		}


		private CacheAndRetry<Boolean> isVerifiedCache =>
			new(TicketKey, false, (key) => userService.VerifyTicketTFA());
		public Boolean IsVerified => isVerifiedCache.Get();

		private CacheAndRetry<SessionInfo> sessionCache =>
			new(TicketKey, null, (key) => userService.GetSession(key));
		private SessionInfo session => sessionCache.Get();


		public Boolean IsAuthenticated => session != null;

		public Boolean IsAdm => IsAuthenticated && session.IsAdm;
		public Boolean IsRobot => IsAuthenticated && session.IsRobot;

		public String Email => session?.Email;

		public Boolean HasTFA => session?.HasTFA ?? false;
		public Boolean TFAPassword => session?.TFAPassword ?? false;
		public Boolean TFAForgottenWarning => session?.TFAForgottenWarning ?? false;

		public DateTime Now => session?.Now ?? DateTime.UtcNow;
		public String TimeZone => session?.TimeZone;
		public String Language => session?.Language;

		public Theme Theme => session?.Theme ?? Defaults.DefaultTheme;

		public Boolean UseCategories => session?.UseCategories ?? Defaults.SettingsUseCategories;
		public Boolean UseAccountsSigns => session?.UseAccountsSigns ?? Defaults.SettingsUseAccountsSigns;
		public Boolean MoveCheck => session?.MoveCheck ?? Defaults.SettingsMoveCheck;
		public Boolean SendMoveEmail => session?.SendMoveEmail ?? Defaults.SettingsSendMoveEmail;
		public Boolean UseCurrency => session?.UseCurrency ?? Defaults.SettingsUseCurrency;

		public Int32 PlanLimitMoveDetail => session?.PlanLimitMoveDetail ?? 0;

		public Boolean Wizard => session?.Wizard ?? false;

		public ActivateWarningLevel ActivateWarning =>
			session?.ActivateWarning ?? ActivateWarningLevel.None;

		public Misc Misc => session?.Misc;

		public CultureInfo Culture => new(Language);

		public String Set(String username, String password, Boolean remember)
		{
			var newTicket = getTicket(remember);

			if (newTicket == null)
				return null;

			var info = new SignInInfo
			{
				Email = username,
				Password = password,
				TicketKey = newTicket.Key,
				TicketType = newTicket.Type,
			};

			return userService.CreateTicket(info);
		}

		public void Clear()
		{
			if (ticket == null)
				return;

			userService.DisableTicket(ticket.Key);
		}
	}
}
