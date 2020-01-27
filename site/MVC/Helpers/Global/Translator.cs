using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Entities.Bases;
using DFM.Generic;
using DFM.Language;
using DFM.Language.Extensions;
using DFM.MVC.Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Error = DFM.BusinessLogic.Exceptions.Error;
using Version = DFM.Language.Version;

namespace DFM.MVC.Helpers.Global
{
	public class Translator
	{
		public Translator(HttpContext context)
		{
			this.context = context;
			PlainText.Initialize(getPath());
		}

		private static string getPath()
		{
			var path = Directory.GetCurrentDirectory();

			if (Cfg.LanguagePath != null)
			{
				path = Path.Combine(path, Cfg.LanguagePath);
			}

			return path;
		}

		private readonly HttpContext context;

		public String this[params String[] phrase] => PlainText.Site[section, Language, phrase];

		public String this[CoreError error] => this[error.Type];

		public String this[Error exception] => this["Error", exception.ToString()];

		public String this[EmailStatus exception] => this["Email", exception.ToString()];

		private String this[String specificSection, params String[] phrase] =>
			PlainText.Site[specificSection, Language, phrase];

		public String GetMonthName(Int32 month)
		{
			return PlainText.GetMonthName(month, Language);
		}

		private String section
		{
			get
			{
				var current = context.GetRoute();

				if (current == null)
					return "Ops";

				var controller = HttpUtility.UrlDecode(
					current["controller"].ToString().ToLower()
				);

				if (!controller.StartsWith("?")) return controller;

				var route = (Route) current["route"];
				return route.Defaults["controller"].ToString().ToLower();
			}
		}

		public String Language
		{
			get
			{
				var browserLanguage = context.Request
					.Headers["Accept-Language"].ToString()
					.Split(",").Where(PlainText.AcceptLanguage)
					.FirstOrDefault();

				var service = context.GetService();
				var userLanguage = service.Current.Language ?? browserLanguage;

				if (userLanguage == null || !PlainText.AcceptLanguage(userLanguage))
					userLanguage = Defaults.ConfigLanguage;

				return userLanguage;
			}
		}

		public IDictionary<T, String> GetEnumNames<T>()
		{
			return EnumHelper.GetEnumNames<T>(section, Language);
		}

		public IList<Version> Versions()
		{
			return Version.Get(Language);
		}
	}
}
