using System;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class DetailInfo
	{
		public DetailInfo()
		{
			Amount = 1;
		}

		public String Description { get; set; }
		public Int16 Amount { get; set; }
		public Decimal Value { get; set; }
		public Decimal? Conversion { get; set; }

		internal Detail Convert()
		{
			return new()
			{
				Description = Description,
				Amount = Amount,
				Value = Value,
				Conversion = Conversion != 0
					? Conversion
					: null,
			};
		}

		internal static DetailInfo Convert(Detail detail)
		{
			return new()
			{
				Description = detail.Description,
				Amount = detail.Amount,
				Value = detail.Value,
				Conversion = detail.Conversion,
			};
		}

		public void RemoveConversion(PrimalMoveNature reportNature)
		{
			if (reportNature == PrimalMoveNature.In)
				Value = Conversion != null && Conversion != 0 ? Conversion.Value : Value;

			Conversion = null;
		}
	}
}
