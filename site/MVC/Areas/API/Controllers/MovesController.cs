using System;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Api.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Starters.Routes;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Areas.Api.Controllers
{
	[Area(Route.ApiArea), ApiAuth]
	public class MovesController : BaseJsonController
	{
		[HttpGetAndHead]
		public IActionResult Extract(String accountUrl, Int32 id)
		{
			return json(() => new MovesExtractModel(accountUrl, id));
		}

		[HttpGetAndHead]
		public IActionResult Summary(String accountUrl, Int16 id)
		{
			return json(() => new MovesSummaryModel(accountUrl, id));
		}

		[HttpGetAndHead]
		public IActionResult Create(Guid? id)
		{
			return json(() => new MovesCreateModel(id));
		}

		[HttpPost]
		public IActionResult Create()
		{
			var move = getFromBody<MoveInfo>();
			var model = new MovesCreateModel();
			return json(() => model.Save(move));
		}

		[HttpPost]
		public IActionResult Delete(Guid id)
		{
			return json(() => new MovesModel().Delete(id));
		}

		[HttpPost]
		public IActionResult Check(Guid id, PrimalMoveNature nature)
		{
			return json(() => new MovesModel().Check(id, nature));
		}

		[HttpPost]
		public IActionResult Uncheck(Guid id, PrimalMoveNature nature)
		{
			return json(() => new MovesModel().Uncheck(id, nature));
		}

		[HttpGetAndHead]
		public IActionResult Lists()
		{
			return json(() => new MovesListsModel());
		}
	}
}
