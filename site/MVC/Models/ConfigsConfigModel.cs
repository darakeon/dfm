using System;
using DFM.MVC.Models.Configs;

namespace DFM.MVC.Models
{
	public class ConfigsConfigModel : BaseSiteModel
	{
		public ConfigsConfigModel()
		{
			TFA = new TFAForm(safe, current, translator, errorAlert);
		}

		public TFAForm TFA { get; set; }

		public Form ActiveForm { get; set; }

		public String BackTo { get; set; }

		// ReSharper disable once UnusedMember.Global
		public enum Form
		{
			TFA,
		}
	}
}
