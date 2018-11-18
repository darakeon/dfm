using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DFM.BusinessLogic.Exceptions;
using DFM.Email;

namespace DFM.MVC.Helpers.Global
{
	public class ErrorAlert
	{
		private static readonly SessionList<ExceptionPossibilities> errors =
			new SessionList<ExceptionPossibilities>("errors");

		public static void Add(ExceptionPossibilities error)
		{
			errors.List.Add(error);
		}



		private static readonly SessionList<EmailStatus> emailsStati =
			new SessionList<EmailStatus>("emailsStati");

		public static void Add(EmailStatus emailStatus)
		{
			emailsStati.List.Add(emailStatus);
		}



		private static readonly SessionList<String> texts =
			new SessionList<String>("texts");

		public static void AddTranslated(String text)
		{
			texts.List.Add(text);
		}

		public static void Add(String text)
		{
			texts.List.Add(MultiLanguage.Dictionary[text]);
		}



		public static IList<String> GetAndClean()
		{
			var list = new List<String>();

			list.AddRange(errors.List.Select(e => MultiLanguage.Dictionary[e]));
			errors.List.Clear();

			list.AddRange(emailsStati.List.Select(e => MultiLanguage.Dictionary[e]));
			emailsStati.List.Clear();

			list.AddRange(texts.List);
			texts.List.Clear();

			return list;
		}

		public static Boolean Any()
		{
			return errors.List.Any()
				|| emailsStati.List.Any()
				|| texts.List.Any();
		}


	}

	internal class SessionList<T>
	{
		private readonly String name;

		internal SessionList(String name)
		{
			this.name = name;
		}

		internal IList<T> List
		{
			get
			{
				if (session == null)
					session = new List<T>();

				return (List<T>)session;
			}
		}

		private object session
		{
			get { return HttpContext.Current.Session[name]; }
			set { HttpContext.Current.Session[name] = value; }
		}

	}
}