using System;
using JetBrains.Annotations;

namespace DFM.MVC.Helpers.Views
{
	public class WizardMenu
	{
		public String Resource { get; }
		public String Partial { get; }

		public WizardMenu(String resource, [AspMvcPartialView] String partial)
		{
			Resource = resource;
			Partial = partial;
		}
	}
}