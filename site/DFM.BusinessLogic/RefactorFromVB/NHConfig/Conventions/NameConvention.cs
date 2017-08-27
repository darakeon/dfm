using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace VB.DBManager.NHConfig.Conventions
{
    internal class NameConvention
    {
        internal class ManyToMany : ManyToManyTableNameConvention
        {
            protected override String GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
            {
                return String.Format("{0}_{1}", collection.EntityType.Name, otherSide.EntityType.Name);
            }

            protected override String GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
            {
                return String.Format("{0}_{1}", collection.EntityType.Name, collection.ChildType.Name);
            }
        }

        internal class Reference : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                var propertyName = putID(instance.Property.Name);

                instance.Column(propertyName);
            }
        }

        internal class HasMany : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                var propertyName = instance.Member.Name.Replace("List", "");
                var propertyType = instance.Relationship.Class.Name;

                propertyName = 
                    propertyName.ToLower()
                            .Contains(propertyType.ToLower())
                        ? instance.EntityType.Name
                        : propertyName;

                propertyName = putID(propertyName);

                instance.Key.Column(propertyName);
            }
        }
    
        private static String putID(String name)
        {
            return String.Format("{0}_ID", name);
        }
    }
}
