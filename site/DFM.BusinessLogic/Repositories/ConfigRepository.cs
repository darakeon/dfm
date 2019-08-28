using DFM.Entities;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class ConfigRepository : BaseRepository<Config>
	{
		internal void Update(Config config)
		{
			SaveOrUpdate(config);
		}


	}

}
