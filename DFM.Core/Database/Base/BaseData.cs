using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ak.DataAccess.NHibernate;
using DFM.Core.Entities.Base;
using DFM.Core.Helpers;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace DFM.Core.Database.Base
{
    public abstract class BaseData<T>
        where T : class, IEntity
    {
        protected static ISession Session
        {
            get { return SessionBuilder.Session; }
        }


        protected static T SaveOrUpdate(T entity, DelegateValidade validate, DelegateComplete complete)
        {
            if (complete != null) completeEntity += complete;
            if (validate != null) validateEntity += validate;

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
            return select().List<T>();
        }

        internal static T SelectSingle(Expression<Func<T, Boolean>> expression)
        {
            return select(expression).UniqueResult<T>();
        }

        // TODO: Replace this shit
        internal static T SelectSingle(Expression<Func<T, Boolean>> preCriteriaExpression, Func<T, Boolean> posCriteriaExpression)
        {
            return Select(preCriteriaExpression).SingleOrDefault(posCriteriaExpression);
        }

        internal static IList<T> Select(Expression<Func<T, Boolean>> expression)
        {
            return select(expression).List<T>();
        }

        private static ICriteria select(Expression<Func<T, Boolean>> expression = null)
        {
            var criteria = Session.CreateCriteria(typeof (T));

            foreach (var entity in Relations<T>.GetDeeper(0))
            {
                criteria.CreateAlias(entity, entity, JoinType.LeftOuterJoin);
            }

            if (expression != null)
                criteria.Add(Restrictions.Where(expression));

            return criteria;
        }




    }
}
