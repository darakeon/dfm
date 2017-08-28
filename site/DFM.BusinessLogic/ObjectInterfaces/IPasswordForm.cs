using System;

namespace DFM.BusinessLogic.ObjectInterfaces
{
	public interface IPasswordForm
	{
		String Password { get; set; }
		String RetypePassword { get; set; }

	}
}
