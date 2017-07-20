using System;
using DFM.Entities;

namespace DFM.Extensions
{
    public static class DetailExtension
    {
        public static Detail Clone(this Detail detail, Move move)
        {
            return new Detail
                       {
                           Description = detail.Description,
                           Amount = detail.Amount,
                           Value = detail.Value,
                           Move = move,
                       };
        }


        public static Boolean HasDescription(this Detail detail)
        {
            return !String.IsNullOrEmpty(detail.Description)
                && detail.Description != detail.Move.Description;
        }

    }
}
