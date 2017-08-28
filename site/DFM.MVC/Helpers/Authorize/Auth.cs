using DFM.Authentication;
using DFM.BusinessLogic;

namespace DFM.MVC.Helpers.Authorize
{
	public class Auth
	{
		private static readonly ServiceAccess access = new ServiceAccess();

		public static readonly Current Current = new Current(access.Safe);

	}

}