using System;
using System.Linq;

namespace DFM.Entities.Extensions
{
    public static class FutureMoveExtension
    {
        public static FutureMove GetNext(this FutureMove futureMove, DateTime dateTime)
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


        public static Move Cast(this FutureMove futureMove)
        {
            var schedule = futureMove.Schedule;
            futureMove.Schedule = null;

            var move = futureMove.CastToChild<Move>();

            futureMove.Schedule = schedule;

            move.Schedule = schedule;
            move.ID = 0;

            foreach (var detail in move.DetailList)
            {
                detail.SetMove(move);
            }

            return move;
        }


    }
}
