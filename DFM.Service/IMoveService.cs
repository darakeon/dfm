using System;
using DFM.Core.Entities;
using DFM.Core.Helpers;
using Account = DFM.Service.Entities.Account;
using Move = DFM.Service.Entities.Move;

namespace DFM.Service
{
    public interface IMoveService
    {
        Move SaveOrUpdate(Move move, Account account, Account secondAccount = null);
        void Delete(Move move);
        void Schedule(Move move, Account account, Account secondAccount, Schedule schedule);
        Move SelectById(Int32 id);
    }
}
