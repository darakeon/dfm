using System;

namespace DFM.Core.Entities.Extensions
{
    public static class DetailExtension
    {
        internal static Detail Clone(this Detail detail, Move move)
        {
            return new Detail
                       {
                           Description = detail.Description,
                           Amount = detail.Amount,
                           Value = detail.Value,
                           Move = move,
                       };
        }


        internal static Boolean HasDescription(this Detail detail)
        {
            return !String.IsNullOrEmpty(detail.Description)
                && detail.Description != detail.Move.Description;
        }

    }
}
