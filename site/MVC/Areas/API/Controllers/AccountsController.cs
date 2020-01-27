using DFM.MVC.Areas.Api.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Api.Controllers
{
	[Area(Route.ApiArea), ApiAuth]
	public class AccountsController : BaseJsonController
	{
		[HttpGetAndHead]
		public IActionResult List()
		{
			return json(() => new AccountsListModel());
		}
	}
}
