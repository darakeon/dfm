using System;
using System.Text;
using DFM.Generic;
using DFM.MVC.Helpers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DFM.MVC.Controllers
{
	[NoWizard]
	public class GenericController : Controller
	{
		[HttpGetAndHead]
		public IActionResult Mobile()
		{
			return Redirect(Cfg.GooglePlay);
		}

		public IActionResult Reload()
		{
			return Content(" ");
		}

		public IActionResult Robots()
		{
			return crawler($"Sitemap: {site}/sitemap.txt");
		}

		public IActionResult SiteMap()
		{
			var pages = new[]
			{
				$"{site}/Users/SignUp",
				$"{site}/Users/LogOn",
				$"{site}/Users/ForgotPassword",
				$"{site}/Users/Contract"
			};

			return crawler(String.Join('\n', pages));
		}

		private IActionResult crawler(String content)
		{
			return Content(content, "text/plain", Encoding.UTF8);
		}

		private String site => $"{Request.Scheme}://{Request.Host}";
	}
}
