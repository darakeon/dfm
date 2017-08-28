using System;
using DFM.Entities.Bases;
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
			get { return ValueCents.ToVisual(); }
			set { ValueCents = value.ToCents(); }
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

		public virtual void SetMove(Move baseMove)
		{
			Move = baseMove;
		}

		public virtual Int64 FakeID
		{
			get
			{
				return ID * Constants.FAKE_ID;
			}
			set
			{
				if (value % Constants.FAKE_ID != 0)
					throw new DFMException("Get back!");

				ID = (Int32)(value / Constants.FAKE_ID);
			}
		}


	}
}
