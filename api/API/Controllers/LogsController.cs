using System;
using DFM.API.Helpers.Authorize;
using DFM.API.Helpers.Controllers;
using DFM.BusinessLogic.Exceptions;
using DfM.Logs;
using Microsoft.AspNetCore.Mvc;

namespace DFM.API.Controllers
{
    [Auth(AuthParams.Admin)]
    public class LogsController : BaseApiController
    {
        [HttpGetAndHead]
        public IActionResult Count()
        {
            return json(() => new LogFile(false));
        }

        [HttpGetAndHead]
        public IActionResult Index()
        {
            return json(() => new LogFile(true));
        }

        [HttpPatch]
        public IActionResult Archive(String id)
        {
	        var model = new LogFile(true);
	        var found = model.Archive(id);

	        if (!found)
		        return error(Error.LogNotFound);

			return json(() => {});
        }
    }
}
