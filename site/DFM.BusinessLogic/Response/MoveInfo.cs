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

		public Int64 ID { get; set; }
		public String OutUrl { get; set; }
		public String InUrl { get; set; }
		public String CategoryName { get; set; }

		public String Description { get; set; }
		public DateTime Date { get; set; }

		public MoveNature Nature { get; set; }

		public Decimal Total { get; private set; }

		public Decimal? Value { get; set; }
		public IList<DetailInfo> DetailList { get; set; }

		public Boolean Checked { get; private set; }

		internal void Update(Move move)
		{
			move.Description = Description;
			move.Date = Date;
			move.Nature = Nature;
			move.Value = Value;

			move.DetailList = DetailList
				.Select(d => d.Convert())
				.ToList();
		}

		internal static MoveInfo Convert(Move move)
		{
			return new MoveInfo
			{
				ID = move.ID,
				Description = move.Description,
				Date = move.Date,
				Nature = move.Nature,
				Total = move.Total(),
				Value = move.Value,
				DetailList = move.DetailList
					.Select(DetailInfo.Convert)
					.ToList(),
				Checked = move.Checked,
				OutUrl = move.Out?.Url,
				InUrl = move.In?.Url,
				CategoryName = move.Category?.Name,
			};
		}
	}
}
