using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.Entities.Enums;
using DFM.Extensions;
using DFM.BusinessLogic.Exceptions;
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
            var model = new MoveCreateEditScheduleModel();
            
            model.Populate(accountid);

            return viewCES(model);
        }

        [HttpPost]
        public ActionResult Create(MoveCreateEditScheduleModel model)
        {
            return createEditSchedule(model);
        }



        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Create");

            var move = Service.Access.Money.SelectMoveById(id.Value);

            if (isUnauthorized(move))
                return RedirectToAction("Create");



            var model = new MoveCreateEditScheduleModel(move);

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
        public ActionResult Edit(Int32 id, MoveCreateEditScheduleModel model)
        {
            var oldMove =  Service.Access.Money.SelectMoveById(id);

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
            var model = new MoveCreateEditScheduleModel();

            model.Populate(accountid, true);

            return viewCES(model);
        }

        [HttpPost]
        public ActionResult Schedule(MoveCreateEditScheduleModel model)
        {
            return createEditSchedule(model, true);
        }



        private ActionResult createEditSchedule(MoveCreateEditScheduleModel model, Boolean isSchedule = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Move.Category =  Service.Access.Admin.SelectCategoryById(model.CategoryID ?? 0);

                    var selector = new AccountSelector(model.Move.Nature, accountid, model.AccountID);

                    Service.Access.Money.SaveOrUpdateMove(model.Move, selector.AccountOut, selector.AccountIn, EmailFormats.GetForMove);

                    return RedirectToAction("SeeMonth", "Report",
                            new { id = (model.Move.Out ?? model.Move.In).Url() }
                        );
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
                }
            }

            model.Populate(accountid, isSchedule);

            return viewCES(model);
        }



        public ActionResult AddDetail(Int32 position = 0, Int32 id = 0)
        {
            var detail =  Service.Access.Money.SelectDetailById(id);

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
            var move =  Service.Access.Money.SelectMoveById(id);
            var reportID = (move.In ?? move.Out).Url();

            if (!isUnauthorized(move))
                Service.Access.Money.DeleteMove(move, EmailFormats.GetForMove);
            //else
            //    move = null;

            // TODO: implement messages on page head
            //var message = move == null
            //    ? PlainText.Dictionary["MoveNotFound"]
            //    : String.Format(PlainText.Dictionary["MoveDeleted"], move.Description);

            return RedirectToAction("SeeMonth", "Report", new { id = reportID });
        }



        private ActionResult viewCES(MoveCreateEditScheduleModel model)
        {
            return View("CreateEditSchedule", model);
        }

        private static Boolean isUnauthorized(Move move)
        {
            return move == null
                   || !move.AuthorizeCRUD(Current.User);
        }

    }
}
