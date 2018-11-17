using System;
using DFM.BusinessLogic.ObjectInterfaces;

namespace DFM.MVC.Areas.API.Models
{
	public class UserConfigModel : BaseApiModel
	{
		public UserConfigModel()
		{
			var config = Current.User.Config;

			UseCategories = config.UseCategories;
			MoveCheck = config.MoveCheck;
		}

		public Boolean UseCategories { get; set; }
		public Boolean MoveCheck { get; set; }

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