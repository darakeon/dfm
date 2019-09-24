using System;
using JetBrains.Annotations;

namespace DFM.MVC.Helpers
{
	public class Button
	{
		public String Text { get; }
		public String Action { get; }
		public Boolean PullLeft { get; }

		public Button(String text, [AspMvcAction] String action, Boolean pullLeft = false)
		{
			Text = text;
			Action = action;
			PullLeft = pullLeft;
		}
	}
}
