using System.Web.Mvc;
using System.Web.Routing;
using DFM.BusinessLogic.Exceptions;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    public class BaseAccountsController : BaseController
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!Current.IsAuthenticated)
                return;

        }



        protected ActionResult CreateEditSchedule(BaseMoveModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selector = new AccountSelector(model.GenericMove.Nature, model.Account.Name, model.AccountName);

                    return saveOrUpdateAndRedirect(model, selector);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }
            }

            model.PopulateExcludingAccount(model.Account.Name);

            return View(model);
        }

        private ActionResult saveOrUpdateAndRedirect(BaseMoveModel model, AccountSelector selector)
        {
            model.SaveOrUpdate(selector);

            var account = model.GenericMove.AccOut() ?? model.GenericMove.AccIn();

            return RedirectToAction("ShowMoves", "Report", new { id = account.Url });
        }


    }
}
