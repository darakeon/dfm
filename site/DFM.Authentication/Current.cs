using System;
using System.Globalization;
using DFM.Entities;
using DFM.Generic;

namespace DFM.Authentication
{
	public class Current
	{
		private ISafeService userService { get; }

		public Current(ISafeService userService, GetTicket getTicket)
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

		public User User
		{
			get
			{
				try
				{
					var key = ticket?.Key;

					return key == null
						? null
						: userService.GetUserByTicket(key);
				}
				catch (DFMException)
				{
					return null;
				}
			}
		}

		public Boolean IsAuthenticated => User != null;

		public String Language => User?.Config?.Language;

		public Boolean IsAdm => IsAuthenticated && User.IsAdm;
		public CultureInfo Culture => new CultureInfo(Language);

		public String Set(String username, String password, Boolean remember)
		{
			var newTicket = getTicket(remember);

			if (newTicket == null)
				return null;

			return userService.ValidateUserAndCreateTicket(
				username,
				password,
				newTicket.Key,
				newTicket.Type
			);
		}

		public void Clear()
		{
			if (ticket == null)
				return;

			userService.DisableTicket(ticket.Key);
		}



	}
}
