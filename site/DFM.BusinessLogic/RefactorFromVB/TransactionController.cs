using System;
using NHibernate;

namespace VB.DBManager
{
    public class TransactionController
    {
        protected static ISession Session
        {
            get { return NHManager.Session; }
        }

        public void Begin()
        {
            if (Session.Transaction != null
                    && Session.Transaction.IsActive)
                throw new VBRepositoryException("There's a Transaction opened already.");

            Session.BeginTransaction();

            if (Session.Transaction == null
                    || !Session.Transaction.IsActive)
                throw new VBRepositoryException("Transaction not opened.");

        }


        public void Commit()
        {
            testTransaction();

            Session.Transaction.Commit();

            Session.Flush();
        }

        public void Rollback()
        {
            if (Session.Transaction.IsActive)
            {
                testTransaction();
                Session.Transaction.Rollback();
            }

            Session.Clear();
        }

        private static void testTransaction()
        {
            if (Session.Transaction.WasCommitted || Session.Transaction.WasRolledBack)
                throw new AccessViolationException("There's a Transaction opened already.");
        }


    }
}
