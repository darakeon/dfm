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
        protected static ISession Session
        {
            get { return NHManager.Session; }
        }


        protected ICriteria CreateSimpleCriteria(Expression<Func<T, Boolean>> expression = null)
        {
            return Session.CreateCriteria<T>().Add(Restrictions.Where(expression));
        }


        protected delegate void DelegateAction(T entity);
        
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
                if (entity.ID == 0 || Session.Contains(entity))
                    Session.SaveOrUpdate(entity);
                else
                    Session.Merge(entity);
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
            if (entity != null) Session.Evict(entity);
            return entity;
        }



        public void Delete(T obj)
        {
            if (obj != null)
                Session.Delete(obj);
        }


        public T SelectById(Int32 id)
        {
            return Session.Get<T>(id);
        }



        internal IList<T> Select()
        {
            return CreateSimpleCriteria().List<T>();
        }






        public object BeginTransaction()
        {
            return Session.BeginTransaction();
        }


        public void CommitTransaction(object transaction)
        {
            ((ITransaction)transaction).Commit();
        }

    }
}
