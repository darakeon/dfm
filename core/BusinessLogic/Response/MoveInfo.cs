using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Response
{
	public class MoveInfo : IMoveInfo
	{
		public MoveInfo()
		{
			DetailList = new List<DetailInfo>();
		}

		public Guid Guid { get; set; }

		public Boolean Foreseen { get; set; }

		public String Description { get; set; }

		public Int16 Year { get; set; }
		public Int16 Month { get; set; }
		public Int16 Day { get; set; }

		public MoveNature Nature { get; set; }

		public Decimal Value { get; set; }
		public Decimal? Conversion { get; set; }
		public IList<DetailInfo> DetailList { get; set; }

		public Boolean Checked { get; private set; }

		public String OutUrl { get; set; }
		public String OutName { get; set; }
		public String InUrl { get; set; }
		public String InName { get; set; }
		public String CategoryName { get; set; }

		public MoveSchedule Schedule { get; set; }

		internal void Update(Move move)
		{
			move.Description = Description;

			move.Year = Year;
			move.Month = Month;
			move.Day = Day;

			move.Nature = Nature;
			move.Value = Value;
			move.Conversion = Conversion != 0
				? Conversion
				: null;

			move.DetailList = DetailList
				.Select(d => d.Convert())
				.ToList();
		}

		internal static MoveInfo Convert4Edit(Move move)
		{
			var info = convert(move);

			if (info.DetailList.Any())
				info.Value = 0;

			info.Description = move.Description;

			info.Checked = move.CheckedIn || move.CheckedOut;

			return info;
		}

		internal static MoveInfo Convert4Report(Move move, String accountUrl, Boolean foreseen)
		{
			return convert4Report(
				move,
				move.Out?.Url == accountUrl
					? PrimalMoveNature.Out
					: PrimalMoveNature.In,
				foreseen
			);
		}

		internal static MoveInfo Convert4Report(Move move, PrimalMoveNature nature)
		{
			return convert4Report(move, nature);
		}

		internal static MoveInfo Convert4Search(Move move)
		{
			return convert4Report(move);
		}

		private static MoveInfo convert4Report(
			Move move,
			PrimalMoveNature? reportNature = null,
			Boolean foreseen = false
		)
		{
			var info = convert(move);

			if (reportNature.HasValue)
			{
				info.removeConversion(reportNature.Value);
				info.DetailList.ToList()
					.ForEach(d => d.RemoveConversion(reportNature.Value));
			}

			info.Description = move.GetDescriptionWithSchedulePosition();

			if (reportNature.HasValue)
			{
				info.Checked = move.IsChecked(reportNature.Value);
			}

			info.Foreseen = foreseen;

			if (move.Schedule != null)
			{
				info.Schedule = new MoveSchedule(move.Schedule);
			}

			return info;
		}

		private static MoveInfo convert(Move move)
		{
			return new()
			{
				Guid = move.Guid,

				Year = move.Year,
				Month = move.Month,
				Day = move.Day,

				Nature = move.Nature,

				Value = move.Value,
				Conversion = move.Conversion,
				DetailList = move.DetailList
					.Select(DetailInfo.Convert)
					.ToList(),

				OutName = move.Out?.Name,
				OutUrl = move.Out?.Url,
				InName = move.In?.Name,
				InUrl = move.In?.Url,
				CategoryName = move.Category?.Name,
			};
		}

		private void removeConversion(PrimalMoveNature reportNature)
		{
			if (reportNature == PrimalMoveNature.In)
				Value = Conversion != null && Conversion != 0 ? Conversion.Value : Value;

			Conversion = null;
		}
	}
}
