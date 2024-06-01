namespace DFM.BusinessLogic.Exceptions;

public class ErrorWithMetadata(Error error, object metadata = null)
{
	public Error Error { get; } = error;
	public object Metadata { get; } = metadata;
}
