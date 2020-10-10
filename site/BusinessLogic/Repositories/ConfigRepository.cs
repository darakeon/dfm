using DFM.Entities;

namespace DFM.BusinessLogic.Repositories
{
	internal class ConfigRepository : Repo<Config>
	{
		internal void Update(Config config)
		{
			SaveOrUpdate(config);
		}
	}
}
