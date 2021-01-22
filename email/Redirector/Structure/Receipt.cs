using System;

namespace Redirector.Structure
{
	public struct Receipt
	{
		public DateTime Timestamp { get; set; }
		public Int16 ProcessingTimeMillis { get; set; }
		public String[] Recipients { get; set; }
		public Verdict SpamVerdict { get; set; }
		public Verdict VirusVerdict { get; set; }
		public Verdict SpfVerdict { get; set; }
		public Verdict DkimVerdict { get; set; }
		public Verdict DmarcVerdict { get; set; }
		public Action Action { get; set; }

		public Boolean IsValid =>
			SpamVerdict.IsValid()
			&& VirusVerdict.IsValid()
			&& SpfVerdict.IsValid()
			&& DkimVerdict.IsValid()
			&& DmarcVerdict.IsValid();
	}
}
