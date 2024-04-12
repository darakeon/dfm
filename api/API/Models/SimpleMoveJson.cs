using System;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;

namespace DFM.API.Models
{
	internal class SimpleMoveJson
	{
		public Guid Guid { get; set; }

		public string Description { get; set; }

		public short Year { get; set; }
		public short Month { get; set; }
		public short Day { get; set; }

		public MoveNature Nature { get; set; }

		public bool Checked { get; set; }

		public decimal Total { get; set; }

		public SimpleMoveJson(MoveInfo move, string accountUrl)
		{
			Guid = move.Guid;

			Description = move.Description;

			Year = move.Year;
			Month = move.Month;
			Day = move.Day;

			Nature = move.Nature;

			var accountOut = move.Nature != MoveNature.In ? move.OutUrl : null;

			Total = move.Value * (accountUrl == accountOut ? -1 : 1);

			Checked = move.Checked;
		}
	}
}
