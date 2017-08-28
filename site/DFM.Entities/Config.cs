using System;
using DFM.Entities.Enums;
using DK.Generic.DB;

namespace DFM.Entities
{
	public partial class Config : IEntity
	{
		public virtual Int32 ID { get; set; }

		public virtual String Language { get; set; }
		public virtual String TimeZone { get; set; }
		
		public virtual Boolean SendMoveEmail { get; set; }
		public virtual Boolean UseCategories { get; set; }
		public virtual Boolean MoveCheck { get; set; }

		public virtual BootstrapTheme Theme { get; set; }

	}
}
