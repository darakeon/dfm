﻿using DFM.Entities.Enums;
using Keon.Util.DB;
using System;
using System.Collections.Generic;

namespace DFM.Entities;

public class Archive : IEntityLong
{
	public virtual Int64 ID { get; set; }
	public virtual Byte[] ExternalId { get; set; }

	public virtual String Filename { get; set; }

	public virtual DateTime Uploaded { get; set; }
	public virtual ImportStatus Status { get; set; }

	public virtual User User { get; set; }

	public virtual IList<Line> LineList { get; set; }

	public virtual Guid Guid
	{
		get => new(ExternalId);
		set => ExternalId = value.ToByteArray();
	}

	public override String ToString()
	{
		return $"[{ID}] {Filename}";
	}
}
