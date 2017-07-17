using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class TransferMap : IAutoMappingOverride<Transfer>
    {
        public void Override(AutoMapping<Transfer> mapping)
        {
            mapping.References(t => t.In)
                .Cascade.SaveUpdate();

            mapping.References(t => t.Out)
                .Cascade.SaveUpdate();
        }
    }
}
