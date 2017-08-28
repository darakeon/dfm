using System;
using JetBrains.Annotations;

namespace DFM.MVC.Helpers
{
	public class Button
	{
		public String Text { get; private set; }
		public String Action { get; private set; }
		public Boolean PullLeft { get; private set; }

		public Button(String text, [AspMvcAction] String action, Boolean pullLeft = false)
		{
			Text = text;
			Action = action;
			PullLeft = pullLeft;
		}
	}
}