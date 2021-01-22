using System;

namespace Redirector.Structure
{
	public struct Verdict
	{
		public String Status { get; set; }

		public Status XStatus =>
			Status == null
				? Structure.Status.Unknown
				: Enum.Parse<Status>(Status.Replace("_", ""), true);

		public Boolean IsValid()
		{
			return XStatus >= Structure.Status.Unknown;
		}
	}
}
