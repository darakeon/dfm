using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
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
            
            model = Populate(model, accountid);

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Create(MoveCreateEditModel model)
        {
            return CreateEdit(model);
        }


        public ActionResult Edit(Int32 id)
        {
            var model = new MoveCreateEditModel
                            {
                                Move = moveData.SelectById(id),
                            };
            
            model = Populate(model, accountid);

            return View("CreateEdit", model);
        }

        [HttpPost]
        public ActionResult Edit(Int32 id, MoveCreateEditModel model)
        {
            model.Move.ID = id;

            return CreateEdit(model);
        }

        private ActionResult CreateEdit(MoveCreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.Move.Account = accountData.SelectById(accountid);


                Int32? categoryID = model.CategoryID ?? 0;

                model.Move.Category = categoryData.SelectById(categoryID.Value);


                try
                {
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

            model = Populate(model, accountid);

            return View("CreateEdit", model);
        }


        private MoveCreateEditModel Populate(MoveCreateEditModel model, Int32 accountID)
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


            
            NHManager.NhInitialize(Current.User.CategoryList);
            
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
    }
}
