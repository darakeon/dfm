using DFM.Entities.Enums;
using Keon.Util.DB;
using System;
using System.Collections.Generic;

namespace DFM.Entities;

public class Line : IEntityLong
{
	public virtual Int64 ID { get; set; }

	public virtual Int16 Position { get; set; }

	public virtual String Description { get; set; }
	public virtual DateTime Date { get; set; }
	public virtual String Category { get; set; }
	public virtual MoveNature? Nature { get; set; }
	public virtual String In { get; set; }
	public virtual String Out { get; set; }
	public virtual Decimal? Value { get; set; }
	public virtual Decimal? Conversion { get; set; }

	public virtual Archive Archive { get; set; }

	public virtual IList<Detail> DetailList { get; set; }

	public virtual ImportStatus Status { get; set; }
}
