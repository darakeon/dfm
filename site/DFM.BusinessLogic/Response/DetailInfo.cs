using System;
using DFM.Entities;

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

		internal Detail Convert()
		{
			return new Detail
			{
				Description = Description,
				Amount = Amount,
				Value = Value,
			};
		}

		internal static DetailInfo Convert(Detail detail)
		{
			return new DetailInfo
			{
				Description = detail.Description,
				Amount = detail.Amount,
				Value = detail.Value,
			};
		}
	}
}
