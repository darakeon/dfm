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

		public virtual Double Value
		{
			get { return ValueCents.ToVisual(); }
			set { ValueCents = value.ToCents(); }
		}

		public override String ToString()
		{
			return String.Format("[{0}] {1}", ID, Description);
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
				return ID * Constants.FakeID;
			}
			set
			{
				if (value % Constants.FakeID != 0)
					throw new DFMException("Get back!");

				ID = (Int32)(value / Constants.FakeID);
			}
		}


    }
}
