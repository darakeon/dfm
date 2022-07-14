using System;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Areas.Api.Models
{
	public class UserSettingsModel : BaseApiModel
	{
		public UserSettingsModel()
		{
			UseCategories = isUsingCategories;
			MoveCheck = moveCheckingEnabled;
		}

		public Boolean UseCategories { get; set; }
		public Boolean MoveCheck { get; set; }

		internal void Save()
		{
			var mainSettings = new SettingsInfo
			{
				UseCategories = UseCategories,
				MoveCheck = MoveCheck
			};

			admin.UpdateSettings(mainSettings);
		}
	}
}
