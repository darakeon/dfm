using System;

namespace Redirector.Structure
{
	public struct Action
	{
		public String Type { get; set; }
		public String FunctionArn { get; set; }
		public String InvocationType { get; set; }
	}
}
