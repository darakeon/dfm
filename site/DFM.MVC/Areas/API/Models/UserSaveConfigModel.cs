using DFM.BusinessLogic.ObjectInterfaces;

namespace DFM.MVC.Areas.API.Models
{
	public class UserSaveConfigModel : UserGetConfigModel
	{
		internal void Save()
		{
			var mainConfig = new MainConfig
			{
				UseCategories = UseCategories,
				MoveCheck = MoveCheck
			};

			Admin.UpdateConfig(mainConfig);
		}
	}
}