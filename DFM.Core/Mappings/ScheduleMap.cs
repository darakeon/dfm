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

            mapping.References(s => s.User)
                .Cascade.None();

            mapping.HasMany(s => s.MoveList)
                .Inverse();
        }
    }
}
