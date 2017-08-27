using System;
using System.Linq.Expressions;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using VB.Generics.Reflection;

namespace VB.DBManager.NHConfig.AutoMapping
{
    ///<summary>
    /// Extensions to relation 1:1
    ///</summary>
    public static class AutoMappingOne2One
    {
        ///<summary>
        /// Used on weak entity of 1:1.
        /// Use HasOne on the strong entity mapping.
        ///</summary>
        ///<param name="mapping">Object received in AutoMappingOverride</param>
        ///<param name="memberExpression">Lambda of entity that is the wrong on relation</param>
        public static IdentityPart IsWeakEntity<T, TFather>(this AutoMapping<T> mapping, Expression<Func<T, TFather>> memberExpression)
        {
            var motherEntityName = memberExpression.GetName();

            return mapping.Id()
                .GeneratedBy.Foreign(motherEntityName);
        }

    }
}
