using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class SummaryMap : IAutoMappingOverride<Summary>
    {
        public void Override(AutoMapping<Summary> mapping)
        {
            mapping.References(s => s.Month)
                .Cascade.SaveUpdate()
                .Nullable();

            mapping.References(s => s.Year)
                .Cascade.SaveUpdate()
                .Nullable();

            mapping.Map(s => s.IsValid)
                .Default("1");

            mapping.IgnoreProperty(s => s.SafeValue);
        }
    }
}
