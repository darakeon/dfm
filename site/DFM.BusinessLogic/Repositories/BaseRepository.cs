using Ak.Generic.DB;
using Ak.Generic.Exceptions;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Repositories
{
    public abstract class BaseRepository<T> : Ak.NHibernate.BaseRepository<T> 
        where T : class, IEntity
    {
        public new T SaveOrUpdate(T entity, params DelegateAction[] actions)
        {
            try
            {
                return base.SaveOrUpdate(entity, actions);
            }
            catch (AkException e)
            {
                DFMCoreException.TestOtherIfTooLarge(e);
                throw;
            }

        }

    }
}
