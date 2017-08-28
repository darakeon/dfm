using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Routing;
using DK.MVC.Route;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;
using DFM.Multilanguage;
using DFM.Multilanguage.Helpers;

namespace DFM.MVC.Helpers.Global
{
	public class MultiLanguage
	{
		public static void Initialize()
		{
			var path = Path.Combine(
				Directory.GetCurrentDirectory(), "bin");

			PlainText.Initialize(path);
		}


		private static MultiLanguage dictionary;

		public static MultiLanguage Dictionary 
			=> dictionary ?? (dictionary = new MultiLanguage());


		public String this[params String[] phrase] => PlainText.Dictionary[section, Language, phrase];

		public String this[DFMCoreException exception] => this[exception.Type];

		public String this[ExceptionPossibilities exception] => this["Error", exception.ToString()];

		public String this[EmailStatus exception] => this["Email", exception.ToString()];

		private String this[String specificSection, params String[] phrase] => PlainText.Dictionary[specificSection, Language, phrase];


		public static String EmailLayout(String layout)
		{
			return PlainText.EmailLayout[Language, layout];
		}

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