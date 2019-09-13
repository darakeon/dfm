using System;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class UsersContractModel : BaseSiteModel
	{
		public UsersContractModel()
		{
			Contract = safe.GetContract();
		}

		public Contract Contract { get; }
		public Boolean Accept { get; set; }

		public Clause Clauses => Contract[Language].GetClauses();

		public void AcceptContract(Action<String, String> addModelError)
		{
			if (!Accept)
				addModelError("Accept", String.Empty);
			else
				safe.AcceptContract();
		}


	}
}
