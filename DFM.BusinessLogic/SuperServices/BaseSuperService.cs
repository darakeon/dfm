using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.SuperServices
{
    public class BaseSuperService
    {
        protected BaseSuperService(ServiceAccess serviceAccess)
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
            Parent.TransactionController.Commit();
        }

        protected void RollbackTransaction()
        {
            Parent.TransactionController.Rollback();
        }

        protected void VerifyUser()
        {
            if (Parent.Current.User == null || !Parent.Current.User.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);
        }

    }
}
