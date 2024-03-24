using System;
using DFM.Generic;

namespace DFM.API.Models
{
	public class Environment
	{
		public Environment(Theme theme, string language)
		{
			Theme = theme;
			Language = language;
		}

		public Theme Theme { get; }
		public String Language { get; }
	}
}
