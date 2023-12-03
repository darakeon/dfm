using DFM.API.Helpers.Authorize;
using DFM.API.Helpers.Controllers;
using DfM.Logs;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
    [Auth(AuthParams.Admin)]
    public class LogController : BaseApiController
    {
        [HttpGetAndHead]
        public IActionResult Count()
        {
            return json(() => new LogFile(false));
        }

        [HttpGetAndHead]
        public IActionResult List()
        {
            return json(() => new LogFile(true));
        }

        [HttpPost, HttpGetAndHead]
        public IActionResult Archive(int id)
        {
            return json(() => new LogFile(true).Archive(id));
        }
    }
}
