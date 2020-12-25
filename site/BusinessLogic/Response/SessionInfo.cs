using System;
using DFM.Authentication;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class SessionInfo : ISessionInfo
	{
		// ReSharper disable once UnusedMember.Global
		public SessionInfo() { }

		public SessionInfo(User user)
		{
			IsAdm = user.IsAdm;

			Email = user.Email;
			HasTFA = !String.IsNullOrEmpty(user.TFASecret);
			TFAPassword = user.TFAPassword;

			Now = user.Now();
			TimeZone = user.Config.TimeZone;
			Language = user.Config.Language;
			Theme = user.Config.Theme;

			UseCategories = user.Config.UseCategories;
			MoveCheck = user.Config.MoveCheck;
			SendMoveEmail = user.Config.SendMoveEmail;

			Wizard = user.Config.Wizard;
		}

		public Boolean IsAdm { get; }

		public String Email { get; }
		public Boolean HasTFA { get; }
		public Boolean TFAPassword { get; }

		public DateTime Now { get; }
		public String TimeZone { get; }
		public String Language { get; }
		public BootstrapTheme Theme { get; }

		public Boolean UseCategories { get; }
		public Boolean MoveCheck { get; }
		public Boolean SendMoveEmail { get; }

		public Boolean Wizard { get; }
	}
}
