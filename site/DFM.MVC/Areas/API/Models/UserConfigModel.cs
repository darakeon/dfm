using System;
using DFM.BusinessLogic.InterfacesAndBases;

namespace DFM.MVC.Areas.API.Models
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
			var mainConfig = new ConfigOptions
			{
				UseCategories = UseCategories,
				MoveCheck = MoveCheck
			};

			admin.UpdateConfig(mainConfig);
		}
	}
}