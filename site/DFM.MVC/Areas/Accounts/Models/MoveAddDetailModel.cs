using System;
using DFM.Entities;
using DFM.MVC.Models;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoveAddDetailModel : BaseModel
    {
        public MoveAddDetailModel()
        {
            Move = new Move();
        }

        public MoveAddDetailModel(Int32 position)
            : this()
        {
            Position = position;

            for (var d = 0; d <= position; d++)
            {
                Move.AddDetail(new Detail());
            }
        }

        public MoveAddDetailModel(Detail detail, Int32 position)
            : this(position)
        {
            Move.DetailList[position] = detail;
        }

        public Int32 Position { get; set; }
        public Move Move { get; set; }


    }
}