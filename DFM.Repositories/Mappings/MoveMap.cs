using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class MoveMap : IAutoMappingOverride<Move>
    {
        public void Override(AutoMapping<Move> mapping)
        {
            BaseMoveMap.Override(mapping);

            mapping.References(m => m.In)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Out)
                .Cascade.None()
                .Nullable();

            mapping.HasMany(m => m.DetailList)
                .Cascade.Delete()
                .Not.LazyLoad();

        }
    }
}
