using System;
using DFM.API.Helpers.Authorize;
using DFM.API.Helpers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
	[Auth(AuthParams.Admin)]
	public class TestsController : Controller
	{
		[HttpGetAndHead]
		public IActionResult Throw()
		{
			throw new Exception("Logging right!");
		}
	}
}
