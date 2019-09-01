using System;
using System.Linq;
using DFM.Entities.Bases;
using DFM.Entities.Enums;

namespace DFM.Entities.Extensions
{
	public static class MoveExtension
	{
		public static MoveValueType ValueType(this IMove move)
		{
			var type = MoveValueType.Empty;

			if (move.Value.HasValue && move.Value != 0)
				type += (Int32)MoveValueType.Unique;

			var details = move.DetailList
				.Sum(d => d.Amount * d.Value);

			if (move.DetailList.Any() && details != 0)
				type += (Int32)MoveValueType.Detailed;

			return type;
		}

		public static Boolean IsDetailed(this IMove move)
		{
			return move.ValueType() == MoveValueType.Detailed;
		}

		public static Boolean HasCategory(this IMove move)
		{
			return move.Category != null;
		}


	}
}
