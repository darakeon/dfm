﻿using System;
using DFM.Entities.Enums;
using Keon.Util.DB;

namespace DFM.Entities
{
	public partial class Config : IEntityLong
	{
		public virtual Int64 ID { get; set; }

		public virtual String Language { get; set; }
		public virtual String TimeZone { get; set; }

		public virtual Boolean SendMoveEmail { get; set; }
		public virtual Boolean UseCategories { get; set; }
		public virtual Boolean MoveCheck { get; set; }

		public virtual Theme Theme { get; set; }

		public virtual Boolean Wizard { get; set; }

	}
}
