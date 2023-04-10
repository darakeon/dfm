using System;
using System.Linq;

namespace DFM.Entities
{
	public class Misc
	{
		internal Misc(Int32 dna, Boolean colors = false)
		{
			DNA = dna;

			var dnaBinary = Convert.ToString(dna - 1, 2)
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

			Colors = colors;

			if (colors)
			{
				Background = Color.Replace("0", "6").Replace("1", "3");
				Border = Color.Replace("1", "F");
			}
			// 000, 100, 010, 001
			else if (Color.Count(c => c == '1') < 2)
			{
				Background = "666";
				Border = "000";
			}
			// 111, 011, 101, 110
			else
			{
				Background = "333";
				Border = "FFF";
			}
		}

		public Int32 DNA { get; }

		public String Color { get; }

		public String Antenna { get; }
		public String Eye { get; }
		public String Arm { get; }
		public String Leg { get; }

		public String Background { get; }
		public String Border { get; }

		public Boolean Colors { get; }

		public static Int32 RandomDNA() => new Random().Next(1, 512);
		public static Misc Random() => new(RandomDNA());
		public static Misc Empty() => new(0);

		public override Boolean Equals(Object other)
		{
			return other is Misc otherMisc
			    && otherMisc.DNA == DNA;
		}

		public override Int32 GetHashCode()
		{
			return DNA;
		}

		public override String ToString()
		{
			return $"C{Color}.A{Antenna}.E{Eye}.H{Arm}.F{Leg}";
		}
	}
}
