using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class MoveInfo : IMoveInfo
	{
		public MoveInfo()
		{
			DetailList = new List<DetailInfo>();
		}

		public Int64 ID { get; set; }

		public String Description { get; set; }

		public Int16 Year { get; set; }
		public Int16 Month { get; set; }
		public Int16 Day { get; set; }

		public MoveNature Nature { get; set; }

		public Decimal Total { get; private set; }

		public Decimal? Value { get; set; }
		public IList<DetailInfo> DetailList { get; set; }

		public Boolean Checked { get; private set; }

		public String OutUrl { get; set; }
		public String InUrl { get; set; }
		public String CategoryName { get; set; }

		internal void Update(Move move)
		{
			move.Description = Description;

			move.Year = Year;
			move.Month = Month;
			move.Day = Day;

			move.Nature = Nature;
			move.Value = Value;

			move.DetailList = DetailList
				.Select(d => d.Convert())
				.ToList();
		}

		internal static MoveInfo Convert4Edit(Move move)
		{
			var info = convert(move, true);

			info.Checked = move.CheckedIn || move.CheckedOut;

			return convert(move, false);
		}

		internal static MoveInfo Convert4Report(Move move, String accountUrl)
		{
			var info = convert(move, true);

			var isOut = info.OutUrl == accountUrl;
			var nature = isOut
				? PrimalMoveNature.Out
				: PrimalMoveNature.In;
			info.Checked = move.IsChecked(nature);

			return info;
		}

		private static MoveInfo convert(Move move, Boolean isReport)
		{
			return new MoveInfo
			{
				ID = move.ID,
				Description = isReport
					? move.GetDescriptionWithSchedulePosition()
					: move.Description,

				Year = move.Year,
				Month = move.Month,
				Day = move.Day,

				Nature = move.Nature,

				Total = move.Total(),

				Value = move.Value,
				DetailList = move.DetailList
					.Select(DetailInfo.Convert)
					.ToList(),

				OutUrl = move.Out?.Url,
				InUrl = move.In?.Url,
				CategoryName = move.Category?.Name,
			};
		}
	}
}
