using System;
using System.Globalization;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Authentication
{
	public class Current<SignInInfo, SessionInfo>
		where SignInInfo : ISignInInfo, new()
		where SessionInfo : class, ISessionInfo, new()
	{
		private ISafeService<SignInInfo, SessionInfo> userService { get; }

		protected Current(ISafeService<SignInInfo, SessionInfo> userService, GetTicket getTicket)
		{
			this.userService = userService;
			clientGetTicket = getTicket;
		}

		private readonly GetTicket clientGetTicket;
		public delegate ClientTicket GetTicket(Boolean remember);

		public delegate String GetUrl();

		private ClientTicket ticket => getTicket(false);
		public String TicketKey => ticket?.Key;

		private ClientTicket getTicket(Boolean remember)
		{
			return clientGetTicket?.Invoke(remember);
		}

		public Boolean IsVerified => userService.VerifyTicketTFA();

		private SessionInfo session
		{
			get
			{
				try
				{
					var key = ticket?.Key;

					if (String.IsNullOrEmpty(key))
						return null;

					return userService.GetSession(key);
				}
				catch (SystemError)
				{
					return null;
				}
			}
		}

		public Boolean IsAuthenticated => session != null;

		public Boolean IsAdm => IsAuthenticated && session.IsAdm;

		public String Email => session?.Email;

		public Boolean HasTFA => session?.HasTFA ?? false;
		public Boolean TFAPassword => session?.TFAPassword ?? false;

		public DateTime Now => session?.Now ?? DateTime.UtcNow;
		public String TimeZone => session?.TimeZone;
		public String Language => session?.Language;
		public BootstrapTheme Theme => session?.Theme ?? Defaults.DefaultTheme;

		public Boolean UseCategories => session?.UseCategories ?? Defaults.ConfigUseCategories;
		public Boolean MoveCheck => session?.MoveCheck ?? Defaults.ConfigMoveCheck;
		public Boolean SendMoveEmail => session?.SendMoveEmail ?? Defaults.ConfigSendMoveEmail;

		public Boolean Wizard => session?.Wizard ?? false;

		public CultureInfo Culture => new CultureInfo(Language);

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
