using System;
using System.Linq;

namespace DFM.Entities.Extensions
{
    public static class FutureMoveExtension
    {
        public static FutureMove CloneChangingDate(this FutureMove futureMove, DateTime dateTime)
        {
            return new FutureMove
            {
                Category = futureMove.Category,
                Date = dateTime,
                Description = futureMove.Description,
                In = futureMove.In,
                Out = futureMove.Out,
                Nature = futureMove.Nature,
                Schedule = futureMove.Schedule,
                DetailList = futureMove.DetailList
                                .Select(d => d.Clone())
                                .ToList(),
            };
        }


        public static Move CastToKill(this FutureMove futureMove)
        {
            var schedule = futureMove.Schedule;
            var category = futureMove.Category;
            var detailList = futureMove.DetailList;

            futureMove.Schedule = null;
            futureMove.Category = null;
            futureMove.DetailList = null;

            futureMove.In = null;
            futureMove.Out = null;

            var move = futureMove.CastToChild<Move>();

            //In case of NH try to save,
            //it causes issues on being null
            futureMove.Category = category;

            move.Schedule = schedule;
            move.Category = category;
            move.DetailList = detailList;

            move.ID = 0;

            return move;
        }


    }
}
