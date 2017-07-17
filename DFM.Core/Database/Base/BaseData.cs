using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ak.DataAccess.NHibernate;
using DFM.Core.Entities.Base;
using NHibernate;
using NHibernate.Criterion;

namespace DFM.Core.Database.Base
{
    public abstract class BaseData<T>
        where T : class, IEntity
    {
        protected static ISession Session
        {
            get { return SessionBuilder.Session; }
        }


        protected static ICriteria CreateSimpleCriteria(Expression<Func<T, Boolean>> expression = null)
        {
            return Session.CreateCriteria<T>().Add(Restrictions.Where(expression));
        }

        protected static T SaveOrUpdate(T entity, DelegateValidade validate, DelegateComplete complete)
        {
            if (complete != null && completeEntity == null) completeEntity += complete;
            if (validate != null && validateEntity == null) validateEntity += validate;

            return saveOrUpdate(entity);
        }

        private static T saveOrUpdate(T entity)
        {
            if (completeEntity != null) completeEntity(entity);
            if (validateEntity != null) validateEntity(entity);

            if (entity.ID == 0 || Session.Contains(entity))
                Session.SaveOrUpdate(entity);
            else
                Session.Merge(entity);

            return entity;
        }

        protected delegate void DelegateValidade(T entity);
        protected delegate void DelegateComplete(T entity);

        private static event DelegateValidade validateEntity;
        private static event DelegateComplete completeEntity;



        internal static void Delete(T obj)
        {
            if (obj != null)
                Session.Delete(obj);
        }


        public static T SelectById(Int32 id)
        {
            return Session.Get<T>(id);
        }



        internal static IList<T> Select()
        {
            return CreateSimpleCriteria().List<T>();
        }




    }
}
