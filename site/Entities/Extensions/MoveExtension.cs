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

			if (move.Value != 0)
				type += (Int32)MoveValueType.Unique;

			if (move.DetailList.Any())
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
