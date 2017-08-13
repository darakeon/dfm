using System;
using System.Linq;
using DFM.Entities.Bases;

namespace DFM.Entities.Extensions
{
    public static class GenericMoveExtension
    {
        public static Boolean IsDetailed<T>(this IMove<T> move)
        {
            return !move.hasJustOneDetail()
                    || move.hasFirstDetailDescription();
        }

        private static Boolean hasJustOneDetail<T>(this IMove<T> move)
        {
            return move.DetailList.Count == 1;
        }

        private static Boolean hasFirstDetailDescription<T>(this IMove<T> move)
        {
            var detail = move.DetailList.First();

            return !String.IsNullOrEmpty(detail.Description)
                && detail.Description != move.Description;
        }



    }
}
