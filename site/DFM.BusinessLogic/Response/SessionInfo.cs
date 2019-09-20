using System;
using DFM.Authentication;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class SessionInfo : ISessionInfo
	{
		public SessionInfo() { }

		public SessionInfo(User user)
		{
			IsAdm = user.IsAdm;

			Email = user.Email;

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
