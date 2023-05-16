using System;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models
{
	public class UsersContractModel : BaseSiteModel
	{
		public UsersContractModel()
		{
			Contract = law.GetContract();
		}

		public ContractInfo Contract { get; }
		public Boolean Accept { get; set; }

		public void AcceptContract(Action<String, String> addModelError)
		{
			if (!Accept)
				addModelError("Accept", String.Empty);
			else
				law.AcceptContract();
		}
	}
}
