using DFM.API.Helpers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
    public class StatusController : BaseApiController
    {
        [HttpGetAndHead]
        public IActionResult Index()
        {
            return json(() => new { status = "online" });
        }
    }
}
