using DFM.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;

namespace DFM.BaseWeb.Starters
{
	public class Rewrite
	{
		public static void TestThemAll(IApplicationBuilder app)
		{
			var options = new RewriteOptions();

			Cfg.Rewrites.ForEach(
				(origin, destiny) =>
					options.AddRedirect(origin, destiny)
			);

			app.Use<Rewrite>("Rewrites", () => app.UseRewriter(options));
		}
	}
}
