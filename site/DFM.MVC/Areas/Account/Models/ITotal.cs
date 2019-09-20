using System;

namespace DFM.MVC.Areas.Account.Models
{
	public interface ITotal
	{
		Decimal Total { get; }
		String Language { get; }
	}
}
