using System;

namespace DFM.Entities
{
	public class Misc
	{
		internal Misc(Int32 dna)
		{
			DNA = dna;

			var dnaBinary = Convert.ToString(dna, 2)
				.PadLeft(9, '0')
				.Substring(0, 9);

			var r = dnaBinary.Substring(0, 1);
			var g = dnaBinary.Substring(3, 1);
			var b = dnaBinary.Substring(6, 1);
			Color = r + g + b;

			Antenna = dnaBinary.Substring(1, 2);
			Eye = dnaBinary.Substring(4, 2);
			Arm = dnaBinary.Substring(7, 1);
			Leg = dnaBinary.Substring(8, 1);
		}

		public Int32 DNA { get; }

		public String Color { get; }

		public String Antenna { get; }
		public String Eye { get; }
		public String Arm { get; }
		public String Leg { get; }

		public static Int32 RandomDNA() => new Random().Next(0, 511);
		public static Misc Random() => new(RandomDNA());
	}
}
