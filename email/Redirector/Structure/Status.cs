using System;

namespace Redirector.Structure
{
	public enum Status
	{
		Unknown = 0,

		Pass = 1,
		Gray = 2,

		Fail = -1,
		ProcessingFailed = -2,
	}
}
