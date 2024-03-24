using System;
using DFM.BusinessLogic.Response;

namespace DFM.API.Models
{
	public class UserSettingsModel : BaseApiModel
	{
		public UserSettingsModel()
		{
			UseCategories = isUsingCategories;
			MoveCheck = moveCheckingEnabled;
		}

		public bool UseCategories { get; set; }
		public bool MoveCheck { get; set; }

		internal void Save()
		{
			var mainSettings = new SettingsInfo
			{
				UseCategories = UseCategories,
				MoveCheck = MoveCheck
			};

			clip.UpdateSettings(mainSettings);
		}
	}
}
