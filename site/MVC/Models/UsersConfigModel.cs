using DFM.MVC.Models.UserConfig;

namespace DFM.MVC.Models
{
	public class UsersConfigModel : BaseSiteModel
	{
		public UsersConfigModel()
		{
			Main = new MainConfig(admin, current, translator, errorAlert);
			Info = new UserInfo(safe, translator, errorAlert);
			TFA = new TFAForm(safe, current, translator, errorAlert);
			ThemeOpt = new ThemeOptions(admin, Theme, translator, errorAlert);
		}

		public MainConfig Main { get; set; }
		public UserInfo Info { get; set; }
		public TFAForm TFA { get; set; }
		public ThemeOptions ThemeOpt { get; set; }

		public Form ActiveForm { get; set; }

		// ReSharper disable once UnusedMember.Global
		public enum Form
		{
			Options,
			Password,
			Email,
			Theme,
			TFA,
		}

	}
}
