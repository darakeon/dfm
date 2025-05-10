using System;
using DFM.BaseWeb.Errors;
using DFM.Email;
using Microsoft.AspNetCore.Http;

namespace DFM.MVC.Models
{
	public class OpsCodeModel(HttpContext httpContext) : BaseSiteModel
	{
		public Error.Status EmailSent { get; set; } =
			ErrorManager.GetEmailSent(httpContext);

		protected override Boolean ShowTip => false;
	}
}
