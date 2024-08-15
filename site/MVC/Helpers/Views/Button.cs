using System;
using JetBrains.Annotations;

namespace DFM.MVC.Helpers.Views
{
	public class Button
	{
		public String Text { get; }
		public String Action { get; }
		public String Controller { get; }
		public String Asset { get; }
		public String Class { get; }
		public Boolean PullLeft { get; }

		private Button(String text, [AspMvcAction] String action, [AspMvcController] String controller, String asset, String @class, Boolean pullLeft = false)
		{
			Text = text;
			Action = action;
			Controller = controller;
			Asset = asset;
			Class = @class;
			PullLeft = pullLeft;
		}

		public static Button ForAction(String text, [AspMvcAction] String action, Boolean pullLeft = false)
		{
			return ForAction(text, action, null, pullLeft);
		}

		public static Button ForAction(String text, [AspMvcAction] String action, String @class, Boolean pullLeft = false)
		{
			return ForAction(text, action, null, @class, pullLeft);
		}

		public static Button ForAction(String text, [AspMvcAction] String action, [AspMvcController] String controller, String @class, Boolean pullLeft = false)
		{
			return new Button(text, action, controller, null, @class, pullLeft);
		}

		public static Button ForAsset(String text, String asset, String @class, Boolean pullLeft = false)
		{
			return new Button(text, null, null, asset, @class, pullLeft);
		}
	}
}
