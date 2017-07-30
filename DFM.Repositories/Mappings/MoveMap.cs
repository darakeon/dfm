using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class MoveMap : IAutoMappingOverride<Move>
    {
        public void Override(AutoMapping<Move> mapping)
        {
            mapping.Map(m => m.Description)
                .Length(MaximumLength.MoveDescription);

            mapping.References(m => m.Schedule)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Category)
                .Cascade.None();

            mapping.References(m => m.In)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Out)
                .Cascade.None()
                .Nullable();

            mapping.HasMany(m => m.DetailList)
                .Cascade.Delete()
                .Not.LazyLoad();

        }
    }
}
