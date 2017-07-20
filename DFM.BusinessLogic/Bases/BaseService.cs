using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Bases
{
    public class BaseService<T> where T : IEntity
    {
        private readonly IRepository<T> repository;

        protected BaseService(IRepository<T> repository)
        {
            this.repository = repository;
        }



        public delegate void DelegateAction(T entity);



        protected T SaveOrUpdate(T entity, params DelegateAction[] actions)
        {
            return repository.SaveOrUpdate(entity, actions);
        }


        internal T SelectById(Int32 id)
        {
            return repository.SelectById(id);
        }
        

        internal T SingleOrDefault(Expression<Func<T, Boolean>> func)
        {
            return repository.SingleOrDefault(func);
        }


        internal IList<T> List(Expression<Func<T, Boolean>> func)
        {
            return repository.List(func);
        }

        
        internal void Delete(T entity)
        {
            repository.Delete(entity);
        }


        protected T SaveOrUpdateInstantly(T entity, params DelegateAction[] actions)
        {
            return repository.SaveOrUpdateInstantly(entity, actions);
        }


        internal T SelectOldById(Int32 id)
        {
            return repository.SelectOldById(id);
        }

    }
}
