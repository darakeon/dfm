using System;
using System.Threading.Tasks;

namespace DFM.Logs;

public interface ILogService
{
	Task Log(Exception exception);
	Task LogHandled(Exception exception, String message);
}
