using System;
using DFM.Generic;
using Keon.Util.DB;

namespace DFM.Entities
{
	public class Detail : IEntityLong
	{
		public Detail()
		{
			Amount = 1;
			ExternalId = new Byte[16];
		}

		public virtual Int64 ID { get; set; }
		public virtual Byte[] ExternalId { get; set; }

		public virtual String Description { get; set; }
		public virtual Int16 Amount { get; set; }
		public virtual Int32 ValueCents { get; set; }
		public virtual Int32? ConversionCents { get; set; }

		public virtual Move Move { get; set; }
		public virtual Schedule Schedule { get; set; }
		public virtual Line Line { get; set; }

		public virtual Guid Guid
		{
			get => new(ExternalId);
			set => ExternalId = value.ToByteArray();
		}

		public virtual Decimal Value
		{
			get => ValueCents.ToVisual();
			set => ValueCents = value.ToCents();
		}

		public virtual Decimal? Conversion
		{
			get => ConversionCents.ToVisual();
			set => ConversionCents = value.ToCents();
		}

		public virtual Decimal GetTotalValue()
		{
			return Value * Amount;
		}

		public virtual Decimal? GetTotalConversion()
		{
			return Conversion * Amount;
		}

		public virtual Detail Clone()
		{
			return new()
			{
				Description = Description,
				Amount = Amount,
				Value = Value,
				Conversion = Conversion,
				Move = Move,
			};
		}

		public override Boolean Equals(Object obj)
		{
			return obj is Detail detail
				&& detail.ID == ID;
		}

		public override Int32 GetHashCode()
		{
			// ReSharper disable once NonReadonlyMemberInGetHashCode
			return ID.GetHashCode();
		}

		public override String ToString()
		{
			return $"[{ID}] {Description}";
		}
	}
}
