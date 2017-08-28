using DK.Generic.DB;
using DK.Generic.Exceptions;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Repositories
{
    public abstract class BaseRepository<T> : DK.NHibernate.Base.BaseRepository<T> 
        where T : class, IEntity
    {
        public new T SaveOrUpdate(T entity, params DelegateAction[] actions)
        {
            try
            {
                return base.SaveOrUpdate(entity, actions);
            }
            catch (DKException e)
            {
                DFMCoreException.TestOtherIfTooLarge(e);
                throw;
            }

        }

    }
}
