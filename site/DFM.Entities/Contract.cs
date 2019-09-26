﻿using System;
using System.Collections.Generic;
using System.Linq;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Contract : IEntityLong
	{
		public virtual Int64 ID { get; set; }

		public virtual DateTime BeginDate { get; set; }
		public virtual String Version { get; set; }

		public virtual IList<Terms> TermsList { get; set; }
	}
}
