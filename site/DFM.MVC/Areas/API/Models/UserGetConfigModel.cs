using System;

namespace DFM.MVC.Areas.API.Models
{
	public class UserGetConfigModel : BaseApiModel
	{
		public UserGetConfigModel()
		{
			var config = Current.User.Config;

			UseCategories = config.UseCategories;
			MoveCheck = config.MoveCheck;
		}

		public Boolean UseCategories { get; set; }
		public Boolean MoveCheck { get; set; }

	}
}