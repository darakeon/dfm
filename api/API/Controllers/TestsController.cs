using System;
using DFM.API.Helpers.Authorize;
using DFM.API.Helpers.Controllers;
using DFM.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
	public class TestsController : BaseApiController
	{
		[HttpGetAndHead]
		public IActionResult Index()
		{
			return json(() => new TestsStatusModel());
		}

		[HttpGetAndHead]
		[Auth(AuthParams.Admin)]
		public IActionResult Throw()
		{
			throw new Exception("Logging right!");
		}
	}
}
