using System;
using System.Web;

namespace DFM.BusinessLogic.Helpers
{
	public class Dfm
	{
		public static String Url
		{
			get
			{
				if (HttpContext.Current == null)
					return "https://dontflymoney.com";

				var url = HttpContext.Current.Request.Url;

				return url.Scheme + "://" + url.Authority;
			}
		}

	}
}
