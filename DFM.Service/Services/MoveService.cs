using System;
using DFM.Core.Database;
using DFM.Core.Entities;
using DFM.Core.Helpers;
using Account = DFM.Service.Entities.Account;
using Move = DFM.Service.Entities.Move;

namespace DFM.Service.Services
{
    public class MoveService : IMoveService
    {
        public Move SaveOrUpdate(Move move, Account account, Account secondAccount)
        {
            return (Move)MoveData.SaveOrUpdate(move, account, secondAccount);
        }

        public void Delete(Move move)
        {
            MoveData.Delete(move);
        }

        public void Schedule(Move move, Account account, Account secondAccount, Schedule schedule)
        {
            MoveData.Schedule(move, account, secondAccount, schedule);
        }

        public Move SelectById(Int32 id)
        {
            return (Move)MoveData.SelectById(id);
        }
    }
}