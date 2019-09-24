using System;
using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.API.Controllers
{
	[ApiAuth]
	public class MovesController : BaseJsonController
	{
		[HttpGet]
		public ActionResult Extract(String accountUrl, Int32 id)
		{
			return json(() => new MovesExtractModel(accountUrl, id));
		}

		[HttpGet]
		public ActionResult Summary(String accountUrl, Int16 id)
		{
			return json(() => new MovesSummaryModel(accountUrl, id));
		}

		[HttpGet]
		public ActionResult Create(Int32? id)
		{
			return json(() => new MovesCreateGetModel(id));
		}

		[HttpPost]
		public ActionResult Create(MovesCreatePostModel move)
		{
			return json(move.Save);
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
