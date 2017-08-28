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
			this.getTicket = getTicket;
		}


		public delegate TypedTicket GetTicket();
		public TypedTicket Ticket => getTicket?.Invoke();


		public User User
		{
			get
			{
				try
				{
					return userService.GetUserByTicket(Ticket.Key);
				}
				catch (DFMException)
				{
					return null;
				}
			}
		}



		public Boolean IsAuthenticated => User != null;


		public String Language => User?.Config.Language;


		public Boolean IsAdm => IsAuthenticated && User.IsAdm();
		public CultureInfo Culture => new CultureInfo(Language);


		public String Set(String username, String password)
		{
			return userService.ValidateUserAndCreateTicket(username, password, Ticket.Key, Ticket.Type);
		}

		public void Reset(String username, String password)
		{
			Clean();
			Set(username, password);
		}

		public void Clean()
		{
			userService.DisableTicket(Ticket.Key);
		}
		


	}
}