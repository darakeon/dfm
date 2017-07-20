using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DFM.Entities.Bases;

namespace DFM.BusinessLogic.Services
{
    public class BaseService<T>
        where T: IEntity
    {
        private readonly IRepository repository;

        internal BaseService(IRepository repository)
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


        internal T SaveOrUpdateInstantly(T entity, params DelegateAction[] actions)
        {
            return repository.SaveOrUpdateInstantly(entity, actions);
        }


        internal T SelectOldById(Int32 id)
        {
            return repository.SelectOldById(id);
        }



        public interface IRepository
        {
            T SaveOrUpdate(T entity, params DelegateAction[] actions);
            T SelectById(Int32 id);
            T SingleOrDefault(Expression<Func<T, Boolean>> func);
            IList<T> List(Expression<Func<T, Boolean>> func);
            void Delete(T entity);
            T SaveOrUpdateInstantly(T entity, params DelegateAction[] actions);
            T SelectOldById(Int32 id);
        }


    }

}
