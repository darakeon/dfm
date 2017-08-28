using System;
using DK.Generic.Exceptions;
using DFM.Generic;

namespace DFM.BusinessLogic.Exceptions
{
	public class DFMCoreException : DFMException
	{
		public static Int32 ErrorCounter { get; private set; }
		public ExceptionPossibilities Type { get; private set; }

		public static DFMCoreException WithMessage(ExceptionPossibilities type)
		{
			throw new DFMCoreException(type);
		}


		private DFMCoreException(ExceptionPossibilities type)
			: base(type.ToString())
		{
			ErrorCounter++;
			Type = type;
		}



		public static void TestOtherIfTooLarge(DKException e)
		{
			if (e.Message.StartsWith("TooLargeData"))
				WithMessage(ExceptionPossibilities.TooLargeData);

			throw e;
		}


	}
}