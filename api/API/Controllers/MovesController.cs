using System;
using DFM.API.Helpers.Authorize;
using DFM.API.Models;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
    [Auth]
    public class MovesController : BaseApiController
    {
        [HttpGet]
        public IActionResult Create(Guid? id)
        {
            return json(() => new MovesCreateModel(id));
        }

        [HttpPost]
        public IActionResult Create()
        {
            var move = getFromBody<MoveInfo>();
            var model = new MovesCreateModel();
            return json(() => model.Save(move));
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            return json(() => new MovesModel().Delete(id));
        }

        [HttpPost]
        public IActionResult Check(Guid id, PrimalMoveNature nature)
        {
            return json(() => new MovesModel().Check(id, nature));
        }

        [HttpPost]
        public IActionResult Uncheck(Guid id, PrimalMoveNature nature)
        {
            return json(() => new MovesModel().Uncheck(id, nature));
        }

        [HttpGet]
        public IActionResult Lists()
        {
            return json(() => new MovesListsModel());
        }
    }
}
