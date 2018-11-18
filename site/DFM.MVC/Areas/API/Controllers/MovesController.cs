using System;
using System.Web.Mvc;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;

namespace DFM.MVC.Areas.API.Controllers
{
	[DFMApiAuthorize]
	public class MovesController : BaseJsonController
	{
		public ActionResult Extract(String accountUrl, Int32 id)
		{
			return JsonGet(() => new MovesExtractModel(accountUrl, id));
		}

		public ActionResult Summary(String accountUrl, Int16 id)
		{
			return JsonGet(() => new MovesSummaryModel(accountUrl, id));
		}

		[HttpGet]
		public ActionResult Create(Int32? id)
		{
			return JsonGet(() => new MovesCreateGetModel(id));
		}

		[HttpPost]
		public ActionResult Create(MovesCreatePostModel move)
		{
			return JsonPost(move.Save);
		}

		[HttpPost]
		public ActionResult Delete(Int32 id)
		{
			return JsonPost(() => MovesModel.Delete(id));
		}

		[HttpPost]
		public ActionResult Check(Int32 id)
		{
			return JsonPost(() => MovesModel.Check(id));
		}

		[HttpPost]
		public ActionResult Uncheck(Int32 id)
		{
			return JsonPost(() => MovesModel.Uncheck(id));
		}

	}
}
