using System;
using DFM.BusinessLogic.Response;

namespace DFM.MVC.Models
{
	public class SettingsTFAPasswordModel : BaseSiteModel
	{
		public TFACheck TFA { get; set; }

		public void UseAsPassword(Boolean use)
		{
			auth.UseTFAAsPassword(use, TFA);
		}
	}
}
