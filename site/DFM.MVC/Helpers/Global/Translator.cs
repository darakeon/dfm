using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Routing;
using Keon.MVC.Route;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Language;
using DFM.Language.Helpers;
using language = DFM.Language.Helpers.Language;

namespace DFM.MVC.Helpers.Global
{
	public class Translator
	{
		public static void Initialize()
		{
			var path = Path.Combine(
				Directory.GetCurrentDirectory(), "bin");

			PlainText.Initialize(path);
		}


		private static Translator dictionary;

		public static Translator Dictionary
			=> dictionary ?? (dictionary = new Translator());


		public String this[params String[] phrase] => PlainText.Dictionary[section, Language, phrase];

		public String this[DFMCoreException exception] => this[exception.Type];

		public String this[DfMError exception] => this["Error", exception.ToString()];

		public String this[EmailStatus exception] => this["Email", exception.ToString()];

		private String this[String specificSection, params String[] phrase] => PlainText.Dictionary[specificSection, Language, phrase];


		public static String GetMonthName(Int32 month)
		{
			return PlainText.GetMonthName(month, Language);
		}



		private static String section
		{
			get
			{
				var current = RouteInfo.Current;

				if (current?.RouteData == null)
					return "Ops";

				var controller = current["controller"].ToLower();

				if (controller.StartsWith("?"))
				{
					var defaults = ((Route) current.RouteData.Route).Defaults;

					controller = defaults["controller"].ToString().ToLower();
				}

				return controller;
			}
		}

		public static String Language
		{
			get
			{
				var browserLanguage =
					request.UserLanguages != null && request.UserLanguages.Length > 0
						? request.UserLanguages[0]
						: null;

				var userLanguage = Service.Current.Language ?? browserLanguage;

				if (userLanguage == null || !PlainText.AcceptLanguage(userLanguage))
					userLanguage = "en-US";

				return userLanguage;
			}
		}



		private static HttpRequest request => HttpContext.Current.Request;


		public static IDictionary<T, String> GetEnumNames<T>()
		{
			return EnumHelper.GetEnumNames<T>(section, Language);
		}


	}
}
