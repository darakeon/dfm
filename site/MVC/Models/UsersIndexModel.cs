using System.Collections.Generic;
using DFM.MVC.Helpers.Global;
using Version = DFM.Language.Version;

namespace DFM.MVC.Models
{
	public class UsersIndexModel : BaseSiteModel
	{
		public IList<Version> Versions = Translator.Versions();
	}
}
