using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DFM.Generic
{
	public class Rewrite
	{
		private class Dic : Dictionary<String, List<String>> { }
		private readonly Dic values;

		public Rewrite(String path)
		{
			var json = File.ReadAllText(path);
			values = JsonConvert.DeserializeObject<Dic>(json);
		}

		public delegate void Each(String origin, String destiny);
		public void ForEach(Each action)
		{
			values
				.Select(r =>
					r.Value.Select(v => new
					{
						origin = v,
						destiny = r.Key
					})
				)
				.SelectMany(r => r)
				.ToList()
				.ForEach(r => action(r.origin, r.destiny));
		}
	}
}
