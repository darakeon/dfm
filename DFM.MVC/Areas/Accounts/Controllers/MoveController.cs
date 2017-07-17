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

namespace DFM.MVC.Areas.Accounts.Controllers
{
    [Authorize]
    public class MoveController : Controller
    {
        private readonly AccountData accountData = new AccountData();
        private readonly MoveData moveData = new MoveData();
        private readonly CategoryData categoryData = new CategoryData();

        private Int32 accountid;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            accountid = Int32.Parse(RouteData.Values["accountid"].ToString());
        }


        public ActionResult Create()
        {
            var model = new MoveCreateEditModel
                            {
                                Date = DateTime.Today
                            };
            
            model = populate(model, accountid);

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Create(MoveCreateEditModel model)
        {
            return createEdit(model);
        }


        public ActionResult Edit(Int32 id)
        {
            var move = moveData.SelectById(id);

            var secondAccountID = 
                move.Nature == MoveNature.Transfer
                    ? move.In.ID : (Int32?)null;

            if (secondAccountID == accountid)
            {
                var action = RouteData.Values["action"].ToString();

                RouteData.Values["accountid"] = move.Out.ID;

                return RedirectToAction(action, RouteData.Values);
            }

            var model = new MoveCreateEditModel
                            {
                                Move = move,
                                AccountID = secondAccountID
                            };
            
            model = populate(model, accountid);

            return View("CreateEdit", model);
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
                    switch (model.Move.Nature)
                    {
                        case MoveNature.Out:
                            model.Move.Out = accountData.SelectById(accountid);
                            break;
                        case MoveNature.In:
                            model.Move.In = accountData.SelectById(accountid);
                            break;
                        case MoveNature.Transfer:
                            model.Move.Out = accountData.SelectById(accountid);
                            model.Move.In = accountData.SelectById(model.AccountID ?? 0);
                            break;
                        default:
                            throw new Exception("Move Nature doesn't exist");
                    }

                    model.Move.Category = categoryData.SelectById(model.CategoryID ?? 0);

                    moveData.SaveOrUpdate(model.Move);

                    return RedirectToRoute(
                            RouteNames.Default,
                            new { action = "Index", controller = "Account" }
                        );
                }
                catch (CoreValidationException e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            model = populate(model, accountid);

            return View("CreateEdit", model);
        }


        private MoveCreateEditModel populate(MoveCreateEditModel model, Int32 accountID)
        {
            model.MakeAccountTransferList(accountID);


            model.IsDetailed = model.Move.HasRealDetails();

            if (!model.Move.DetailList.Any())
            {
                var detail = new Detail();
                model.Move.AddDetail(detail);
            }

            if (model.Move.Category != null)
            {
                model.CategoryID = model.Move.Category.ID;
            }



            model.Title = RouteData.Values["action"].ToString();


            
            model.CategorySelectList = SelectListExtension
                .CreateSelect(Current.User.CategoryList, mv => mv.ID, mv => mv.Name);
            

            return model;
        }


        public ActionResult AddDetail(Int32 position = 0, Int32 id = 0, String description = null, Double value = 0)
        {
            var model = new MoveAddDetailModel(position, id, description, value);

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
            String message;

            try
            {
                var move = moveData.SelectById(id);

                moveData.Delete(move);

                message = move == null
                    ? "No Move to delete."
                    : String.Format("Move '{0}' deleted.", move.Description);
            }
            catch (Exception e)
            {
                message = String.Format("Error: {0}", e.MostInner().Message);
            }

            return new JsonResult { Data = new { message } };
        }
    }
}
