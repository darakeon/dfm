using System;
using DFM.Email;
using DFM.Entities;

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
			Date = move.Date;
		}

		public Int64 ID { get; }
		public EmailStatus Email { get; }
		public Boolean Check { get; }
		public DateTime Date { get; set; }
	}
}
