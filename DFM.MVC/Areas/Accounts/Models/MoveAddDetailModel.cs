using System;
using DFM.Core.Entities;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoveAddDetailModel
    {
        public MoveAddDetailModel()
        {
            Move = new Move();
        }

        public MoveAddDetailModel(Int32 position, Int32 id, String description, Double value)
            : this()
        {
            Position = position;

            for (var d = 0; d <= position; d++)
            {
                Move.AddDetail(new Detail());
            }

            Move.DetailList[position].ID = id;
            Move.DetailList[position].Description = description;
            Move.DetailList[position].Value = value;
        }

        public Int32 Position { get; set; }
        public Move Move { get; set; }
    }
}