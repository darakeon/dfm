using System;
using DFM.Generic;

namespace DFM.Entities
{
	public partial class Summary
	{
		public virtual Decimal In
		{
			get => InCents.ToVisual();
			set => InCents = value.ToCents();
		}

		public virtual Decimal Out
		{
			get => OutCents.ToVisual();
			set => OutCents = value.ToCents();
		}


		public override String ToString()
		{
			return $"[{ID}] {In - Out}";
		}

		public virtual Decimal Value()
		{
			return Math.Round(In - Out, 2);
		}

		public virtual String UniqueID()
		{
			var accountID = Account.ID;

			var category = Category;
			var categoryID = category?.ID ?? 0;

			return $"{accountID}_{categoryID}";
		}

		public virtual User User()
		{
			return Account.User;
		}
	}
}
