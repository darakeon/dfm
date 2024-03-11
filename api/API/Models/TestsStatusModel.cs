using Keon.NHibernate.Schema;

namespace DFM.API.Models;

public class TestsStatusModel : BaseApiModel
{
	public ApiStatus Status { get; } =
		SessionFactoryManager.Instance == null
			? ApiStatus.DbError
			: ApiStatus.Online;

	public enum ApiStatus
	{
		None,
		Online,
		DbError,
	}
}