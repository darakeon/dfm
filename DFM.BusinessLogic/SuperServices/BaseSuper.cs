namespace DFM.BusinessLogic.SuperServices
{
    public class BaseSuper
    {
        protected BaseSuper(ServiceAccess serviceAccess)
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

    }
}
