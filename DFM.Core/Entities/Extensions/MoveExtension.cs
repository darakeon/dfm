using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace DFM.Core.Entities.Extensions
{
    public static class MoveExtension
    {
        internal static String Month(this Move move)
        {
            return move.Date.ToString("MMMM");
        }


        public static Double Value(this Move move)
        {
            return move.DetailList.Sum(d => d.Value * d.Amount);
        }


        internal static Boolean Show(this Move move)
        {
            return move.Date <= DateTime.Now;
        }


        internal static Account AccountIn(this Move move)
        {
            return move.In.Year.Account;
        }


        internal static Account AccountOut(this Move move)
        {
            return move.Out.Year.Account;
        }


        public static void AddDetail(this Move move, Detail detail)
        {
            move.DetailList.Add(detail);
            detail.Move = move;
        }


        public static Boolean HasRealDetails(this Move move)
        {
            return move.DetailList.Any()
                && (
                    move.DetailList.Count > 1
                    || move.DetailList[0].HasDescription()
                );
        }


        internal static void MakePseudoDetail(this Move move, Double value)
        {
            var id = (move.DetailList.FirstOrDefault() ?? new Detail()).ID;

            move.DetailList = new List<Detail>();

            var detail = new Detail { ID = id, Description = move.Description, Value = value };

            move.AddDetail(detail);
        }


        internal static Move Clone(this Move move)
        {
            var newMove = new Move
                           {
                               Description = move.Description,
                               Date = move.Date,
                               Nature = move.Nature,

                               Category = move.Category,

                               In = move.In,
                               Out = move.Out,
                           };

            move.DetailList.ForEach(d => 
                newMove.AddDetail(d.Clone(newMove)));

            return newMove;
        }

    }
}
