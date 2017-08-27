using System;

namespace DFM.Authentication
{
    class DFMAuthException : Exception
    {
        private DFMAuthException(String message) : base(message) { }

        public static DFMAuthException NotWeb()
        {
            throw new DFMAuthException("Not web!");
        }

        public static DFMAuthException IsWeb()
        {
            throw new DFMAuthException("It's web!");
        }

    }
}
