using System;
using DFM.Service.Entities;
using DFM.Core.Helpers;

namespace DFM.Service
{
    public interface IMoveService
    {
        Move SaveOrUpdate(Move move, Account account, Account secondAccount = null);
        void Delete(Move move);
        void Schedule(Move move, Account account, Account secondAccount, Scheduler scheduler);
        Move SelectById(Int32 id);
    }
}
