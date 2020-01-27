using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Email;
using Error = DFM.BusinessLogic.Exceptions.Error;

namespace DFM.MVC.Helpers.Global
{
	public class ErrorAlert
	{
		public ErrorAlert(Translator translator)
		{
			this.translator = translator;
		}

		private readonly Translator translator;

		private readonly SessionList<Error> errors =
			new SessionList<Error>("errors");

		public void Add(Error error)
		{
			errors.List.Add(error);
		}

		private readonly SessionList<EmailStatus> emailsStati =
			new SessionList<EmailStatus>("emailsStati");

		public void Add(EmailStatus emailStatus)
		{
			emailsStati.List.Add(emailStatus);
		}

		private readonly SessionList<String> texts =
			new SessionList<String>("texts");

		public void AddTranslated(String text)
		{
			texts.List.Add(text);
		}

		public void Add(String text)
		{
			texts.List.Add(translator[text]);
		}

		public IList<String> GetAndClean()
		{
			var list = new List<String>();

			list.AddRange(errors.List.Select(e => translator[e]));
			errors.List.Clear();

			list.AddRange(emailsStati.List.Select(e => translator[e]));
			emailsStati.List.Clear();

			list.AddRange(texts.List);
			texts.List.Clear();

			return list;
		}

		public Boolean Any()
		{
			return errors.List.Any()
				|| emailsStati.List.Any()
				|| texts.List.Any();
		}
	}

	internal class SessionList<T>
	{
		private readonly IDictionary<String, IList<T>> messages
			= new Dictionary<String, IList<T>>();

		private readonly String name;

		internal SessionList(String name)
		{
			this.name = name;

			if (!messages.ContainsKey(name))
				messages.Add(name, new List<T>());
		}

		internal IList<T> List => messages[name];
	}
}
