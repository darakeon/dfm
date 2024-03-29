﻿using System;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;

namespace DFM.MVC.Areas.Api.Json
{
	internal class SimpleMoveJson
	{
		public Guid Guid { get; set; }

		public String Description { get; set; }

		public Int16 Year { get; set; }
		public Int16 Month { get; set; }
		public Int16 Day { get; set; }

		public MoveNature Nature { get; set; }

		public Boolean Checked { get; set; }

		public Decimal Total { get; set; }

		public SimpleMoveJson(MoveInfo move, String accountUrl)
		{
			Guid = move.Guid;

			Description = move.Description;

			Year = move.Year;
			Month = move.Month;
			Day = move.Day;

			var accountOut = move.Nature != MoveNature.In ? move.OutUrl : null;

			Total = move.Value * (accountUrl == accountOut ? -1 : 1);

			Checked = move.Checked;
		}
	}
}
