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
        [OperationContract]
        Year GetYearReport(Int32 id, Int32 year);
        [OperationContract]
        void Close(Account account);
        [OperationContract]
        void Delete(Account account);
        [OperationContract]
        Account SaveOrUpdate(Account entity);
        [OperationContract]
        Account SelectById(Int32 id);
    }

}
