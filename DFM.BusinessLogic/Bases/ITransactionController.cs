namespace DFM.BusinessLogic.Bases
{
    public interface ITransactionController
    {
        void Begin();
        void Commit();
        void Rollback();
    }
}
