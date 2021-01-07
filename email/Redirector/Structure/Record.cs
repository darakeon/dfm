using System;

namespace Redirector.Structure
{
	public struct Record
	{
		public String EventSource { get; set; }
		public String EventVersion { get; set; }
		public SES SES { get; set; }
	}
}
