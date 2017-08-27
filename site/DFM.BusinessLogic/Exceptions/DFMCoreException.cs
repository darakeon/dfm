using System;
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



        public static void TestOtherIfTooLarge(Exception e)
        {
            if (e.InnerException != null && e.InnerException.Message.StartsWith("Data too long for column"))
                WithMessage(ExceptionPossibilities.TooLargeData);

            throw e;
        }


    }
}