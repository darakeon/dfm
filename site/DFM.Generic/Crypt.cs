using System;
using BCrypt.Net;
using bcrypt = BCrypt.Net.BCrypt;

namespace DFM.Generic
{
	public static class Crypt
	{
		public static String Do(String text)
		{
			return bcrypt.HashPassword(text);
		}

		public static Boolean Check(String text, String hash)
		{
			try
			{
				return bcrypt.Verify(text, hash);
			}
			catch (SaltParseException)
			{
				return false;
			}
		}
	}
}
