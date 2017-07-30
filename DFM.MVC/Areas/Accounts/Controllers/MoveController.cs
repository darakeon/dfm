using System;
using System.Web.Mvc;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
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
        public ActionResult Create()
        {
            var model = new MoveCreateEditScheduleModel(OperationType.Creation);
            
            model.PopulateExcludingAccount(Account.Url);

            return viewCES(model);
        }

        [HttpPost]
        public ActionResult Create(MoveCreateEditScheduleModel model)
        {
            model.Schedule = null;
            model.Type = OperationType.Creation;

            return createEditSchedule<Move>(model);
        }



        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Create");

            var move = Services.Money.SelectMoveById(id.Value);

            var model = new MoveCreateEditScheduleModel(move, OperationType.Edit);

            if (model.AccountName == Account.Name)
                return redirectToRightAccount(move);

            model.PopulateExcludingAccount(Account.Name);

            return viewCES(model);
        }

        private ActionResult redirectToRightAccount(Move move)
        {
            var action = RouteData.Values["action"].ToString();

            RouteData.Values["accounturl"] = move.Out.Year.Account.Url;

            return RedirectToAction(action, RouteData.Values);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, MoveCreateEditScheduleModel model)
        {
            var oldMove = Services.Money.SelectMoveById(id);

            model.Move.ID = id;
            model.Schedule = oldMove.Schedule;
            model.Type = OperationType.Edit;

            return createEditSchedule<Move>(model);
        }



        public ActionResult Schedule()
        {
            var model = new MoveCreateEditScheduleModel(OperationType.Schedule);

            model.PopulateExcludingAccount(Account.Name);

            return viewCES(model);
        }

        [HttpPost]
        public ActionResult Schedule(MoveCreateEditScheduleModel model)
        {
            model.Type = OperationType.Schedule;

            return createEditSchedule<FutureMove>(model);
        }



        private ActionResult createEditSchedule<T>(MoveCreateEditScheduleModel model) 
            where T : BaseMove, new()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selector = new AccountSelector(model.Move.Nature, Account.Name, model.AccountName);

                    return saveOrUpdateAndRedirect<T>(model.Move, selector, model.CategoryName, model.Schedule);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }
            }

            model.PopulateExcludingAccount(Account.Name);

            return viewCES(model);
        }

        private ActionResult saveOrUpdateAndRedirect<T>(BaseMove baseMove, AccountSelector selector, String categoryName, Schedule schedule = null)
            where T : BaseMove, new()
        {
            if (typeof(T) == typeof(FutureMove))
            {
                var futureMove = baseMove.CastToChild<FutureMove>();

                Services.Robot.SaveOrUpdateSchedule(futureMove, selector.AccountOutName, selector.AccountInName, categoryName, schedule);

                return RedirectToAction("Index", "Report");
            }
            
            if (typeof(T) == typeof(Move))
            {
                var move = baseMove.CastToChild<Move>();

                Services.Money.SaveOrUpdateMove(move, selector.AccountOutName, selector.AccountInName, categoryName);

                return RedirectToAction("ShowMoves", "Report", new { id = (move.Out ?? move.In).Url() } );
            }
            
            throw new Exception("Not known type of Move");
        }


        public ActionResult AddDetail(Int32 position = 0, Int32 id = 0)
        {
            var detail = id == 0
                ? default(Detail)
                : Services.Money.SelectDetailById(id);

            var model = new MoveAddDetailModel(position, detail);

            return View(model);
        }

        [HttpPost]
        public Boolean ShowAccountList(MoveNature nature)
        {
            return nature == MoveNature.Transfer;
        }



        public ActionResult Delete(Int32 id)
        {
            var move =  Services.Money.SelectMoveById(id);

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



        private ActionResult viewCES(MoveCreateEditScheduleModel model) 
        {
            return View("CreateEditSchedule", model);
        }

    }
}
