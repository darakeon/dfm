using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFM.Core.Entities;
using NHibernate;

namespace DFM.Core.Database
{
    public abstract class BaseData<T>
        where T : IEntity
    {
        protected ISession Session
        {
            get
            {
                return NHManager.Session;
            }
        }

        public virtual T SaveOrUpdate(T obj)
        {
            if (obj.ID == 0)
                Session.SaveOrUpdate(obj);
            else
                Session.Merge(obj);

            return obj;
        }

        public virtual void Delete(T obj)
        {
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
