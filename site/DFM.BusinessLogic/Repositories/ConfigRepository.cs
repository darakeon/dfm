using System;
using DFM.Entities;
using Keon.NHibernate.Base;

namespace DFM.BusinessLogic.Repositories
{
	internal class ConfigRepository : BaseRepositoryLong<Config>
	{
		internal void Update(Config config)
		{
			SaveOrUpdate(config);
		}
	}
}
