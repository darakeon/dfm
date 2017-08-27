using DFM.BusinessLogic.Bases;
using DFM.Entities;

namespace DFM.BusinessLogic.Repositories
{
    internal class ConfigRepository : BaseRepository<Config>
    {
        internal ConfigRepository(IData<Config> repository) : base(repository) { }

        internal void Update(Config config)
        {
            SaveOrUpdate(config);
        }


    }

}
