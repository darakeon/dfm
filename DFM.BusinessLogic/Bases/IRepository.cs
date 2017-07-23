using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Bases
{
    public interface IRepository<T> where T : IEntity
    {
        T SaveOrUpdate(T entity, params BaseService<T>.DelegateAction[] actions);
        T SelectById(Int32 id);
        T SingleOrDefault(Expression<Func<T, Boolean>> func);
        IList<T> List(Expression<Func<T, Boolean>> func);
        void Delete(T entity);
        T SelectOldById(Int32 id);

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}