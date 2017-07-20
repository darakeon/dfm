using System;
using DFM.Entities.Bases;

namespace DFM.Entities.Extensions
{
    public static class DetailExtension
    {
        public static Detail Clone(this Detail detail)
        {
            return new Detail
                       {
                           Description = detail.Description,
                           Amount = detail.Amount,
                           Value = detail.Value,
                       };
        }

        public static void SetMove(this Detail detail, BaseMove baseMove)
        {
            if (baseMove is Move)
                detail.Move = (Move)baseMove;
            else if (baseMove is FutureMove)
                detail.FutureMove = (FutureMove)baseMove;
            else
                throw new ArgumentOutOfRangeException("baseMove", "Base Move not known");
        }




    }
}
