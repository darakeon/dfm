using System;
using DFM.Core.Database;
using DFM.Service.Entities;
using DFM.Core.Helpers;

namespace DFM.Service.Services
{
    public class MoveService : IMoveService
    {
        private readonly MoveData data = new MoveData();

        public Move SaveOrUpdate(Move move, Account account, Account secondAccount)
        {
            return (Move)data.SaveOrUpdate(move, account, secondAccount);
        }

        public void Delete(Move move)
        {
            data.Delete(move);
        }

        public void Schedule(Move move, Account account, Account secondAccount, Scheduler scheduler)
        {
            data.Schedule(move, account, secondAccount, scheduler);
        }

        public Move SelectById(Int32 id)
        {
            return (Move)data.SelectById(id);
        }
    }
}