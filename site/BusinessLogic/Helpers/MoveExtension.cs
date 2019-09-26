using System;
using System.Linq;
using DFM.BusinessLogic.Response;
using DFM.Entities.Enums;

namespace DFM.BusinessLogic.Helpers
{
	public static class MoveExtension
	{
		public static MoveValueType ValueType(this IMoveInfo move)
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

		public static Boolean IsDetailed(this IMoveInfo move)
		{
			return move.ValueType() == MoveValueType.Detailed;
		}
	}
}
