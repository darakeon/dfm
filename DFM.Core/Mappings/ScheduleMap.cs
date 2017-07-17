using DFM.Core.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Core.Mappings
{
    public class ScheduleMap : IAutoMappingOverride<Schedule>
    {
        public void Override(AutoMapping<Schedule> mapping)
        {
            mapping.Map(s => s.Active)
                .Default("1");

            mapping.HasMany(d => d.MoveList)
                .Cascade.None();

            mapping.References(d => d.User)
                .Cascade.None();
        }
    }
}
