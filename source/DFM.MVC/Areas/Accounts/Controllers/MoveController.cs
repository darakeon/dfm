using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Authorize;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Extensions;
using DFM.Repositories;
using DFM.Generic;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [DFMAuthorize]
    public class MoveController : BaseAccountsController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Create");
        }



        public ActionResult Create()
        {
            var model = new MoveCreateEditModel(OperationType.Creation);
            
            model.PopulateExcludingAccount(Account.Url);

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MoveCreateEditModel model)
        {
            model.Type = OperationType.Creation;

            return CreateEditSchedule(model);
        }



        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Create");

            var move = Services.Money.GetMoveById(id.Value);

            var model = new MoveCreateEditModel(move, OperationType.Edit);

            if (model.AccountName == Account.Name)
                return redirectToRightAccount(move);

            model.PopulateExcludingAccount(Account.Name);

            return View(model);
        }

        private ActionResult redirectToRightAccount(Move move)
        {
            var action = RouteData.Values["action"].ToString();

            RouteData.Values["accounturl"] = move.Out.Year.Account.Url;

            return RedirectToAction(action, RouteData.Values);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, MoveCreateEditModel model)
        {
            model.Move.ID = id;

            return CreateEditSchedule(model);
        }



        [HttpPost]
        public Boolean ShowAccountList(MoveNature nature)
        {
            return nature == MoveNature.Transfer;
        }



        public ActionResult Delete(Int32 id)
        {
            var move =  Services.Money.GetMoveById(id);

            Services.Money.DeleteMove(id);
            //else
            //    move = null;

            // TODO: implement messages on page head
            //var message = move == null
            //    ? MultiLanguage.Dictionary["MoveNotFound"]
            //    : String.Format(MultiLanguage.Dictionary["MoveDeleted"], move.Description);

            var reportID = (move.In ?? move.Out).Url();

            return RedirectToAction("ShowMoves", "Report", new { id = reportID });
        }




    }
}
