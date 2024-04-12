using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.BusinessLogic.Response
{
	public class SearchResult
	{
		public SearchResult()
			: this(new List<Move>()) { }

		public SearchResult(IList<Move> moveList)
		{
			MoveList = moveList
				.Select(MoveInfo.Convert4Search)
				.ToList();
		}

		public IList<MoveInfo> MoveList { get; set; }
	}
}
