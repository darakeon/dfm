using System;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Exceptions;
using DFM.BusinessLogic.Services;
using DFM.Entities.Enums;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Global;

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

		protected DateTime now => current.Now;
		protected BootstrapTheme theme => current.Theme;
		protected String language => Translator.Language;
		protected Boolean wizard => current.Wizard;

		protected Boolean isUsingCategories => current.UseCategories ;
		protected Boolean moveCheckingEnabled => current.MoveCheck ;


		protected String login(String email, String password, Boolean rememberMe)
		{
			try
			{
				return current.Set(email, password, rememberMe);
			}
			catch (CoreError e)
			{
				if (e.Type == Error.DisabledUser)
					safe.SendUserVerify(email);

				throw;
			}
		}

		protected void logout()
		{
			current.Clear();
		}
	}
}
