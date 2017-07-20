using System;

namespace DFM.BusinessLogic.Exceptions
{
    public class DFMCoreException : Exception
    {
        public static Int32 ErrorCounter { get; private set; }

        public static DFMCoreException WithMessage(ExceptionPossibilities message)
        {
            return new DFMCoreException(message);
        }


        private DFMCoreException(ExceptionPossibilities message)
            : base(message.ToString())
        {
            ErrorCounter++;
        }

    }
}