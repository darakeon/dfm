using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.Email;
using DFM.Entities.Bases;
using DFM.Entities.Enums;
using DFM.BusinessLogic.Exceptions;
using DFM.Entities.Extensions;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Entities;
using DFM.MVC.Authentication;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.MultiLanguage;
using DFM.MVC.MultiLanguage.Helpers;
using DFM.Repositories;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [Authorize]
    public class MoveController : Controller
    {
        private Int32 accountid;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            accountid = Int32.Parse(RouteData.Values["accountid"].ToString());
        }



        public ActionResult Create()
        {
            var model = new MoveCreateEditScheduleModel<Move>();
            
            model.Populate(accountid);

            return viewCES(model);
        }

        [HttpPost]
        public ActionResult Create(MoveCreateEditScheduleModel<Move> model)
        {
            return createEditSchedule(model);
        }



        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Create");

            var move = Services.Money.SelectMoveById(id.Value);

            if (isUnauthorized(move))
                return RedirectToAction("Create");



            var model = new MoveCreateEditScheduleModel<Move>(move);

            if (model.AccountID == accountid)
                return redirectToRightAccount(move);



            model.Populate(accountid);

            return viewCES(model);
        }

        private ActionResult redirectToRightAccount(Move move)
        {
            var action = RouteData.Values["action"].ToString();

            RouteData.Values["accountid"] = move.Out.Year.Account.ID;

            return RedirectToAction(action, RouteData.Values);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, MoveCreateEditScheduleModel<Move> model)
        {
            var oldMove =  Services.Money.SelectMoveById(id);

            if (isUnauthorized(oldMove))
                return RedirectToAction("Create");


            model.Move.ID = id;
            model.Move.Out = oldMove.Out;
            model.Move.In = oldMove.In;
            model.Move.Schedule = oldMove.Schedule;

            return createEditSchedule(model);
        }



        public ActionResult Schedule()
        {
            var model = new MoveCreateEditScheduleModel<FutureMove>();

            model.Populate(accountid);

            return viewCES(model);
        }

        [HttpPost]
        public ActionResult Schedule(MoveCreateEditScheduleModel<FutureMove> model)
        {
            return createEditSchedule(model);
        }



        private ActionResult createEditSchedule<T>(MoveCreateEditScheduleModel<T> model) 
            where T : BaseMove, new()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selector = new AccountSelector(model.Move.Nature, accountid, model.AccountID);

                    return saveOrUpdateAndRedirect(model.Move, selector.AccountOut, selector.AccountIn, EmailFormats.GetForMove);
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
                }
            }

            model.Populate(accountid);

            return viewCES(model);
        }

        private ActionResult saveOrUpdateAndRedirect(BaseMove baseMove, Account accountOut, Account accountIn, Format.GetterForMove getForMove)
        {
            if (baseMove is FutureMove)
            {
                var futureMove = (FutureMove)baseMove;

                Services.Money.SaveOrUpdateMove(futureMove, accountOut, accountIn);

                return RedirectToAction("Index", "Report");
            }
            
            if (baseMove is Move)
            {
                var move = (Move)baseMove;
                Services.Money.SaveOrUpdateMove(move, accountOut, accountIn, getForMove);

                return RedirectToAction("SeeMonth", "Report", new { id = (move.Out ?? move.In).Url() } );
            }
            
            throw new Exception("Not known type of Move");
        }


        public ActionResult AddDetail(Int32 position = 0, Int32 id = 0)
        {
            var detail =  Services.Money.SelectDetailById(id);

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
            var reportID = (move.In ?? move.Out).Url();

            if (!isUnauthorized(move))
                Services.Money.DeleteMove(move, EmailFormats.GetForMove);
            //else
            //    move = null;

            // TODO: implement messages on page head
            //var message = move == null
            //    ? PlainText.Dictionary["MoveNotFound"]
            //    : String.Format(PlainText.Dictionary["MoveDeleted"], move.Description);

            return RedirectToAction("SeeMonth", "Report", new { id = reportID });
        }



        private ActionResult viewCES<T>(MoveCreateEditScheduleModel<T> model) 
            where T : BaseMove, new()
        {
            return View("CreateEditSchedule", model.ConvertToGeneric());
        }

        private static Boolean isUnauthorized(Move move)
        {
            return move == null
                   || !move.AuthorizeCRUD(Current.User);
        }

    }
}
