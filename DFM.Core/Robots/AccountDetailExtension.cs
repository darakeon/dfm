using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Entities.Bases;
using DFM.Core.Entities.Extensions;
using DFM.Core.Enums;

namespace DFM.Core.Robots
{
    public static class FixRateRunner
    {
        public static void Run(User user)
        {
            var accountDetailList = AccountData.GetRatesToApply(user);

            foreach (var accountDetail in accountDetailList)
            {
                CreateMove(accountDetail);
            }
        }

        public static void CreateMove(IAccountDetail accountDetail)
        {
            var move = new Move
            {
                Category = accountDetail.Category,
                Date = accountDetail.Next,
                Description = accountDetail.Description,
                Nature = MoveNature.Out,
            };

            move.MakePseudoDetail(accountDetail.Value);

            MoveData.SaveOrUpdate(move, accountDetail.Account);


            accountDetail.Next = 
                accountDetail.Frequency.Next(accountDetail.Next);

            AccountData.SaveOrUpdate(accountDetail.Account);
        }

    }
}
