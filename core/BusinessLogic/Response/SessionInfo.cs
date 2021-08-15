using System;
using DFM.Authentication;
using DFM.Entities;
using DFM.Generic;

namespace DFM.BusinessLogic.Response
{
	public class SessionInfo : ISessionInfo
	{
		// ReSharper disable once UnusedMember.Global
		public SessionInfo() { }

		public SessionInfo(User user)
		{
			IsAdm = user.Control.IsAdm;
			IsRobot = user.Control.IsRobot;

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

			var age = DateTime.UtcNow - user.Control.Creation;
			Age = (Int32)age.TotalDays;

			if (!user.Control.Active)
			{
				ActivateWarning = Age switch
				{
					> 4 => ActivateWarningLevel.High,
					> 2 => ActivateWarningLevel.Low,
					_ => ActivateWarningLevel.None
				};
			}

			Misc = user.Control.Misc;
		}

		public Boolean IsAdm { get; }
		public Boolean IsRobot { get; }

		public String Email { get; }
		public Boolean HasTFA { get; }
		public Boolean TFAPassword { get; }

		public DateTime Now { get; }
		public String TimeZone { get; }
		public String Language { get; }
		public Theme Theme { get; }

		public Boolean UseCategories { get; }
		public Boolean MoveCheck { get; }
		public Boolean SendMoveEmail { get; }

		public Boolean Wizard { get; }

		public Int32 Age { get; }
		public ActivateWarningLevel ActivateWarning { get; }

		public Misc Misc { get; }
	}
}
