using System;

namespace DFM.Core.Helpers
{
    public class CoreValidationException : Exception
    {
        public CoreValidationException(string message) : base(message) { }
    }
}