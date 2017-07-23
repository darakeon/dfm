using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class FutureMoveMap : IAutoMappingOverride<FutureMove>
    {
        public void Override(AutoMapping<FutureMove> mapping)
        {
            BaseMoveMap.Override(mapping);

            mapping.References(m => m.In)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Out)
                .Cascade.None()
                .Nullable();

            mapping.HasMany(m => m.DetailList)
                .Cascade.None()
                .Not.LazyLoad();

        }
    }
}
