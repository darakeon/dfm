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

		private ClientTicket ticket => getTicket(false);
		public String TicketKey => ticket?.Key;

		private ClientTicket getTicket(Boolean remember)
		{
			return clientGetTicket?.Invoke(remember);
		}

		public Boolean IsVerified => userService.VerifyTicket();

		private SessionInfo session
		{
			get
			{
				try
				{
					var key = ticket?.Key;

					if (key == null)
						return null;

					return userService.GetSessionByTicket(key);
				}
				catch (DFMException)
				{
					return null;
				}
			}
		}

		public Boolean IsAuthenticated => session != null;

		public Boolean IsAdm => IsAuthenticated && session.IsAdm;

		public String Email => session?.Email;

		public DateTime Now => session?.Now ?? DateTime.UtcNow;
		public String TimeZone => session?.TimeZone;
		public String Language => session?.Language;
		public BootstrapTheme Theme => session?.Theme ?? Defaults.DEFAULT_THEME;

		public Boolean UseCategories => session?.UseCategories ?? Defaults.CONFIG_USE_CATEGORIES;
		public Boolean MoveCheck => session?.MoveCheck ?? Defaults.CONFIG_MOVE_CHECK;
		public Boolean SendMoveEmail => session?.SendMoveEmail ?? Defaults.CONFIG_SEND_MOVE_EMAIL;

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

			return userService.ValidateUserAndCreateTicket(info);
		}

		public void Clear()
		{
			if (ticket == null)
				return;

			userService.DisableTicket(ticket.Key);
		}
	}
}
