using System;
using DFM.Core.Database;
using DFM.Service.Entities;

namespace DFM.Service.Services
{
    class DetailService : IDetailService
    {
        public Detail SaveOrUpdate(Detail detail)
        {
            return (Detail)DetailData.SaveOrUpdate(detail);
        }

        public Detail SelectById(Int32 id)
        {
            return (Detail)DetailData.SelectById(id);
        }
    }
}