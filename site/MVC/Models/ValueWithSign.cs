using System;

namespace DFM.MVC.Models
{
	public class ValueWithSign
	{
		public ValueWithSign(Decimal value, String language, Boolean differentIfZero = false)
		{
			Value = value;
			Language = language;
			DifferentIfZero = differentIfZero;
		}

		public Decimal Value { get; }
		public String Language { get; }
		public Boolean DifferentIfZero { get; }
	}
}