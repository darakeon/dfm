using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DFM.MVC.Helpers.Views
{
	public class MobileUrls
	{
		private MobileUrls()
		{
			add("Users", "LogOn", "Login");
			add("Users", "SignUp", "SignUp");
			add("Accounts", "Index", "Accounts");
			add("Reports", "Month", "Extract");
			add("Reports", "Year", "Summary");
			add("Moves", "Create", "MovesCreate");
			add("Moves", "Edit", "MovesCreate");
			add("Settings", "Index", "Settings");
		}

		private const String @default = "Welcome";

		private readonly IDictionary<String, IDictionary<String, String>> urls =
			new Dictionary<String, IDictionary<String, String>>();

		private void add([AspMvcController] String controller, [AspMvcAction] String action, String mobile)
		{
			if (!urls.ContainsKey(controller))
			{
				urls.Add(controller, new Dictionary<String, String>());
			}

			if (!urls[controller].ContainsKey(action))
			{
				urls[controller].Add(action, mobile);
			}
		}


		public static MobileUrls Get = new();

		public String this[[AspMvcController] Object controller, [AspMvcAction] Object action] =>
			this[controller.ToString(), action.ToString()];

		public String this[[AspMvcController] String controller, [AspMvcAction] String action] =>
			!urls.ContainsKey(controller)
			|| !urls[controller].ContainsKey(action)
				? @default
				: urls[controller][action];
	}
}
