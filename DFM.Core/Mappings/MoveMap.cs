using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class MoveMap : IAutoMappingOverride<Move>
    {
        public void Override(AutoMapping<Move> mapping)
        {
            mapping.References(m => m.Transfer)
                .Cascade.SaveUpdate()
                .Nullable();

            mapping.IgnoreProperty(m => m.Month);
            mapping.IgnoreProperty(m => m.Value);

            mapping.HasMany(m => m.DetailList)
                .Cascade.AllDeleteOrphan();
        }
    }
}
