using System;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.API.Json
{
	internal class SimpleMoveJson
	{
		public Int64 ID { get; set; }

		public String Description { get; set; }
		public DateJson Date { get; set; }
		public MoveNature Nature { get; set; }

		public Boolean Checked { get; set; }

		public Decimal Total { get; set; }

		public SimpleMoveJson(MoveInfo move, String accountUrl)
		{
			ID = move.ID;

			Description = move.Description;
			Date = new DateJson(move.Date);

			var accountOut = move.Nature != MoveNature.In ? move.OutUrl : null;

			Total = move.Total * (accountUrl == accountOut ? -1 : 1);

			Checked = move.Checked;
		}
	}
}
