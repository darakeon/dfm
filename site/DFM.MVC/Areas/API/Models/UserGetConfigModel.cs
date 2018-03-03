using System;

namespace DFM.MVC.Areas.API.Models
{
	public class UserGetConfigModel : BaseApiModel
	{
		public UserGetConfigModel()
		{
			var config = Current.User.Config;

			MoveCheck = config.MoveCheck;
		}

		public new Boolean UseCategories { get; set; }
		public Boolean MoveCheck { get; set; }

	}
}