using System;

namespace DFM.Generic
{
	public class SystemError : Exception
	{
		public SystemError(String message, Exception? inner = null)
			: base(message, inner) { }

	}
}
