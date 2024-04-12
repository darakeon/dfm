using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Generic;
using DFM.Generic.Datetime;
using Keon.Util.Exceptions;

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
		public TipType TipType => (TipType)(ticket?.Type??TicketType.None);

		private ClientTicket getTicket(Boolean remember)
		{
			return clientGetTicket?.Invoke(remember);
		}

		public Boolean IsVerified => userService.VerifyTicketTFA();

		private readonly IDictionary<String, SessionInfo> sessions =
			new Dictionary<String, SessionInfo>();
		private readonly Random random = new();

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
			catch (SystemError)
			{
				return null;
			}
			catch (DKException)
			{
				return null;
			}
			catch
			{
				// random to not all the threads try the same time again
				var milliseconds = random.Next(1000);

				Thread.Sleep(milliseconds);

				return getSession(++count);
			}
		}

		private static Int64 nowKey()
		{
			var text = DateTime.UtcNow.UntilSecond();
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
		public Boolean IsRobot => IsAuthenticated && session.IsRobot;

		public String Email => session?.Email;

		public Boolean HasTFA => session?.HasTFA ?? false;
		public Boolean TFAPassword => session?.TFAPassword ?? false;

		public DateTime Now => session?.Now ?? DateTime.UtcNow;
		public String TimeZone => session?.TimeZone;
		public String Language => session?.Language;

		public Theme Theme => session?.Theme ?? Defaults.DefaultTheme;

		public Boolean UseCategories => session?.UseCategories ?? Defaults.SettingsUseCategories;
		public Boolean UseAccountsSigns => session?.UseAccountsSigns ?? Defaults.SettingsUseAccountsSigns;
		public Boolean MoveCheck => session?.MoveCheck ?? Defaults.SettingsMoveCheck;
		public Boolean SendMoveEmail => session?.SendMoveEmail ?? Defaults.SettingsSendMoveEmail;
		public Boolean UseCurrency => session?.UseCurrency ?? Defaults.SettingsUseCurrency;

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
