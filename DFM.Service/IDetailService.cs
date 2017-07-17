using System;
using DFM.Service.Entities;

namespace DFM.Service
{
    public interface IDetailService
    {
        Detail SaveOrUpdate(Detail entity);
        Detail SelectById(Int32 id);
    }
}
