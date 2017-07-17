using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class DetailMap : IAutoMappingOverride<Detail>
    {
        public void Override(AutoMapping<Detail> mapping)
        {
            mapping.References(d => d.Move)
                .Cascade.SaveUpdate();

            mapping.Map(d => d.Amount)
                .Default("1");
        }
    }
}
