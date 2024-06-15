using DFM.Generic;

namespace DFM.Queue;

public class QueueError : SystemError
{
	public QueueError(object message, Exception? inner = null)
		: base(message.ToString() ?? "queue error", inner)
	{
	}
}
