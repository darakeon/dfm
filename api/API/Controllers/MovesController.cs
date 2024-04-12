using System;
using DFM.API.Helpers.Authorize;
using DFM.API.Models;
using DFM.API.Starters.Routes;
using DFM.BusinessLogic.Response;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
	[Auth]
	[Route(Apis.ControllerPath)]
	public class MovesController : BaseApiController
	{
		[HttpGet]
		[Route(Apis.IdPath)]
		public IActionResult Get(Guid id)
		{
			return json(() => new MovesCreateModel(id));
		}

		[HttpPost]
		public IActionResult Create([FromBody] MoveInfo move)
		{
			var model = new MovesCreateModel();
			move.Guid = Guid.Empty;
			return json(() => model.Save(move));
		}

		[HttpPut]
		[Route(Apis.IdPath)]
		public IActionResult Edit(Guid id, [FromBody] MoveInfo move)
		{
			var model = new MovesCreateModel();
			move.Guid = id;
			return json(() => model.Save(move));
		}

		[HttpDelete]
		[Route(Apis.IdPath)]
		public IActionResult Delete(Guid id)
		{
			var model = new MovesDeleteModel();
			return json(() => model.Delete(id));
		}

		[HttpPatch]
		[Route(Apis.IdActionPath)]
		public IActionResult Check(Guid id, [FromBody] MovesToggleCheckModel model)
		{
			return json(() => model.Check(id));
		}

		[HttpPatch]
		[Route(Apis.IdActionPath)]
		public IActionResult Uncheck(Guid id, [FromBody] MovesToggleCheckModel model)
		{
			return json(() => model.Uncheck(id));
		}

		[HttpGet]
		[Route(Apis.ActionPath)]
		public IActionResult Relations()
		{
			return json(() => new MovesListsModel());
		}
	}
}
