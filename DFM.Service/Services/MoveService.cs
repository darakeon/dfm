using System;
using DFM.Core.Database;
using DFM.Service.Entities;
using DFM.Core.Helpers;

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

        public void Schedule(Move move, Account account, Account secondAccount, Scheduler scheduler)
        {
            MoveData.Schedule(move, account, secondAccount, scheduler);
        }

        public Move SelectById(Int32 id)
        {
            return (Move)MoveData.SelectById(id);
        }
    }
}