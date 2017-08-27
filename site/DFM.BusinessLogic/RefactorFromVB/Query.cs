using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using VB.Generics.Reflection;

namespace VB.DBManager
{
    public class Query<T> where T : class
    {
        private ICriteria criteria;

        public Query(ISession session)
        {
            criteria = session.CreateCriteria<T>();
        }


        public Query<T> FilterSimple(Expression<Func<T, Boolean>> where)
        {
            criteria = criteria.Add(Restrictions.Where(where));

            return this;
        }

        public Query<T> LikeCondition(Expression<Func<T, object>> property, String term)
        {
            var searchTerms = new List<SearchItem<T>>
            {
                new SearchItem<T>(property, term)
            };

            criteria = criteria.Add(accumulateLikeOr(searchTerms));

            return this;
        }

        public Query<T> LikeCondition(IList<SearchItem<T>> searchTerms)
        {
            criteria = criteria.Add(accumulateLikeOr(searchTerms));

            foreach (var searchTerm in searchTerms)
            {
                if (searchTerm.ParentType() != typeof (T))
                {
                    var typeName = searchTerm.ParentType().Name;

                    criteria.CreateAlias(typeName, typeName, CriteriaSpecification.InnerJoin);
                }
            }

            return this;
        }

        private AbstractCriterion accumulateLikeOr(IList<SearchItem<T>> searchTerms)
        {
            if (!searchTerms.Any())
                return null;

            var searchTerm = searchTerms.First();
            var restriction = Restrictions.On(searchTerm.Property).IsLike("%" + searchTerm.Term + "%");

            if (searchTerms.Count() == 1)
            {
                return restriction;
            }

            var otherTerms = searchTerms.Skip(1).ToList();
            var otherProcessedTerms = accumulateLikeOr(otherTerms);

            return Restrictions.Or(restriction, otherProcessedTerms);
        }

        

        public Query<T> OrderBy<TPropOrder>(Expression<Func<T, TPropOrder>> order, Boolean? ascending = true)
        {
            var propName = order.GetName();
            var orderBy = ascending.HasValue && ascending.Value
                ? Order.Asc(propName) : Order.Desc(propName);

            criteria = criteria.AddOrder(orderBy);

            return this;
        }

        public Query<T> Page(Int32? itemsPerPage, Int32? page = 1)
        {
            if (itemsPerPage.HasValue && page != 0)
            {
                var skip = ((page ?? 1) - 1) * itemsPerPage.Value;

                criteria = criteria
                    .SetFirstResult(skip)
                    .SetMaxResults(itemsPerPage.Value);
            }

            return this;
        }



        public Int32 Count
        {
            get
            {
                return criteria
                    .SetProjection(Projections.Count(Projections.Id()))
                    .List<Int32>()
                    .Sum();
            }
        }

        public IList<T> Result
        {
            get { return criteria.List<T>(); }
        }

        public T UniqueResult
        {
            get { return criteria.UniqueResult<T>(); }
        }


    }
}