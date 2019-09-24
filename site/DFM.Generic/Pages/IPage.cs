using System;

namespace DFM.Generic.Pages
{
	public interface IPage
	{
		String Url { get; }
		String Label { get; }

		IPage Add(Int32 qty);
		Boolean LessThan(IPage other);
		Boolean GreaterThan(IPage other);
		Boolean Same(IPage other);
	}
}
