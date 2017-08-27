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

        public MoveAddDetailModel(Int32 position, Int32 id)
            : this()
        {
            var detail = id == 0
                ? default(Detail)
                : Money.GetDetailById(id);

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