using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DFM.Generic;
using DFM.Generic.Datetime;
using Keon.Util.Exceptions;

namespace DFM.Authentication;

class CacheAndRetry<T>(String ticketKey, T defaultValue, Func<String, T> getFromDb)
{
	private static readonly IDictionary<String, T> items =
		new Dictionary<String, T>();

	private readonly Random random = new();

	public T Get()
	{
		return get(0);
	}

	private T get(Int32 count)
	{
		var key = ticketKey;

		if (String.IsNullOrEmpty(key))
			return defaultValue;

		if (count == 10)
			return defaultValue;

		var now = nowKey();
		clearDeadSessions(now);

		var dicKey = key + "_" + now;

		lock (items)
		{
			if (items.ContainsKey(dicKey))
				return items[dicKey];

			try
			{
				var value = getFromDb(key);
				if (value != null)
					items.Add(dicKey, value);
				return value;
			}
			catch (SystemError)
			{
				return defaultValue;
			}
			catch (DKException)
			{
				return defaultValue;
			}
			catch
			{
				// random to not all the threads try the same time again
				var milliseconds = random.Next(1000);

				Thread.Sleep(milliseconds);

				return get(++count);
			}
		}
	}

	private static Int64 nowKey()
	{
		var text = DateTime.UtcNow.UntilMillisecond();
		var factor = Int64.Parse(text);
		return factor / Cfg.MillisecondsToCache;
	}

	private void clearDeadSessions(Int64 now)
	{
		items
			.Select(s => s.Key)
			.Where(key => !key.EndsWith(now.ToString()))
			.ToList()
			.ForEach(key => items.Remove(key));
	}
}
