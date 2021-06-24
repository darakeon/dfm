using System;
using System.Collections.Generic;

namespace DFM.MVC.Models
{
	public interface ConfigsModel
	{
		String BackTo { get; }
		IList<String> Save();
	}
}