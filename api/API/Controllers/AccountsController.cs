using System;
using DFM.API.Helpers.Authorize;
using DFM.API.Helpers.Controllers;
using DFM.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
	[Auth]
	public class AccountsController : BaseApiController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			return json(() => new AccountsIndexModel());
		}

		[HttpGetAndHead]
		public IActionResult Summary(String id, Int16? year)
		{
			return json(() => new AccountsSummaryModel(id, year));
		}

		[HttpGetAndHead]
		public IActionResult Extract(String id, Int16? year, Int16? month)
		{
			return json(() => new AccountsExtractModel(id, year, month));
		}
	}
}
