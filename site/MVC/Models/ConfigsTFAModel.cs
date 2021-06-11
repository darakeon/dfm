using System;
using DFM.BusinessLogic.Exceptions;

namespace DFM.MVC.Models
{
	public class ConfigsTFAModel : BaseSiteModel
	{
		public String Code { get; set; }

		public void UseAsPassword(Boolean use)
		{
			safe.UseTFAAsPassword(use);
		}
	}
}
