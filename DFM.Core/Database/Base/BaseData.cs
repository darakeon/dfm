using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ak.DataAccess.NHibernate;
using DFM.Core.Entities.Bases;
using DFM.Core.Exceptions;
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

        protected static T SaveOrUpdate(T entity, DelegateComplete complete, DelegateValidade validate)
        {
            if (complete != null) complete(entity);
            if (validate != null) validate(entity);

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

        protected delegate void DelegateValidade(T entity);
        protected delegate void DelegateComplete(T entity);


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
