using System;
using DFM.Entities.Enums;

namespace DFM.MVC.Models
{
	public class ValueWithSign
	{
		public ValueWithSign(Decimal value, MoveNature nature, String language)
			: this(value, nature, language, false) { }

		public ValueWithSign(Decimal value, String language)
			: this(value, null, language, false) { }

		public ValueWithSign(Decimal value, String language, Boolean differentIfZero)
			: this(value, null, language, differentIfZero) { }

		private ValueWithSign(Decimal value, MoveNature? nature, String language, Boolean differentIfZero)
		{
			Value = value;
			Nature = nature;
			Language = language;
			DifferentIfZero = differentIfZero;
		}

		public Decimal Value { get; }
		public MoveNature? Nature { get; }
		public String Language { get; }
		public Boolean DifferentIfZero { get; }
	}
}
