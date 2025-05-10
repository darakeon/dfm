using System;
using DFM.API.Routes;
using DFM.BaseWeb.Routes;
using DFM.BaseWeb.Starters;

namespace DFM.API
{
	public class Program
	{
		public static void Main(String[] args)
		{
			Route.AddUrl<Apis.Main>();
			Route.AddUrl<Apis.Object>();

			Startup.Run<Program>(args);
		}
	}
}
