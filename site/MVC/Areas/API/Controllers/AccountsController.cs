using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.API.Controllers
{
	[ApiAuth]
	public class AccountsController : BaseJsonController
	{
		[HttpGetAndHead]
		public ActionResult List()
		{
			return json(() => new AccountsListModel());
		}
	}
}
