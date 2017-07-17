using System;
using DFM.Core.Database;
using DFM.Service.Entities;

namespace DFM.Service.Services
{
    class DetailService : IDetailService
    {
        private readonly DetailData data = new DetailData();

        public Detail SaveOrUpdate(Detail detail)
        {
            return (Detail)data.SaveOrUpdate(detail);
        }

        public Detail SelectById(Int32 id)
        {
            return (Detail)data.SelectById(id);
        }
    }
}