using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Global;

namespace DFM.MVC.Areas.API.Controllers
{
    [DFMApiAuthorize]
    public class MovesController : BaseJsonController
    {
        public ActionResult Extract(String accounturl, Int32 id)
        {
	        return JsonGet(() => new MovesExtractModel(accounturl, id));
        }

        public ActionResult Summary(String accounturl, Int16 id)
        {
	        return JsonGet(() => new MovesSummaryModel(accounturl, id));
        }

		[HttpGet]
		public ActionResult Create(String accounturl, Int32? id)
		{
			return JsonGet(() => new MovesCreateGetModel(accounturl, id));
		}

        [HttpPost]
        public ActionResult Create(MovesCreatePostModel move)
        {
            try
            {
                move.Save();

                return JsonPostSuccess();
            }
            catch (DFMCoreException e)
            {
                return JsonPostError(MultiLanguage.Dictionary[e]);
            }
        }

		[HttpPost]
		public ActionResult Delete(Int32 id)
		{
			try
			{
				MovesDeleteModel.Delete(id);

				return JsonPostSuccess();
			}
			catch (DFMCoreException e)
			{
				return JsonPostError(MultiLanguage.Dictionary[e]);
			}
		}

    }
}
