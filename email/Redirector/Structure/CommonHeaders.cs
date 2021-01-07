using System;

namespace Redirector.Structure
{
	public struct CommonHeaders
	{
		public String ReturnPath { get; set; }
		public String[] From { get; set; }
		public String Date { get; set; }
		public DateTime XDate => DateTime.Parse(Date);
		public String[] To { get; set; }
		public String MessageId { get; set; }
		public String Subject { get; set; }
	}
}
