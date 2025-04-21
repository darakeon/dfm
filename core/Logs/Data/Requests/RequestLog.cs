using System;
using Newtonsoft.Json;

namespace DFM.Logs.Data.Requests;

internal class RequestLog(
	String moment,
	String method,
	String path,
	DateTime time
)
{
	public String Moment { get; } = moment;
	public String Method { get; } = method;
	public String Path { get; } = path;
	public DateTime Time { get; } = time;

	public override String ToString()
	{
		return JsonConvert.SerializeObject(
			this,
			Formatting.Indented
		);
	}
}
