using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Bases
{
    public class BaseRepository<T> where T : IEntity
    {
        private readonly IData<T> repository;

        protected BaseRepository(IData<T> repository)
        {
            this.repository = repository;
        }



        public delegate void DelegateAction(T entity);



        protected T SaveOrUpdate(T entity, params DelegateAction[] actions)
        {
            return repository.SaveOrUpdate(entity, actions);
        }


        internal T GetById(Int32 id)
        {
            return repository.GetById(id);
        }


        internal Boolean Exists(Expression<Func<T, Boolean>> func)
        {
            return repository.Exists(func);
        }


        internal T SingleOrDefault(Expression<Func<T, Boolean>> func)
        {
            return repository.SingleOrDefault(func);
        }


        internal IList<T> List(Expression<Func<T, Boolean>> func)
        {
            return repository.GetWhere(func);
        }

        
        internal void Delete(T entity)
        {
            repository.Delete(entity);
        }

        internal void Delete(Int32 id)
        {
            Delete(GetById(id));
        }


        internal T GetOldById(Int32 id)
        {
            return repository.GetOldById(id);
        }

    }
}
