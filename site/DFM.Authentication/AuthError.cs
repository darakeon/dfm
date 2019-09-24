using System;

namespace DFM.Authentication
{
	class AuthError : Exception
	{
		private AuthError(String message) : base(message) { }

		public static AuthError NotWeb()
		{
			throw new AuthError("Not web!");
		}

		public static AuthError IsWeb()
		{
			throw new AuthError("It's web!");
		}

	}
}
