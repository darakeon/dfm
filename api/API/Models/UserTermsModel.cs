using System;
using DFM.BusinessLogic.Response;

namespace DFM.API.Models;

public class UserTermsModel : BaseApiModel
{
	public UserTermsModel()
	{
		var contract = law.GetContract();
		if (contract == null) return;

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
