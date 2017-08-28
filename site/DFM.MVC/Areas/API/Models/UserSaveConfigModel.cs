namespace DFM.MVC.Areas.API.Models
{
	public class UserSaveConfigModel : UserGetConfigModel
	{
		internal void Save()
		{
			Admin.UpdateConfig(null, null, null, UseCategories, null);
		}
	}
}