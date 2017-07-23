using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
    public class DetailMap : IAutoMappingOverride<Detail>
    {
        public void Override(AutoMapping<Detail> mapping)
        {
            mapping.Map(d => d.Description)
                .Length(MaximumLength.DetailDescription);

            mapping.Map(d => d.Amount)
                .Default("1");


            mapping.References(d => d.Move)
                .Nullable();

            mapping.References(d => d.FutureMove)
                .Nullable();

        }
    }
}
