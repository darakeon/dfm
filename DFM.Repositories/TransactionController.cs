using System;
using DFM.BusinessLogic.Bases;
using NHibernate;

namespace DFM.Repositories
{
    public class TransactionController : ITransactionController
    {
        protected static ISession Session
        {
            get { return NHManager.Session; }
        }

        public void Begin()
        {
            if (Session.Transaction != null
                    && Session.Transaction.IsActive)
                throw new DFMRepositoryException("There's a Transaction opened already.");

            Session.BeginTransaction();
        }


        public void Commit()
        {
            testTransaction();

            Session.Transaction.Commit();

            Session.Flush();
        }

        public void Rollback()
        {
            testTransaction();

            Session.Transaction.Rollback();

            Session.Clear();
        }

        private static void testTransaction()
        {
            if (Session.Transaction.WasCommitted || Session.Transaction.WasRolledBack)
                throw new AccessViolationException("There's a Transaction opened already.");
        }


    }
}
