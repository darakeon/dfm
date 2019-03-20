using DFM.Entities;
using DFM.BusinessLogic.Helpers;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.Repositories.Mappings
{
	public class AccountMap : IAutoMappingOverride<Account>
	{
		public void Override(AutoMapping<Account> mapping)
		{
			mapping.Map(a => a.Name)
				.Length(MaximumLength.Account_Name)
				.UniqueKey("Account_NameUser");

			mapping.Map(a => a.Url)
				.Length(MaximumLength.Account_Url)
				.UniqueKey("Account_UrlUser");

			mapping.Map(a => a.EndDate).Nullable();

			mapping.Map(a => a.RedLimitCents).Nullable();
			mapping.Map(a => a.YellowLimitCents).Nullable();

			mapping.IgnoreProperty(a => a.RedLimit);
			mapping.IgnoreProperty(a => a.YellowLimit);

			mapping.HasMany(a => a.YearList)
				.Cascade.Delete()
				.Inverse();

			mapping.References(a => a.User)
				.UniqueKey("Account_NameUser")
				.UniqueKey("Account_UrlUser");
		}
	}
}
