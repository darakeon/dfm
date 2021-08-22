using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Repositories
{
	internal class TipsRepository : Repo<Tips>
	{
		public Tips By(User user, TipType type)
		{
			return SingleOrDefault(
				t => t.User.ID == user.ID
					&& t.Type == type
			);
		}
	}
}
