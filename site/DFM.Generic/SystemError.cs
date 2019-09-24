using System;

namespace DFM.Generic
{
	public class SystemError : Exception
	{
		public SystemError(String message)
			: base(message) { }

	}
}
