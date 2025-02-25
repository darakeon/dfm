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
			HasTFA = user.HasTFA();
			TFAPassword = user.TFAPassword;

			Now = user.Now();
			TimeZone = user.Settings.TimeZone;
			Language = user.Settings.Language;
			Theme = user.Settings.Theme;

			UseCategories = user.Settings.UseCategories;
			UseAccountsSigns = user.Settings.UseAccountsSigns;
			MoveCheck = user.Settings.MoveCheck;
			SendMoveEmail = user.Settings.SendMoveEmail;
			UseCurrency = user.Settings.UseCurrency;

			Wizard = user.Settings.Wizard;

			PlanLimitMoveDetail = user.Control.Plan.MoveDetail;

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

			Misc = user.GenerateMisc();
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
		public Boolean UseAccountsSigns { get; }
		public Boolean MoveCheck { get; }
		public Boolean SendMoveEmail { get; }
		public Boolean UseCurrency { get; }

		public Boolean Wizard { get; }

		public Int32 PlanLimitMoveDetail { get; }

		public Int32 Age { get; }
		public ActivateWarningLevel ActivateWarning { get; }

		public Misc Misc { get; }
	}
}
