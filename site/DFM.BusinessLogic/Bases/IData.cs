using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Bases
{
    public interface IData<T> where T : IEntity
    {
        T SaveOrUpdate(T entity, params BaseRepository<T>.DelegateAction[] actions);
        T GetById(Int32 id);
        Boolean Exists(Expression<Func<T, Boolean>> func);
        T SingleOrDefault(Expression<Func<T, Boolean>> func);
        IList<T> GetWhere(Expression<Func<T, Boolean>> func);
        void Delete(T entity);
        T GetOldById(Int32 id);
    }
}