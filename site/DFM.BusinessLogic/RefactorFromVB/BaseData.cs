using System;
using NHibernate;
using VB.Entities.Base;

namespace VB.DBManager
{
    public class BaseData<T> where T : class, IEntity
    {
        private static ISession session
        {
            get { return NHManager.Session; }
        }

        private static ISession sessionOld
        {
            get { return NHManager.SessionOld; }
        }



        public T SaveOrUpdate(T entity, params BaseRepository<T>.DelegateAction[] actions)
        {
            foreach (var delegateAction in actions)
            {
                delegateAction(entity);
            }

            return saveOrUpdate(entity);
        }

        private static T saveOrUpdate(T entity)
        {
            if (entity.ID == 0 || session.Contains(entity))
                session.SaveOrUpdate(entity);
            else
                session.Merge(entity);

            return entity;
        }

        

        public T GetOldById(Int32 id)
        {
            return sessionOld.Get<T>(id);
        }



        public void Delete(T obj)
        {
            if (obj != null)
                session.Delete(obj);
        }


        public T GetById(Int32 id)
        {
            return session.Get<T>(id);
        }


        public Query<T> NewQuery()
        {
            return new Query<T>(session);
        }


    }
}
