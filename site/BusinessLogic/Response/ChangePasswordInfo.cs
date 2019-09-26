using System;
using DFM.BusinessLogic.InterfacesAndBases;

namespace DFM.BusinessLogic.Response
{
	public class ChangePasswordInfo : IPasswordForm
	{
		public String CurrentPassword { get; set; }
		public String Password { get; set; }
		public String RetypePassword { get; set; }
	}
}
