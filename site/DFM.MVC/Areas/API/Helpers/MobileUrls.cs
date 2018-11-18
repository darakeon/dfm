using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace DFM.MVC.Areas.API.Helpers
{
	public class MobileUrls
	{
		private MobileUrls()
		{
			add("Users", "LogOn", "Login");
			add("Accounts", "Index", "Accounts");
			add("Reports", "ShowMoves", "Extract");
			add("Reports", "SummarizeMonths", "Summary");
			add("Moves", "Create", "MovesCreate");
			add("Moves", "Edit", "MovesCreate");
			add("Users", "Config", "Settings");
		}

		private const String @default = "Welcome";

		private IDictionary<String, IDictionary<String, String>> urls =
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


		public static MobileUrls Get = new MobileUrls();

		public String this[[AspMvcController] object controller, [AspMvcAction] object action]
		{
			get { return this[controller.ToString(), action.ToString()]; }
		}

		public String this[[AspMvcController] String controller, [AspMvcAction] String action]
		{
			get
			{
				return !urls.ContainsKey(controller)
						|| !urls[controller].ContainsKey(action)
					? @default
					: urls[controller][action];
			}
		}
	}
}