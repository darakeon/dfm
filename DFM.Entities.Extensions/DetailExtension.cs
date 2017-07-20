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
                           FutureMove = detail.FutureMove,
                           Move = detail.Move,
                       };
        }

        public static void SetMove(this Detail detail, BaseMove baseMove)
        {
            if (baseMove is Move)
            {
                detail.Move = (Move) baseMove;
                detail.FutureMove = null;
            }
            else if (baseMove is FutureMove)
            {
                detail.Move = null;
                detail.FutureMove = (FutureMove)baseMove;
            }
            else
            {
                throw new ArgumentOutOfRangeException("baseMove", "Base Move not known");
            }
        }




    }
}
