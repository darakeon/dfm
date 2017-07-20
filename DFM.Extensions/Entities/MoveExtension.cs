using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities;

namespace DFM.Extensions.Entities
{
    public static class MoveExtension
    {
        public static String Month(this Move move)
        {
            return move.Date.ToString("MMMM");
        }


        public static Account AccountIn(this Move move)
        {
            return getAccount(move.In);
        }

        public static Account AccountOut(this Move move)
        {
            return getAccount(move.Out);
        }

        private static Account getAccount(Month month)
        {
            return month == null ? null : month.Year.Account;
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


        public static void MakePseudoDetail(this Move move, Double value)
        {
            var id = (move.DetailList.FirstOrDefault() ?? new Detail()).ID;

            move.DetailList = new List<Detail>();

            var detail = new Detail { ID = id, Description = move.Description, Value = value };

            move.AddDetail(detail);
        }


        public static Move Clone(this Move move)
        {
            var newMove = new Move
                           {
                               Description = move.Description,
                               Date = move.Date,
                               Nature = move.Nature,

                               Category = move.Category,

                               In = move.In,
                               Out = move.Out,
                               Schedule = move.Schedule,
                           };

            move.DetailList.ToList().ForEach(d => 
                newMove.AddDetail(d.Clone(newMove)));

            return newMove;
        }

        public static Boolean AuthorizeCRUD(this Move move, User user)
        {
            return (move.In ?? move.Out).Year.Account.User == user;
        }


        //Remove doesn't work because they aren't same object
        public static void RemoveFromIn(this Move move)
        {
            move.In.InList =
                move.In.InList
                    .Where(m => m.ID != move.ID)
                    .ToList();
        }

        //Remove doesn't work because they aren't same object
        public static void RemoveFromOut(this Move move)
        {
            move.Out.OutList =
                move.Out.OutList
                    .Where(m => m.ID != move.ID)
                    .ToList();
        }



        public static User User(this Move move)
        {
            return (move.In ?? move.Out)
                .Year.Account.User;
        }

    }
}
