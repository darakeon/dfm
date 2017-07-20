using System;

namespace DFM.Repositories
{
    public class DFMRepositoryException : Exception
    {
        public DFMRepositoryException(String message)
            : base(message) { }
    }
}
