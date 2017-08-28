using System;
using DFM.Entities;

namespace DFM.MVC.Models
{
	public class UsersContractModel : BaseLoggedModel
	{
		public UsersContractModel()
		{
			Contract = Safe.GetContract();
		}

		public Contract Contract { get; private set; }
		public Boolean Accept { get; set; }


		public void AcceptContract(Action<String, String> addModelError)
		{
			if (!Accept)
				addModelError("Accept", String.Empty);
			else
				Safe.AcceptContract();
		}


	}
}