using System;
using System.Collections.Generic;
using DFM.Entities;

namespace DFM.BusinessLogic.Response;

public class OrderInfo
{
	public virtual DateTime Start { get; set; }
	public virtual DateTime End { get; set; }

	public virtual IList<String> AccountList { get; } = new List<String>();
	public virtual IList<String> CategoryList { get; } = new List<String>();
}
