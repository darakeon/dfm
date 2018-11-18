using System;
using DFM.Authentication;
using DFM.BusinessLogic.Helpers;
using DFM.BusinessLogic.Services;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.MVC.Helpers;

namespace DFM.MVC.Models
{
	public abstract class BaseModel
	{
		protected AdminService admin => Service.Access.Admin;
		protected MoneyService money => Service.Access.Money;
		protected ReportService report => Service.Access.Report;
		protected RobotService robot => Service.Access.Robot;
		protected SafeService safe => Service.Access.Safe;

		protected Current current => Service.Access.Current;
		protected Config config => current.User?.Config;

		protected BootstrapTheme theme => config?.Theme ?? Defaults.DEFAULT_THEME;
		protected String language => current.Language ?? Defaults.CONFIG_LANGUAGE;

		protected Boolean isUsingCategories => config?.UseCategories ?? Defaults.CONFIG_USE_CATEGORIES;
		protected Boolean moveCheckingEnabled => config?.MoveCheck ?? Defaults.CONFIG_MOVE_CHECK;

		protected DateTime today => current.User?.Now().Date ?? DateTime.UtcNow;

		protected String login(String email, String password, Boolean rememberMe)
		{
			return current.Set(email, password, rememberMe);
		}

		protected void logout()
		{
			current.Clear();
		}
	}
}
