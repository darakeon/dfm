using DFM.Entities;

namespace DFM.MVC.Areas.Accounts.Models
{
    public class MoveCreateEditModel : MoveCreateEditScheduleModel<Move>
    {
        public MoveCreateEditModel(Move move)
            : base(move) { }

        public MoveCreateEditModel() { }

    }
}