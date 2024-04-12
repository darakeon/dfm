using System;
using DFM.Entities;
using DFM.Generic;

namespace DFM.Authentication
{
	public interface ISessionInfo
	{
		Boolean IsAdm { get; }
		Boolean IsRobot { get; }

		String Email { get; }
		Boolean HasTFA { get; }
		Boolean TFAPassword { get; }

		DateTime Now { get; }
		String TimeZone { get; }
		String Language { get; }

		Theme Theme { get; }

		Boolean UseCategories { get; }
		Boolean UseAccountsSigns { get; }
		Boolean MoveCheck { get; }
		Boolean SendMoveEmail { get; }
		Boolean UseCurrency { get; }

		Boolean Wizard { get; }

		ActivateWarningLevel ActivateWarning { get; }

		Misc Misc { get; }
	}
}
