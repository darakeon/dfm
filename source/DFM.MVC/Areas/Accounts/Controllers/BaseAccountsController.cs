using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Generic;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Extensions;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    public class BaseAccountsController : BaseController
    {
        protected Account Account;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!Current.IsAuthenticated)
                return;

            var url = RouteData.Values["accounturl"].ToString();

            Account = Services.Admin.GetAccountByUrl(url);
        }



        protected ActionResult CreateEditSchedule(GenericMoveModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selector = new AccountSelector(model.GenericMove.Nature, Account.Name, model.AccountName);

                    return saveOrUpdateAndRedirect(model, selector);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }
            }

            model.PopulateExcludingAccount(Account.Name);

            return View(model);
        }

        private ActionResult saveOrUpdateAndRedirect(GenericMoveModel model, AccountSelector selector)
        {
            model.SaveOrUpdate(selector);

            var account = model.GenericMove.AccOut() ?? model.GenericMove.AccIn();

            return RedirectToAction("ShowMoves", "Report", new { id = account.Url });
        }


    }
}
