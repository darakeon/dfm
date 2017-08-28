using System;
using DFM.Entities;

namespace DFM.MVC.Areas.API.Jsons
{
	public class DetailJson
	{
		public DetailJson() { }

		public DetailJson(Detail detail) : this()
		{
			Description = detail.Description;
			Amount = detail.Amount;
			Value = detail.Value;
		}

		public String Description { get; set; }
		public Int16 Amount { get; set; }
		public Decimal Value { get; set; }

		public Detail ConvertToEntity()
		{
			return new Detail
			{
				Amount = Amount,
				Value = Value,
				Description = Description
			};
		}
	}
}