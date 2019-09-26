using System;
using DFM.Email;
using DFM.Entities;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Response
{
	public class MoveResult
	{
		internal MoveResult(Move move, EmailStatus email)
			: this(move)
		{
			Email = email;
		}

		internal MoveResult(Move move)
		{
			ID = move.ID;
			Check = move.Checked;
			MonthYear = move.ToMonthYear();
		}

		public Int64 ID { get; }
		public EmailStatus Email { get; }
		public Boolean Check { get; }
		public Int32 MonthYear { get; set; }
	}
}
