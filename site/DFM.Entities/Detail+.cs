using System;
using DFM.Generic;

namespace DFM.Entities
{
	public partial class Detail
	{
		private void init()
		{
			Amount = 1;
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

		public override String ToString()
		{
			return $"[{ID}] {Description}";
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
	}
}
