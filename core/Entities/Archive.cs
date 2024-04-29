using DFM.Entities.Enums;
using Keon.Util.DB;
using System;
using System.Collections.Generic;

namespace DFM.Entities;

public class Archive : IEntityLong
{
	public virtual Int64 ID { get; set; }

	public virtual ImportStatus Status { get; set; }

	public virtual User User { get; set; }

	public virtual IList<Line> LineList { get; set; }
}
