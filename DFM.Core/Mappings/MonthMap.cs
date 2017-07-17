using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class MonthMap : IAutoMappingOverride<Month>
    {
        public void Override(AutoMapping<Month> mapping)
        {
            mapping.HasMany(m => m.InList)
                .Cascade.Delete();

            mapping.HasMany(m => m.OutList)
                .Cascade.Delete();

            mapping.References(m => m.Year)
                .Cascade.None();

            mapping.HasMany(m => m.SummaryList)
                .Cascade.SaveUpdate();
        }
    }
}
