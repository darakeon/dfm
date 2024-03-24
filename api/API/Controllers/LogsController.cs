using System;
using DFM.API.Helpers.Authorize;
using DFM.BusinessLogic.Exceptions;
using DfM.Logs;
using Microsoft.AspNetCore.Mvc;
using DFM.API.Starters.Routes;

namespace DFM.API.Controllers
{
	[Auth(AuthParams.Admin)]
	[Route(Apis.Main.ObjectPath)]
	[Route(Apis.Object.ObjectPath)]
	public class LogsController : BaseApiController
	{
		[HttpGet]
		public IActionResult Count()
		{
			return jsonNonBaseApi(() => new LogFile(false));
		}

		[HttpGet]
		public IActionResult Index()
		{
			return jsonNonBaseApi(() => new LogFile(true));
		}

		[HttpPatch]
		public IActionResult Archive(String id)
		{
			var model = new LogFile(true);
			var found = model.Archive(id);

			if (!found)
				return error(Error.LogNotFound);

			return json(() => {});
		}
	}
}
