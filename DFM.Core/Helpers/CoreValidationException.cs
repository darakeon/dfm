using System;
using DFM.Core.Database.Base;

namespace DFM.Core.Helpers
{
    public class DFMCoreException : Exception
    {
        public DFMCoreException(String message) : base(message)
        {
            NHManager.Error();
        }
    }
}