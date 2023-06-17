using System;
using DFM.Generic;

namespace DFM.Entities.Bases;

public interface ITalkable
{
	public Theme Theme { get; set; }
	public String Language { get; set; }
}
