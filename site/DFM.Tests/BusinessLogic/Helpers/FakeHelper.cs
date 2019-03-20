using System;
using System.Collections.Generic;
using DFM.BusinessLogic.Helpers;
using DK.NHibernate.UserPassed;
using fieldSizes = System.Collections.Generic.Dictionary<
	System.String, 
	System.Collections.Generic.IDictionary<
		System.String, System.Int16
	>
>;

namespace DFM.Tests.BusinessLogic.Helpers
{
	public class FakeHelper
	{
		public static fieldSizes FieldSizes = getSizes();

		public static Boolean IsFake => DK.NHibernate.Helpers.FakeHelper.IsFake;

		private static fieldSizes getSizes()
		{
			var sizes = new fieldSizes();

			var lengths = typeof(MaximumLength).GetFields();

			foreach (var length in lengths)
			{
				var nameParts = length.Name.Split('_');
				var entity = nameParts[0];
				var field = nameParts[1];
				var value = (Int16) length.GetValue(null);

				if (!sizes.ContainsKey(entity))
					sizes.Add(entity, new Dictionary<String, Int16>());

				sizes[entity].Add(field, value);
			}

			return sizes;
		}
	}
}