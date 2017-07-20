using System;
using DFM.Entities;
using DFM.Entities.Extensions;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoveAddDetailModel
    {
        public MoveAddDetailModel()
        {
            Move = new Move();
        }

        public MoveAddDetailModel(Int32 position, Detail detail)
            : this()
        {
            Position = position;

            for (var d = 0; d <= position; d++)
            {
                Move.AddDetail(new Detail());
            }

            Move.DetailList[position] = detail ?? new Detail();
        }

        public Int32 Position { get; set; }
        public Move Move { get; set; }
    }
}