using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.MVC.Helpers.Views
{
	public class WizardHL
	{
		public WizardHL(params Boolean[] conditions)
		{
			this.conditions = conditions;
			boxes = new Dictionary<String, String[]>();
			names = new List<String>();
		}

		private readonly Boolean[] conditions;
		private readonly IDictionary<String, String[]> boxes;
		private readonly IList<String> names;

		public WizardHL AddBox(String name, params Int32?[] hlNumbers)
		{
			var hlNeeded = conditions.Length + 1;
			if (hlNumbers.Length != hlNeeded)
			{
				throw new ArgumentException(
					$"They must be {hlNeeded} variations for {name}",
					nameof(hlNumbers)
				);
			}

			names.Add(name);
			boxes.Add(
				name, hlNumbers.Select(toClass).ToArray()
			);

			return this;
		}

		private static String toClass(Int32? n)
		{
			return n.HasValue
				? $"wizard-highlight-{n.Value}"
				: null;
		}

		public String this[String name]
		{
			get
			{
				if (!names.Contains(name))
					throw new ArgumentException(
						$"No box for {name}",
						nameof(name)
					);

				if (!boxes.ContainsKey(name))
					return null;

				var box = boxes[name];
				boxes.Remove(name);

				for (var c = 0; c < conditions.Length; c++)
				{
					if (conditions[c])
						return box[c];
				}

				return box.Last();
			}
		}
	}
}
