using System;

namespace DFM.Core.Helpers
{
    public class CoreValidationException : Exception
    {
        public CoreValidationException(String message) : base(message) { }
    }
}