using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.Entities.Extensions;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Extensions;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [Authorize]
    public class MoveController : BaseController
    {
        private String accountname;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var accountid = Int32.Parse(RouteData.Values["accountid"].ToString());

            accountname = Services.Admin.SelectAccountById(accountid).Name;
        }



        public ActionResult Create()
        {
            var model = new MoveCreateEditScheduleModel();
            
            model.Populate(accountname);

            return viewCES(model);
        }

        [HttpPost]
        public ActionResult Create(MoveCreateEditScheduleModel model)
        {
            model.Schedule = null;

            return createEditSchedule<Move>(model);
        }



        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Create");

            var move = Services.Money.SelectMoveById(id.Value);

            if (isUnauthorized(move))
                return RedirectToAction("Create");



            var model = new MoveCreateEditScheduleModel(move);

            if (model.AccountName == accountname)
                return redirectToRightAccount(move);



            model.Populate(accountname);

            return viewCES(model);
        }

        private ActionResult redirectToRightAccount(Move move)
        {
            var action = RouteData.Values["action"].ToString();

            RouteData.Values["accountid"] = move.Out.Year.Account.ID;

            return RedirectToAction(action, RouteData.Values);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, MoveCreateEditScheduleModel model)
        {
            var oldMove =  Services.Money.SelectMoveById(id);

            if (isUnauthorized(oldMove))
                return RedirectToAction("Create");

            model.Move.ID = id;
            model.Schedule = oldMove.Schedule;

            return createEditSchedule<Move>(model);
        }



        public ActionResult Schedule()
        {
            var model = new MoveCreateEditScheduleModel {IsSchedule = true};

            model.Populate(accountname);

            return viewCES(model);
        }

        [HttpPost]
        public ActionResult Schedule(MoveCreateEditScheduleModel model)
        {
            return createEditSchedule<FutureMove>(model);
        }



        private ActionResult createEditSchedule<T>(MoveCreateEditScheduleModel model) 
            where T : BaseMove, new()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selector = new AccountSelector(model.Move.Nature, accountname, model.AccountName);

                    return saveOrUpdateAndRedirect<T>(model.Move, selector, model.CategoryName, model.Schedule);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", MultiLanguage.Dictionary[e]);
                }
            }

            model.Populate(accountname);

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

            if (!isUnauthorized(move))
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

        private Boolean isUnauthorized(BaseMove baseMove)
        {
            return baseMove == null
                   || !baseMove.AuthorizeCRUD(Current.User);
        }

    }
}
