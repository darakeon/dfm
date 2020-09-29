using System;
using DFM.Generic;

namespace DFM.Entities
{
	public partial class Detail
	{
		private void init()
		{
			Amount = 1;
			ExternalId = new Byte[16];
		}

		public override String ToString()
		{
			return $"[{ID}] {Description}";
		}

		public virtual Guid Guid
		{
			get => new Guid(ExternalId);
			set => ExternalId = value.ToByteArray();
		}

		public virtual Decimal Value
		{
			get => ValueCents.ToVisual();
			set => ValueCents = value.ToCents();
		}

		public virtual Decimal GetTotal()
		{
			return Value * Amount;
		}

		public virtual Detail Clone()
		{
			return new Detail
			{
				Description = Description,
				Amount = Amount,
				Value = Value,
				Move = Move,
			};
		}

		public override Boolean Equals(object obj)
		{
			return obj is Detail detail
			       && detail.ID == ID;
		}
	}
}
