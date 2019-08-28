using System;
using Keon.Util.Exceptions;
using DFM.Generic;

namespace DFM.BusinessLogic.Exceptions
{
	public class DFMCoreException : DFMException
	{
		public static Int32 ErrorCounter { get; private set; }
		public DfMError Type { get; private set; }

		public static DFMCoreException WithMessage(DfMError type)
		{
			throw new DFMCoreException(type);
		}

		private DFMCoreException(DfMError type)
			: base(type.ToString())
		{
			ErrorCounter++;
			Type = type;
		}
	}
}
