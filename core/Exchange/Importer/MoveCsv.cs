using System;
using System.Collections.Generic;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Exchange.Exporter;

namespace DFM.Exchange.Importer
{
	public class MoveCsv
	{
		public String Description { get; set; }
		public String Date { get; set; }
		public String Category { get; set; }
		public String Nature { get; set; }
		public String In { get; set; }
		public String Out { get; set; }
		public String Value { get; set; }
		public String Conversion { get; set; }
	}
}
