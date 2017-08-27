using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Authorize;

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
        public ActionResult Create(String accounturl)
        {
            try
            {
                var model = new MoveCreateGetModel(accounturl);

                return JsonGet(model);
            }
            catch (DFMCoreException e)
            {
                return JsonGetError(MultiLanguage.Dictionary[e]);
            }
        }

        [HttpPost]
        public ActionResult Create(MoveCreatePostModel model)
        {
            try
            {
                //var model = new MovesSummaryModel(accounturl, id);

                return JsonGet(model);
            }
            catch (DFMCoreException e)
            {
                return JsonGetError(MultiLanguage.Dictionary[e]);
            }
        }

    }
}
