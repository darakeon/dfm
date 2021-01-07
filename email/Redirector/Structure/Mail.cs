using System;

namespace Redirector.Structure
{
	public struct Mail
	{
		public DateTime Timestamp { get; set; }
		public String Source { get; set; }
		public String MessageId { get; set; }
		public String[] Destination { get; set; }
		public Boolean HeadersTruncated { get; set; }
		public Header[] Headers { get; set; }
		public CommonHeaders CommonHeaders { get; set; }
	}
}
