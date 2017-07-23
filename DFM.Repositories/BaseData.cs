using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DFM.BusinessLogic.Bases;
using DFM.Entities.Bases;
using DFM.BusinessLogic.Exceptions;
using NHibernate;
using NHibernate.Criterion;

namespace DFM.Repositories
{
    public class BaseData<T> : IRepository<T> where T : class, IEntity
    {
        private static ISession session
        {
            get { return NHManager.Session; }
        }


        protected ICriteria CreateSimpleCriteria(Expression<Func<T, Boolean>> expression = null)
        {
            return session.CreateCriteria<T>().Add(Restrictions.Where(expression));
        }


        public T SaveOrUpdate(T entity, params BaseService<T>.DelegateAction[] actions)
        {
            foreach (var delegateAction in actions)
            {
                delegateAction(entity);
            }

            return saveOrUpdate(entity);
        }

        private static T saveOrUpdate(T entity)
        {
            try
            {
                if (entity.ID == 0 || session.Contains(entity))
                    session.SaveOrUpdate(entity);
                else
                    session.Merge(entity);
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.Message.StartsWith("Data too long for column"))
                    throw DFMCoreException.WithMessage(ExceptionPossibilities.TooLargeData);
                throw;
            }

            return entity;
        }



        public T SingleOrDefault(Expression<Func<T, Boolean>> func)
        {
            var criteria = CreateSimpleCriteria(func);

            return criteria.UniqueResult<T>();

        }

        public IList<T> List(Expression<Func<T, Boolean>> func)
        {
            var criteria = CreateSimpleCriteria(func);

            return criteria.List<T>();
        }

        public T SelectOldById(Int32 id)
        {
            var entity = SelectById(id);
            
            if (entity != null)
            {
                session.Evict(entity);
                entity = SelectById(id);
                session.Evict(entity);
            }

            return entity;
        }



        public void Delete(T obj)
        {
            if (obj != null)
                session.Delete(obj);
        }


        public T SelectById(Int32 id)
        {
            return session.Get<T>(id);
        }



        internal IList<T> Select()
        {
            return CreateSimpleCriteria().List<T>();
        }






        

    }
}
