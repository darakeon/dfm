using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Services
{
    public class BaseService
    {
        protected BaseService(ServiceAccess serviceAccess)
        {
            Parent = serviceAccess;
        }

        protected ServiceAccess Parent { get; private set; }


        protected void BeginTransaction()
        {
            Parent.TransactionController.Begin();
        }

        protected void CommitTransaction()
        {
            try
            {
                Parent.TransactionController.Commit();
            }
            catch (Exception e)
            {
                DFMCoreException.TestOtherIfTooLarge(e);
            }
        }

        protected void RollbackTransaction()
        {
            Parent.TransactionController.Rollback();
        }

        

    

    }
}
