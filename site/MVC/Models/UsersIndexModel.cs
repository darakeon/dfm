using System.Collections.Generic;
using Version = DFM.Language.Version;

namespace DFM.MVC.Models
{
	public class UsersIndexModel : BaseSiteModel
	{
		public IList<Version> Versions => translator.Versions();
	}
}
