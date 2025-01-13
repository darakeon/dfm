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
		None = 0,
		Online = 1,
		DbError = 2,
		Maintenance = 3,
	}
}