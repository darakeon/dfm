using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VB.Entities.Base;

namespace VB.DBManager
{
    public class BaseRepository<T>
        where T : class, IEntity
    {
        protected readonly BaseData<T> repository;

        public BaseRepository()
        {
            repository = new BaseData<T>();
        }


        public delegate void DelegateAction(T entity);



        public T SaveOrUpdate(T entity, params DelegateAction[] actions)
        {
            return repository.SaveOrUpdate(entity, actions);
        }



        public T Get(Int32 id)
        {
            return repository.GetById(id);
        }

        public T GetOld(Int32 id)
        {
            return repository.GetOldById(id);
        }



        protected internal Boolean Any(Expression<Func<T, Boolean>> func)
        {
            return repository.NewQuery().FilterSimple(func).Count > 0;
        }


        public T SingleOrDefault(Expression<Func<T, Boolean>> func)
        {
            return repository.NewQuery().FilterSimple(func).UniqueResult;
        }




        public void Delete(T entity)
        {
            repository.Delete(entity);
        }

        public void Delete(Int32 id)
        {
            Delete(Get(id));
        }



        protected internal T GetOldById(Int32 id)
        {
            return repository.GetOldById(id);
        }




        public Query<T> NewQuery()
        {
            return repository.NewQuery();
        }

        public IList<T> GetAll()
        {
            return repository.NewQuery().Result;
        }

        public IList<T> SimpleFilter(Expression<Func<T, bool>> condition)
        {
            return repository.NewQuery().FilterSimple(condition).Result;
        }

        public Int32 Count()
        {
            return repository.NewQuery().Count;
        }

        public Int32 Count(Expression<Func<T, Boolean>> func)
        {
            return repository.NewQuery().FilterSimple(func).Count;
        }




    }
}
