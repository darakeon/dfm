using System;
using System.Collections.Generic;

namespace DFM.Entities
{
	public class Clause
	{
		public Clause()
		{
			Items = new List<Clause>();
		}

		public String Text { get; set; }
		public IList<Clause> Items { get; set; }
	}
}
