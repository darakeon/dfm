using System;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Bases;

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

        protected void VerifyUser()
        {
            if (Parent.Current.User == null || !Parent.Current.User.Active)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);
        }

        protected void VerifyMove(BaseMove move)
        {
            if (move == null)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.InvalidMove);

            if (move.User().Email != Parent.Current.User.Email)
                throw DFMCoreException.WithMessage(ExceptionPossibilities.Unauthorized);
        }

    

    }
}
