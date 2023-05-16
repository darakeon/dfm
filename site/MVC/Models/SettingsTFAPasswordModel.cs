using System;

namespace DFM.MVC.Models
{
	public class SettingsTFAPasswordModel : BaseSiteModel
	{
		public String Code { get; set; }

		public void UseAsPassword(Boolean use)
		{
			auth.UseTFAAsPassword(use);
		}
	}
}
