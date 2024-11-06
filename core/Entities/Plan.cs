using System;
using DFM.Generic;
using Keon.Util.DB;

namespace DFM.Entities;

public class Plan : IEntityLong
{
	public virtual Int64 ID { get; set; }

	public virtual String Name { get; set; }
	public virtual Int32 PriceCents { get; set; }

	public virtual Int32 AccountOpened { get; set; }
	public virtual Int32 CategoryEnabled { get; set; }
	public virtual Int32 ScheduleActive { get; set; }
	public virtual Int32 AccountMonthMove { get; set; }
	public virtual Int32 MoveDetail { get; set; }
	public virtual Int32 ArchiveMonthUpload { get; set; }
	public virtual Int32 ArchiveLine { get; set; }
	public virtual Int32 ArchiveSize { get; set; }
	public virtual Int32 OrderMonth { get; set; }
	public virtual Int32 OrderMove { get; set; }

	public virtual Decimal Price
	{
		get => PriceCents.ToVisual();
		set => PriceCents = value.ToCents();
	}

	public override String ToString()
	{
		return $"[{ID}] {Name} R$ {Price:F2}";
	}
}
