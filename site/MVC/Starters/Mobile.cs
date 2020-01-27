using DFM.MVC.Helpers.Global;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DFM.MVC.Starters
{
	class Mobile
	{
		public static void ConfigureIP(IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				IP.SaveCurrent();
		}
	}
}
