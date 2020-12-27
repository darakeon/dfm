using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Account.Models.SubModels
{
	public class Subtotal
	{
		public Subtotal(String language, String key, Decimal value, AccountSign sign)
		{
			Language = language;
			Key = key;
			Value = value;
			Sign = sign;
		}

		public String Language { get; }
		public String Key { get; }
		public Decimal Value { get; }
		public AccountSign Sign { get; }
	}
}
