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
        protected ISession Session
        {
            get { return SessionBuilder.Session; }
        }



        public T SaveOrUpdate(T entity)
        {
            if (Complete != null) Complete(entity);
            if (Validate != null) Validate(entity);

            if (entity.ID == 0 || Session.Contains(entity))
                Session.SaveOrUpdate(entity);
            else
                Session.Merge(entity);

            return entity;
        }

        protected delegate void DelegateValidade(T entity);
        protected delegate void DelegateComplete(T entity);

        protected event DelegateValidade Validate;
        protected event DelegateComplete Complete;



        public void Delete(T obj)
        {
            if (obj != null)
                Session.Delete(obj);
        }



        public T SelectById(Int32 id)
        {
            return Session.Get<T>(id);
        }



        public IList<T> Select()
        {
            return select().List<T>();
        }

        public T SelectSingle(Expression<Func<T, Boolean>> expression)
        {
            return select(expression).UniqueResult<T>();
        }

        // TODO: Replace this shit
        public T SelectOne(Expression<Func<T, Boolean>> preCriteriaExpression, Func<T, Boolean> posCriteriaExpression)
        {
            return Select(preCriteriaExpression).SingleOrDefault(posCriteriaExpression);
        }

        public IList<T> Select(Expression<Func<T, Boolean>> expression)
        {
            return select(expression).List<T>();
        }

        private ICriteria select(Expression<Func<T, Boolean>> expression = null)
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
