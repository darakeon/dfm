using System;
using System.Threading.Tasks;
using DFM.Logs.Data;

namespace DFM.Logs;

public abstract class BaseLogService : ILogService
{
	public async Task Log(Exception exception)
	{
		var error = new ErrorLog(exception);
		await saveLog(Division.Exception, error);
	}

	public async Task LogHandled(Exception exception, String message)
	{
		var error = new ErrorLog(exception, message);
		await saveLog(Division.Exception, error);
	}

	private protected abstract Task saveLog(Division division, Object content);
}
