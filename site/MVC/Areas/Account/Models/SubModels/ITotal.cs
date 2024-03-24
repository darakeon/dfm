using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Account.Models.SubModels
{
	public interface ITotal
	{
		Decimal Current { get; }
		AccountSign CurrentSign { get; }
		Decimal? Foreseen { get; }
		AccountSign ForeseenSign { get; }
		String ForeseenClass { get; }
		String Language { get; }
	}
}
