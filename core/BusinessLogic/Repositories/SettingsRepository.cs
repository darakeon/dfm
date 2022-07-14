using DFM.Entities;

namespace DFM.BusinessLogic.Repositories
{
	internal class SettingsRepository : Repo<Settings>
	{
		internal void Update(Settings settings)
		{
			SaveOrUpdate(settings);
		}
	}
}
