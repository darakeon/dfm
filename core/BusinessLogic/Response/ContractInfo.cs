using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using Newtonsoft.Json;

namespace DFM.BusinessLogic.Response
{
	public class ContractInfo
	{
		public ContractInfo(Contract contract)
		{
			BeginDate = contract.BeginDate;

			clauses = contract.TermsList
				.ToDictionary(
					t => t.Language.ToLower(),
					t => getClause(t.Json)
				);
		}

		private static Clause getClause(String json)
		{
			return JsonConvert.DeserializeObject<Clause>(json);
		}

		public DateTime BeginDate { get; }
		private IDictionary<String, Clause> clauses { get; }

		public Clause this[String language] =>
			clauses[language.ToLower()];

		public class Clause
		{
			public Clause()
			{
				Items = new List<Clause>();
			}

			public String Text { get; set; }
			public IList<Clause> Items { get; set; }
			public Boolean New { get; set; }
		}
	}
}
