using System;
using DFM.Authentication;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Generic;

namespace DFM.BusinessLogic.Response
{
	public class SessionInfo : ISessionInfo
	{
		// ReSharper disable once UnusedMember.Global
		public SessionInfo() { }

		public SessionInfo(User user)
		{
			var control = user.Control;
			var settings = user.Settings;

			IsAdm = control.IsAdm;
			IsRobot = control.IsRobot;

			Email = user.Email;
			HasTFA = user.HasTFA();
			TFAPassword = user.TFAPassword;

			Now = user.Now();
			TimeZone = settings.TimeZone;
			Language = settings.Language;
			Theme = settings.Theme;

			UseCategories = settings.UseCategories;
			UseAccountsSigns = settings.UseAccountsSigns;
			MoveCheck = settings.MoveCheck;
			SendMoveEmail = settings.SendMoveEmail;
			UseCurrency = settings.UseCurrency;

			Wizard = settings.Wizard;

			PlanLimitMoveDetail = control.Plan.MoveDetail;

			var age = DateTime.UtcNow - control.Creation;
			Age = (Int32)age.TotalDays;

			if (!control.Active)
			{
				ActivateWarning = Age switch
				{
					> 4 => ActivateWarningLevel.High,
					> 2 => ActivateWarningLevel.Low,
					_ => ActivateWarningLevel.None
				};
			}

			var dayLimit = DateTime.UtcNow.AddDays(-Defaults.TFARemovedWarningDays);
			TFAForgottenWarning =
				control.TFAForgotten != null
					&& control.TFAForgotten >= dayLimit;

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
		public Boolean TFAForgottenWarning { get; }

		public Misc Misc { get; }
	}
}
