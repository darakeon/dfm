using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Account.Models.SubModels
{
	public interface ITotal
	{
		Decimal Total { get; }
		AccountSign TotalSign { get; }
		Decimal? Foreseen { get; }
		AccountSign ForeseenSign { get; }
		String Language { get; }
	}
}
