using System;
using System.Collections.Generic;

namespace DFM.MVC.Models
{
	public interface SettingsModel
	{
		String BackTo { get; }
		IList<String> Save();
	}
}