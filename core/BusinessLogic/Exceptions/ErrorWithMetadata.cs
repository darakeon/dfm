using System;
using Newtonsoft.Json;

namespace DFM.BusinessLogic.Exceptions;

public class ErrorWithMetadata(Error error, Object metadata = null)
{
	public Error Error { get; } = error;
	public Object Metadata { get; } = metadata;

	public override String ToString()
	{
		var metadata = JsonConvert.SerializeObject(
			Metadata, Formatting.Indented
		);

		return $"{Error}: {metadata}";
	}
}
