using System;
using DFM.Generic;

namespace DFM.Queue;

public class QueueError : SystemError
{
	public QueueError(Object message, Exception? inner = null)
		: base(message.ToString() ?? "queue error", inner)
	{
	}
}
