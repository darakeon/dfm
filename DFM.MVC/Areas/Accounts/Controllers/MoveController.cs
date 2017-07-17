using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.Core.Helpers;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Enums;
using DFM.MVC.Helpers;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [Authorize]
    public class MoveController : Controller
    {
        private readonly AccountData accountData = new AccountData();
        private readonly MoveData moveData = new MoveData();
        private readonly CategoryData categoryData = new CategoryData();
        private readonly DetailData detailData = new DetailData();

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
            return createEdit(model);
        }



        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue) return RedirectToAction("Create");

            var move = moveData.SelectById(id.Value);

            if (move == null) return RedirectToAction("Create");


            var model = new MoveCreateEditScheduleModel(move);

            if (model.AccountID == accountid)
                return redirectToRightAccount(move);



            model.Populate(accountid);

            return viewCES(model);
        }

        private ActionResult redirectToRightAccount(Move move)
        {
            var action = RouteData.Values["action"].ToString();

            RouteData.Values["accountid"] = move.Out.ID;

            return RedirectToAction(action, RouteData.Values);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, MoveCreateEditScheduleModel model)
        {
             model.Move.ID = id;

            return createEdit(model);
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
            return createEdit(model, true);
        }



        private ActionResult createEdit(MoveCreateEditScheduleModel model, Boolean isScheduler = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Move.Category = categoryData.SelectById(model.CategoryID ?? 0);

                    var currentAccount = accountData.SelectById(accountid);
                    var otherAccount = accountData.SelectById(model.AccountID ?? 0);

                    if (isScheduler)
                        moveData.Schedule(model.Move, currentAccount, otherAccount, model.Scheduler);
                    else
                        moveData.SaveOrUpdate(model.Move, currentAccount, otherAccount);

                    return RedirectToRoute(
                            RouteNames.Default,
                            new { action = "Index", controller = "Account" }
                        );
                }
                catch (DFMCoreException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
                }
            }

            model.Populate(accountid, isScheduler);

            return viewCES(model);
        }

        private ActionResult viewCES(MoveCreateEditScheduleModel model)
        {
            return View("CreateEditSchedule", model);
        }



        public ActionResult AddDetail(Int32 position = 0, Int32 id = 0)
        {
            var detail = detailData.SelectById(id);

            var model = new MoveAddDetailModel(position, detail);

            return View(model);
        }

        [HttpPost]
        public Boolean ShowAccountList(MoveNature nature)
        {
            return nature == MoveNature.Transfer;
        }



        [HttpPost]
        public JsonResult Delete(Int32 id)
        {
            var move = moveData.SelectById(id);

            moveData.Delete(move);

            var message = move == null
                ? PlainText.Dictionary["MoveNotFound"]
                : String.Format(PlainText.Dictionary["MoveDeleted"], move.Description);

            return new JsonResult { Data = new { message } };
        }
    }
}
