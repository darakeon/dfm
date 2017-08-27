using System;

namespace VB.DBManager
{
    public class VBRepositoryException : Exception
    {
        public VBRepositoryException(String message)
            : base(message) { }
    }
}
