using System;
using DFM.Email;
using DFM.MVC.Helpers.Global;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Models
{
	public class OpsCodeModel : BaseSiteModel
	{
		public OpsCodeModel(HttpContext httpContext)
		{
			var errorManager = new ErrorManager(httpContext, logService);
			EmailSent = errorManager.EmailSent;
		}

		public Error.Status EmailSent { get; set; }

		protected override Boolean ShowTip => false;
	}
}
