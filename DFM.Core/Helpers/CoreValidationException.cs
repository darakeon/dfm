using System;
using Ak.DataAccess.NHibernate;
using DFM.Core.Database;

namespace DFM.Core.Helpers
{
    public class CoreValidationException : Exception
    {
        public CoreValidationException(String message) : base(message)
        {
            NHManager.Error();
        }
    }
}