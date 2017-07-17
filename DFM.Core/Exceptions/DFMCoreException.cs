using System;
using DFM.Core.Database.Base;

namespace DFM.Core.Exceptions
{
    public class DFMCoreException : Exception
    {
        internal static DFMCoreException WithMessage(ExceptionPossibilities message)
        {
            return new DFMCoreException(message);
        }


        private DFMCoreException(ExceptionPossibilities message)
            : base(message.ToString())
        {
            NHManager.Error();
        }

    }
}