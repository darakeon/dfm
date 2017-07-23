using DFM.BusinessLogic.Helpers;
using DFM.Entities.Bases;
using FluentNHibernate.Automapping;

namespace DFM.Repositories.Mappings
{
    internal class BaseMoveMap
    {
        public static void Override<T>(AutoMapping<T> mapping)
            where T : BaseMove
        {
            mapping.Map(m => m.Description)
                .Length(MaximumLength.MoveDescription);

            mapping.References(m => m.Schedule)
                .Cascade.None()
                .Nullable();

            mapping.References(m => m.Category)
                .Cascade.None();
        }
    }
}
