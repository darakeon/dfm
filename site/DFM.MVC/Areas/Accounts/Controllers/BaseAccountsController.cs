using System;
using System.Web.Mvc;
using System.Web.Routing;
using DFM.Entities.Enums;
using DFM.MVC.Areas.Accounts.Models;
using DFM.MVC.Helpers.Controllers;

namespace DFM.MVC.Areas.Accounts.Controllers
{
    public class BaseAccountsController : BaseController
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!Current.IsAuthenticated)
                return;
        }



        protected ActionResult CreateEditSchedule(BaseMoveModel model)
        {
            if (ModelState.IsValid)
            {
                var errors = model.CreateEditSchedule();

                AddErrors(errors);
            }

            if (ModelState.IsValid)
                return RedirectToAction("ShowMoves", "Report");

            return View(model);
        }


        
        [HttpPost]
        public Boolean ShowAccountList(MoveNature nature)
        {
            return nature == MoveNature.Transfer;
        }

    }
}
