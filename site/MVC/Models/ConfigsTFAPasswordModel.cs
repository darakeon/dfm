﻿using System;

namespace DFM.MVC.Models
{
	public class ConfigsTFAPasswordModel : BaseSiteModel
	{
		public String Code { get; set; }

		public void UseAsPassword(Boolean use)
		{
			safe.UseTFAAsPassword(use);
		}
	}
}
