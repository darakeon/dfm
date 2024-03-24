using System;
using DFM.API.Helpers.Authorize;
using DFM.API.Models;
using DFM.API.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
	[Route(Apis.Main.ObjectPath)]
	[Route(Apis.Object.ObjectPath)]
	public class TestsController : BaseApiController
	{
		[HttpGet]
		public IActionResult Index()
		{
			return json(() => new TestsStatusModel());
		}

		[HttpGet]
		[Auth(AuthParams.Admin)]
		public IActionResult Throw()
		{
			throw new Exception("Logging right!");
		}
	}
}
