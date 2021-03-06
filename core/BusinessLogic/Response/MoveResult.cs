﻿using System;
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
			Guid = move.Guid;
			CheckIn = move.CheckedIn;
			CheckOut = move.CheckedOut;
			MonthYear = move.ToMonthYear();
		}

		public Guid Guid { get; }
		public EmailStatus Email { get; }
		public Boolean CheckIn { get; }
		public Boolean CheckOut { get; }
		public Int32 MonthYear { get; set; }
	}
}
