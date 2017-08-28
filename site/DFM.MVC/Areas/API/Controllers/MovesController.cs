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
            try
            {
                var model = new MovesExtractModel(accounturl, id);

                return JsonGet(model);
            }
            catch (DFMCoreException e)
            {
                return JsonGetError(MultiLanguage.Dictionary[e]);
            }
        }

        public ActionResult Summary(String accounturl, Int16 id)
        {
            try
            {
                var model = new MovesSummaryModel(accounturl, id);

                return JsonGet(model);
            }
            catch (DFMCoreException e)
            {
                return JsonGetError(MultiLanguage.Dictionary[e]);
            }
        }

		[HttpGet]
		public ActionResult Create(String accounturl, Int32? id)
		{
			try
			{
				var model = new MovesCreateGetModel(accounturl, id);

				return JsonGet(model);
			}
			catch (DFMCoreException e)
			{
				return JsonGetError(MultiLanguage.Dictionary[e]);
			}
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
				return JsonGetError(MultiLanguage.Dictionary[e]);
			}
		}

    }
}
