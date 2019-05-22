using Keon.Util.DB;
using Keon.Util.Exceptions;
using DFM.BusinessLogic.Exceptions;

namespace DFM.BusinessLogic.Repositories
{
	public abstract class BaseRepository<T> : Keon.NHibernate.Base.BaseRepository<T>
		where T : class, IEntity, new()
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
