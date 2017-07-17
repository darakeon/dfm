using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DFM.MVC.Helpers
{
    public static class Navigation
    {
        public static MvcHtmlString NumberNavFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Navigator navigator)
        {
            var textBox = htmlHelper.TextBoxFor(expression, navigator).ToHtmlString();

            var idFinder = new Regex(@"(id="")([A-Za-z_]*)");

            var fieldID = idFinder.Match(textBox).Groups[2].ToString();

            var lessBox = button(@"<", -1, fieldID, "navLess");
            var moreBox = button(@">", +1, fieldID, "navMore");

            return MvcHtmlString.Create(lessBox + textBox + moreBox);
        }

        private static String button(String symbol, Int16 add, String fieldID, String @class)
        {
            return String.Format(
                "<button type='button' class='navigation {0}' rel='{1}' add='{2}'>{3}</button>"
                    , @class, fieldID, add, symbol
                );
        }



        public class Navigator
        {
            public Int32 Min { get; set; }
            public Int32 Max { get; set; }

            public String Class { get { return "navigation"; } }
        }
    }
}