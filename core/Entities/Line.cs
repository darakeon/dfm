using DFM.Entities.Enums;
using Keon.Util.DB;
using System;
using System.Collections.Generic;

namespace DFM.Entities;

public class Line : IEntityLong
{
	public Line()
	{
		DetailList = new List<Detail>();
	}

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

	public virtual Boolean HasIn => !String.IsNullOrEmpty(In);
	public virtual Boolean HasOut => !String.IsNullOrEmpty(Out);
	public virtual Boolean HasCategory => !String.IsNullOrEmpty(Category);

	public virtual MoveNature GetNature()
	{
		if (Nature.HasValue)
			return Nature.Value;

		if (!HasIn)
			return MoveNature.Out;

		if (!HasOut)
			return MoveNature.In;

		return MoveNature.Transfer;
	}

	public override String ToString()
	{
		return $"[{ID}] {Position} of {Archive}";
	}
}
