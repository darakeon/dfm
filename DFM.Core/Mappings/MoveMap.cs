using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class MoveMap : IAutoMappingOverride<Move>
    {
        public void Override(AutoMapping<Move> mapping)
        {
            mapping.IgnoreProperty(m => m.Month);
            mapping.IgnoreProperty(m => m.Value);

            mapping.IgnoreProperty(m => m.Show);
            
            mapping.IgnoreProperty(m => m.AccountIn);
            mapping.IgnoreProperty(m => m.AccountOut);


            mapping.HasMany(m => m.DetailList)
                .Cascade.AllDeleteOrphan()
                .Inverse();

            mapping.References(m => m.In)
                .Nullable();

            mapping.References(m => m.Out)
                .Nullable();
        }
    }
}
