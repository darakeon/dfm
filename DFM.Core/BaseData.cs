using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ak.DataAccess.NHibernate;
using DFM.BusinessLogic.Services;
using DFM.Entities.Bases;
using DFM.BusinessLogic.Exceptions;
using NHibernate;
using NHibernate.Criterion;

namespace DFM.Core
{
    public class BaseData<T> : BaseService<T>.IRepository where T : class, IEntity
    {
        protected static ISession Session
        {
            get { return SessionBuilder.Session; }
        }


        protected ICriteria CreateSimpleCriteria(Expression<Func<T, Boolean>> expression = null)
        {
            return Session.CreateCriteria<T>().Add(Restrictions.Where(expression));
        }

        public T SaveOrUpdateInstantly(T entity, params BaseService<T>.DelegateAction[] actions)
        {
            var transac = Session.BeginTransaction();
            entity = SaveOrUpdate(entity, actions);
            transac.Commit();

            return entity;
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





        protected delegate void DelegateAction(T entity);


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






    }
}
