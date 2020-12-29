using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using Keon.Util.Exceptions;

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

		private IDictionary<String, SessionInfo> sessions =
			new Dictionary<String, SessionInfo>();
		private Random random = new Random();

		private SessionInfo session => getSession(0);

		private SessionInfo getSession(Int32 count)
		{
			var key = ticket?.Key;

			if (String.IsNullOrEmpty(key))
				return null;

			if (count == 10)
				return null;

			var now = nowKey();
			clearDeadSessions(now);

			var dicKey = key + "_" + now;

			if (sessions.ContainsKey(dicKey))
				return sessions[dicKey];

			try
			{
				var value = userService.GetSession(key);
				if (value != null)
					sessions.Add(dicKey, value);
				return value;
			}
			catch (SystemError e)
			{
				return printAndReturn(e);
			}
			catch (DKException e)
			{
				return printAndReturn(e);
			}
			catch (Exception e)
			{
				// random to not all the threads try the same time again
				var milliseconds = random.Next(1000);

				e.TryLogHandled($"Key not found {key}, retry in {milliseconds}ms");

				Thread.Sleep(milliseconds);

				return getSession(++count);
			}
		}

		private static SessionInfo printAndReturn(Exception e)
		{
			e.TryLogHandled("Problem on session recover");
			return null;
		}

		private static Int64 nowKey()
		{
			var date = DateTime.Now;
			var text = date.ToString("yyyyMMddHHmmss");
			var factor = Int64.Parse(text);
			return factor / 2;
		}

		private void clearDeadSessions(Int64 now)
		{
			sessions
				.Select(s => s.Key)
				.Where(key => !key.StartsWith(now.ToString()))
				.ToList()
				.ForEach(key => sessions.Remove(key, out SessionInfo _));
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
