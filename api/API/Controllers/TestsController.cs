using System;
using DFM.API.Authorize;
using DFM.API.Models;
using DFM.API.Routes;
using DFM.BaseWeb.Helpers.Authorize;
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
