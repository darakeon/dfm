using System;
using DFM.Generic;

namespace DFM.MVC.Areas.Api.Models
{
	public class Environment
	{
		public Environment(Theme theme, String language)
		{
			Theme = theme.ToString();
			Language = language;
		}

		public String Theme { get; }
		public String Language { get; }
	}
}
