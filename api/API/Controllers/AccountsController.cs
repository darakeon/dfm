using DFM.API.Helpers.Authorize;
using DFM.API.Helpers.Controllers;
using DFM.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
    [Auth]
    public class AccountsController : BaseApiController
    {
        [HttpGetAndHead]
        public IActionResult List()
        {
            return json(() => new AccountsListModel());
        }
    }
}
