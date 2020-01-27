using System;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Areas.Api.Models
{
	public class UserConfigModel : BaseApiModel
	{
		public UserConfigModel()
		{
			UseCategories = isUsingCategories;
			MoveCheck = moveCheckingEnabled;
		}

		public Boolean UseCategories { get; set; }
		public Boolean MoveCheck { get; set; }

		internal void Save()
		{
			var mainConfig = new ConfigInfo
			{
				UseCategories = UseCategories,
				MoveCheck = MoveCheck
			};

			admin.UpdateConfig(mainConfig);
		}
	}
}
