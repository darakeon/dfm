using System;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Areas.API.Json
{
	public class DetailJson
	{
		public DetailJson() { }

		public DetailJson(DetailInfo detail) : this()
		{
			Description = detail.Description;
			Amount = detail.Amount;
			Value = detail.Value;
		}

		public String Description { get; set; }
		public Int16 Amount { get; set; }
		public Decimal Value { get; set; }

		public DetailInfo ConvertToEntity()
		{
			return new DetailInfo
			{
				Amount = Amount,
				Value = Value,
				Description = Description
			};
		}
	}
}
