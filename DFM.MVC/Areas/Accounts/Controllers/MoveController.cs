using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.Core.Entities.Extensions;
using DFM.Core.Exceptions;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Enums;
using DFM.MVC.Authentication;
using DFM.MVC.Helpers.Controllers;
using DFM.MVC.Helpers.Extensions;
using DFM.MVC.MultiLanguage;
using DFM.MVC.MultiLanguage.Helpers;

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

            var move = MoveData.SelectById(id.Value);

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
            var oldMove = MoveData.SelectById(id);

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
                    model.Move.Category = CategoryData.SelectById(model.CategoryID ?? 0);

                    var selector = new AccountSelector(model.Move.Nature, accountid, model.AccountID);

                    MoveData.SaveOrUpdate(model.Move, selector.AccountOut, selector.AccountIn, EmailFormats.GetForMove);

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
            var detail = DetailData.SelectById(id);

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
            var move = MoveData.SelectById(id);
            var reportID = (move.In ?? move.Out).Url();

            if (isUnauthorized(move))
                move = null;
            else
                MoveData.Delete(move, EmailFormats.GetForMove);

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
