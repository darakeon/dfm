using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class MoveMap : IAutoMappingOverride<Move>
    {
        public void Override(AutoMapping<Move> mapping)
        {
            mapping.HasMany(m => m.DetailList)
                .Cascade.AllDeleteOrphan();


            mapping.References(m => m.In)
                .Cascade.SaveUpdate()
                .Nullable();

            mapping.References(m => m.Out)
                .Cascade.SaveUpdate()
                .Nullable();

            mapping.References(m => m.Schedule)
                .Cascade.None()
                .Nullable();
        }
    }
}
