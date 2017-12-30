using System;
using System.Globalization;
using DFM.Entities;
using DFM.Generic;

namespace DFM.Authentication
{
	public class Current
	{
		private ISafeService userService { get; }
		private event GetTicket getTicket;

		public Current(ISafeService userService, GetTicket getTicket)
		{
			this.userService = userService;
			this.getTicket += getTicket;
		}

		public delegate TypedTicket GetTicket(Boolean? remember = null);



		public User User
		{
			get
			{
				try
				{
					var ticket = getTicket?.Invoke()?.Key;

					if (ticket == null)
						return null;

					return userService.GetUserByTicket(ticket);
				}
				catch (DFMException)
				{
					return null;
				}
			}
		}



		public Boolean IsAuthenticated => User != null;


		public String Language => User?.Config?.Language;


		public Boolean IsAdm => IsAuthenticated && User.IsAdm();
		public CultureInfo Culture => new CultureInfo(Language);


		public String Set(String username, String password, Boolean remember)
		{
			var ticket = getTicket?.Invoke(remember);

			if (ticket == null)
				return null;

			return userService.ValidateUserAndCreateTicket(username, password, ticket.Key, ticket.Type);
		}

		public void Clear()
		{
			var ticket = getTicket?.Invoke();

			if (ticket == null)
				return;

			userService.DisableTicket(ticket.Key);
		}



	}
}