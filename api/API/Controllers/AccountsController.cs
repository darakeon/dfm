using System;
using DFM.API.Helpers.Authorize;
using DFM.API.Models;
using DFM.API.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
	[Auth]
	[Route(Apis.Main.ObjectPath)]
	[Route(Apis.Object.ObjectPath)]
	public class AccountsController : BaseApiController
	{
		[HttpGet]
		public IActionResult Index()
		{
			return json(() => new AccountsIndexModel());
		}

		[HttpGet]
		public IActionResult Summary(String id, Int16? year)
		{
			return json(() => new AccountsSummaryModel(id, year));
		}

		[HttpGet]
		public IActionResult Extract(String id, Int16? year, Int16? month)
		{
			return json(() => new AccountsExtractModel(id, year, month));
		}
	}
}
