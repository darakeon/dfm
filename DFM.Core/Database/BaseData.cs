using System.Collections.Generic;
using Ak.DataAccess.NHibernate;
using DFM.Core.Entities;
using NHibernate;

namespace DFM.Core.Database
{
    public abstract class BaseData<T>
        where T : IEntity
    {
        protected ISession Session
        {
            get { return SessionBuilder.Session; }
        }

        public T SaveOrUpdate(T entity)
        {
            if (Complete != null) Complete(entity);
            if (Validate != null) Validate(entity);

            if (entity.ID == 0)
                Session.SaveOrUpdate(entity);
            else
                Session.Merge(entity);

            return entity;
        }

        protected delegate void DelegateValidade(T entity);
        protected delegate void DelegateComplete(T entity);

        protected event DelegateValidade Validate;
        protected event DelegateComplete Complete;



        public virtual void Delete(T obj)
        {
            if (obj != null)
                Session.Delete(obj);
        }

        public virtual T SelectById(int id)
        {
            return Session.Get<T>(id);
        }

        public virtual IList<T> Select()
        {
            return Session
                .CreateCriteria(typeof(T))
                .List<T>();
        }
    }
}
