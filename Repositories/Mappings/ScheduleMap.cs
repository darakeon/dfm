using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class ScheduleMap : IAutoMappingOverride<Schedule>
    {
        public void Override(AutoMapping<Schedule> mapping)
        {
            mapping.Not.LazyLoad();

            mapping.Map(s => s.Active)
                .Default("1");

            mapping.References(s => s.User)
                .Cascade.None();

            mapping.HasMany(s => s.MoveList)
                .Cascade.None();
        }
    }
}
