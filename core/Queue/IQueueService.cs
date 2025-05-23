﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFM.Entities;

namespace DFM.Queue
{
	public interface IQueueService
	{
		Task<IDictionary<Line, Boolean>> Enqueue(IList<Line> lineList);
		Task<KeyValuePair<String, Line>?> Dequeue();
		void Delete(String key);
	}
}
