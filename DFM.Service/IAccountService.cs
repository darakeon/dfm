using System;
using System.Collections.Generic;
using System.ServiceModel;
using DFM.Service.Entities;

namespace DFM.Service
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        IList<Move> GetMonthReport(Int32 id, Int32 dateMonth, Int32 dateYear);
        Year GetYearReport(Int32 id, Int32 year);
        void Close(Account account);
        Account SaveOrUpdate(Account entity);
        Account SelectById(Int32 id);
    }

}
