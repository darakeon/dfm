using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Ak.Generic.Exceptions;
using Ak.MVC.Forms;
using DFM.Core.Helpers;
using DFM.MVC.Areas.Accounts.Models;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Enums;
using DFM.MVC.Authentication;
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
            var model = new MoveCreateEditModel { Date = DateTime.Today };
            
            model.Populate(accountid);

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Create(MoveCreateEditModel model)
        {
            return createEdit(model);
        }

        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue) return RedirectToAction("Create");


            var move = moveData.SelectById(id.Value);

            var model = new MoveCreateEditModel(move);

            if (model.AccountID == accountid)
                return redirectToRightAccount(move);



            model.Populate(accountid);

            return View("CreateEdit", model);
        }

        private ActionResult redirectToRightAccount(Move move)
        {
            var action = RouteData.Values["action"].ToString();

            RouteData.Values["accountid"] = move.Out.ID;

            return RedirectToAction(action, RouteData.Values);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, MoveCreateEditModel model)
        {
            model.Move.ID = id;

            return createEdit(model);
        }



        private ActionResult createEdit(MoveCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentAccount = accountData.SelectById(accountid);
                    var otherAccount = accountData.SelectById(model.AccountID ?? 0);

                    moveData.PlaceAccountsInMove(model.Move, currentAccount, otherAccount);

                    model.Move.Category = categoryData.SelectById(model.CategoryID ?? 0);

                    moveData.SaveOrUpdate(model.Move);

                    return RedirectToRoute(
                            RouteNames.Default,
                            new { action = "Index", controller = "Account" }
                        );
                }
                catch (CoreValidationException e)
                {
                    ModelState.AddModelError("", PlainText.Dictionary[e.Message]);
                }
            }

            model.Populate(accountid);

            return View("CreateEdit", model);
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
