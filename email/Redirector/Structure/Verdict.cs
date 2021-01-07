using System;

namespace Redirector.Structure
{
	public struct Verdict
	{
		public String Status { get; set; }
		public Status XStatus => Enum.Parse<Status>(Status, true);
	}
}
