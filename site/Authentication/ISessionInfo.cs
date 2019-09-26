using System;
using DFM.Entities.Enums;

namespace DFM.Authentication
{
	public interface ISessionInfo
	{
		Boolean IsAdm { get; }

		String Email { get; }

		DateTime Now { get; }
		String TimeZone { get; }
		String Language { get; }
		BootstrapTheme Theme { get; }

		Boolean UseCategories { get; }
		Boolean MoveCheck { get; }
		Boolean SendMoveEmail { get; }

		Boolean Wizard { get; }
	}
}
