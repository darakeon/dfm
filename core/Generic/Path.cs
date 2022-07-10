using System;
using System.Runtime.CompilerServices;

namespace DFM.Generic
{
	public class FilePath
	{
		public static String Get(
			[CallerFilePath] String path = null
		) => path;
	}
}
