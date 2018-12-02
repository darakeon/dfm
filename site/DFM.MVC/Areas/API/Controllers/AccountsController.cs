using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.API.Controllers
{
	[DFMApiAuthorize]
	public class AccountsController : BaseJsonController
	{
		[HttpGet]
		public ActionResult List()
		{
			return json(() => new AccountsListModel());
		}
	}
}
