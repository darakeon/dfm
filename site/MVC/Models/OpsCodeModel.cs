﻿using System;
using DFM.Email;

namespace DFM.MVC.Models
{
	public class OpsCodeModel : BaseSiteModel
	{
		public Error.Status EmailSent { get; set; }

		protected override Boolean ShowTip => false;
	}
}
