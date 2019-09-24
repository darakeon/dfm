using System;
using BCrypt.Net;
using crypt = BCrypt.Net.BCrypt;

namespace DFM.Generic
{
	public static class Crypt
	{
		public static String Do(String text)
		{
			return crypt.HashPassword(text);
		}

		public static Boolean Check(String text, String hash)
		{
			try
			{
				return crypt.Verify(text, hash);
			}
			catch (SaltParseException)
			{
				return false;
			}
		}
	}
}
