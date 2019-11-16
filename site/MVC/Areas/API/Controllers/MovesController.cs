using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Response;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.API.Controllers
{
	[ApiAuth]
	public class MovesController : BaseJsonController
	{
		[HttpGetAndHead]
		public ActionResult Extract(String accountUrl, Int32 id)
		{
			return json(() => new MovesExtractModel(accountUrl, id));
		}

		[HttpGetAndHead]
		public ActionResult Summary(String accountUrl, Int16 id)
		{
			return json(() => new MovesSummaryModel(accountUrl, id));
		}

		[HttpGetAndHead]
		public ActionResult Create(Int32? id)
		{
			return json(() => new MovesCreateModel(id));
		}

		[HttpPost]
		public ActionResult Create(MoveInfo move)
		{
			var model = new MovesCreateModel();
			return json(() => model.Save(move));
		}

		[HttpPost]
		public ActionResult Delete(Int32 id)
		{
			return json(() => MovesModel.Delete(id));
		}

		[HttpPost]
		public ActionResult Check(Int32 id)
		{
			return json(() => MovesModel.Check(id));
		}

		[HttpPost]
		public ActionResult Uncheck(Int32 id)
		{
			return json(() => MovesModel.Uncheck(id));
		}
	}
}
