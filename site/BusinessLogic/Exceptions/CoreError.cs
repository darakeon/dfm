using System;
using DFM.Generic;

namespace DFM.BusinessLogic.Exceptions
{
	public class CoreError : SystemError
	{
		public static Int32 ErrorCounter { get; private set; }
		public Error Type { get; }

		internal CoreError(Error type, Exception inner = null)
			: base(type.ToString(), inner)
		{
			ErrorCounter++;
			Type = type;
		}
	}
}
