using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Entities.Extensions
{
    public static class MoveExtension
    {
        public static String Month(this Move move)
        {
            return move.Date.ToString("MMMM");
        }




        public static void AddDetail(this Move move, Detail detail)
        {
            move.DetailList.Add(detail);
        }


        public static Boolean HasDetails(this Move move)
        {
            return move.DetailList.Any();
        }




        public static void MakePseudoDetail(this Move move, Double value)
        {
            var id = (move.DetailList.FirstOrDefault() ?? new Detail()).ID;

            move.DetailList = new List<Detail>();

            var detail = new Detail { ID = id, Description = move.Description, Value = value };

            move.AddDetail(detail);
        }



        public static Boolean AuthorizeCRUD(this Move move, User user)
        {
            return move.User() == user;
        }




        public static String GetDescriptionDetailed(this Move move)
        {
            const string boundlessFormat = "{0} [{1}]";
            const string boundedFormat = "{0} [{1}/{2}]";
            var schedule = move.Schedule;

            if (schedule == null || !schedule.ShowInstallment)
                return move.Description;

            
            var total = schedule.Times;
            var executed = schedule.ExecutedMoves();
                
            var format = schedule.Boundless
                             ? boundlessFormat
                             : boundedFormat;

            return String.Format(format, move.Description, executed, total);
        }


    }
}
