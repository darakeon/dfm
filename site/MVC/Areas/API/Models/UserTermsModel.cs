using System;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Areas.Api.Models;

public class UserTermsModel : BaseApiModel
{
	public UserTermsModel()
	{
		var contract = law.GetContract();
		Content = contract[language];
		date = contract.BeginDate;
	}

	public ContractInfo.Clause Content { get; }

	public Int32 Year { get; private set; }
	public Int32 Month { get; private set; }
	public Int32 Day { get; private set; }

	private DateTime date
	{
		set
		{
			Year = value.Year;
			Month = value.Month;
			Day = value.Day;
		}
	}
}
