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

		private static String getPath()
		{
			var path = Directory.GetCurrentDirectory();

			if (Cfg.LanguagePath != null)
			{
				path = Path.Combine(path, Cfg.LanguagePath);
			}

			return path;
		}

		private readonly HttpContext context;

		public String this[params String[] phrase] =>
			PlainText.Site[section, Language, phrase];

		public String this[CoreError error] =>
			this[error.Type];

		public String this[Error exception] =>
			this["Error", exception.ToString()];

		public String this[EmailStatus exception] =>
			this["Email", exception.ToString()];

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

				if (current.ContainsKey("controller"))
				{
					var controller = HttpUtility.UrlDecode(
						current["controller"].ToLower()
					);

					if (!controller.StartsWith("?"))
						return controller;
				}

				if (current.ContainsKey("route"))
				{
					var route = (Route) current["route"];
					return route.Defaults["controller"].ToLower();
				}

				return "general";
			}
		}

		public String Language
		{
			get
			{
				var userLanguage = context.GetService().Current.Language;
				var language = userLanguage ?? browserLanguage();

				if (language == null || !PlainText.AcceptLanguage(language))
					language = Defaults.ConfigLanguage;

				return language;
			}
		}

		private String browserLanguage()
		{
			return sessionLanguage()
			    ?? headerLanguage();
		}

		private String sessionLanguage()
		{
			return context?.Session.GetString("Language");
		}

		private String headerLanguage()
		{
			return context.Request
				.Headers["Accept-Language"].ToString()
				.Split(",").Where(PlainText.AcceptLanguage)
				.FirstOrDefault();
		}

		public IDictionary<T, String> GetEnumNames<T>()
		{
			return EnumHelper.GetEnumNames<T>(section, Language);
		}

		public IList<Version> Versions()
		{
			return Version.Get(Language);
		}

		public Int32 CountTranslations(String sectionToCount, String phrasePrefix)
		{
			return PlainText.Site.CountTranslations(sectionToCount, Language, phrasePrefix);
		}
	}
}
