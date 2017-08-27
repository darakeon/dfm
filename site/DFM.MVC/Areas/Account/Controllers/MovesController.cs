using System;
using System.Web.Mvc;
using DFM.Entities;
using DFM.MVC.Areas.Account.Models;
using DFM.MVC.Helpers.Authorize;
using DFM.Generic;

namespace DFM.MVC.Areas.Account.Controllers
{
    [DFMAuthorize]
    public class MovesController : BaseAccountsController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Create");
        }



        public ActionResult Create()
        {
            var model = new MovesCreateEditModel(OperationType.Creation);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MovesCreateEditModel model)
        {
            model.Type = OperationType.Creation;

            return CreateEditSchedule(model);
        }



        public ActionResult Edit(Int32? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Create");

            var model = new MovesCreateEditModel(id.Value, OperationType.Edit);

            if (model.ChosenAccountUrl == model.CurrentAccountUrl)
                return redirectToRightAccount(model.Move);

            return View(model);
        }

        private ActionResult redirectToRightAccount(Move move)
        {
            var action = RouteData.Values["action"].ToString();

            RouteData.Values["accounturl"] = move.Out.Year.Account.Url;

            return RedirectToAction(action, RouteData.Values);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, MovesCreateEditModel model)
        {
            model.Move.ID = id;

            return CreateEditSchedule(model);
        }



        public ActionResult Delete(Int32 id)
        {
            var model = new MoneyModel();

            model.DeleteMove(id);

            return RedirectToAction("ShowMoves", "Reports", new { id = model.ReportUrl });
        }
        


    }
}
