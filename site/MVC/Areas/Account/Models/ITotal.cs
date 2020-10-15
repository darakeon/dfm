using System;

namespace DFM.MVC.Areas.Account.Models
{
	public interface ITotal
	{
		Decimal Total { get; }
		Decimal? Foreseen { get; }
		String Language { get; }
	}
}
