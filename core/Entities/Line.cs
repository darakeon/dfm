using DFM.Entities.Enums;
using Keon.Util.DB;
using System;

namespace DFM.Entities;

public class Line : IEntityLong
{
	public virtual Int64 ID { get; set; }

	public virtual String Content { get; set; }

	public virtual ImportStatus Status { get; set; }
}
