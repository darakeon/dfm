using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DFM.MVC.Helpers.Views
{
	public static class TimeNavigation
	{
		public static MvcHtmlString NumberNavFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Navigator navigator)
		{
			var textBox = htmlHelper.TextBoxFor(expression, navigator).ToHtmlString();

			var idFinder = new Regex(@"(id="")([A-Za-z_]*)");

			var fieldID = idFinder.Match(textBox).Groups[2].ToString();

			var lessBox = button(@"<", -1, fieldID, "navLess", navigator);
			var moreBox = button(@">", +1, fieldID, "navMore", navigator);

			return MvcHtmlString.Create(lessBox + textBox + moreBox);
		}

		private static String button(String symbol, Int16 add, String fieldID, String @class, Navigator navigator)
		{
			return $@"
				<button type='button' 
						class='{@class} {navigator.Class}' 
						rel='{fieldID}' 
						add='{add}'>
					{symbol}
				</button>";
		}

		public static Navigator Limits(Int32 min, Int32 max)
		{
			return Navigator.Limitated(min, max);
		}

		public static Navigator LimitsAndParent(Int32 min, Int32 max, String parent)
		{
			return Navigator.LimitatedWithParent(min, max, parent);
		}



		public class Navigator
		{
			internal static Navigator Limitated(Int32 min, Int32 max)
			{
				return new Navigator { Min = min, Max = max };
			}

			internal static Navigator LimitatedWithParent(Int32 min, Int32 max, String parent)
			{
				return new Navigator { Min = min, Max = max, Parent = parent };
			}

			public Int32 Min { get; private set; }
			public Int32 Max { get; private set; }
			public String Parent { get; private set; }

			public String Class => "navigation";
		}
	}
}