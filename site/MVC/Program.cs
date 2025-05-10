using System;
using DFM.BaseWeb.Starters;
using DFM.BaseWeb.Routes;
using DFM.MVC.Routes;

namespace DFM.MVC
{
	public class Program
	{
		public static void Main(String[] args)
		{
			Error.GetErrorUrl = () => Route.GetUrl<Default.Main>("Ops", "Code", "{0}");

			Route.AddUrl<Default.Robots>();
			Route.AddUrl<Default.SiteMap>();
			Route.AddUrl<Accounts>();
			Route.AddUrl<Default.Contract>();
			Route.AddUrl<Default.Misc>();
			Route.AddUrl<Default.Mail>();
			Route.AddUrl<Default.Mobile>();
			Route.AddUrl<Default.Main>();

			Startup.Run<Program>(args);
		}
	}
}
