using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.API.Models;
using DFM.MVC.Helpers;

namespace DFM.MVC.Areas.API.Controllers
{
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

    }
}
