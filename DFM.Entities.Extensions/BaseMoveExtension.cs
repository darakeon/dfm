using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Bases;

namespace DFM.Entities.Extensions
{
    public static class BaseMoveExtension
    {
        public static String Month(this BaseMove move)
        {
            return move.Date.ToString("MMMM");
        }




        public static void AddDetail(this BaseMove move, Detail detail)
        {
            move.DetailList.Add(detail);
        }


        public static Boolean HasDetails(this BaseMove move)
        {
            return move.DetailList.Any();
        }

        public static Boolean IsDetailed(this BaseMove move)
        {
            return !move.hasJustOneDetail()
                    || move.hasFirstDetailDescription();
        }

        private static Boolean hasJustOneDetail(this BaseMove move)
        {
            return move.DetailList.Count == 1;
        }

        private static Boolean hasFirstDetailDescription(this BaseMove move)
        {
            var detail = move.DetailList.First();

            return !String.IsNullOrEmpty(detail.Description)
                && detail.Description != move.Description;
        }



        public static void MakePseudoDetail(this BaseMove move, Double value)
        {
            var id = (move.DetailList.FirstOrDefault() ?? new Detail()).ID;

            move.DetailList = new List<Detail>();

            var detail = new Detail { ID = id, Description = move.Description, Value = value };

            move.AddDetail(detail);
        }



        public static Boolean AuthorizeCRUD(this BaseMove move, User user)
        {
            return move.User() == user;
        }



    }
}
