using DFM.BusinessLogic.Helpers;
using DFM.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DFM.BusinessLogic.Repositories.Mappings
{
	public class AccountMap : IAutoMappingOverride<Account>
	{
		public void Override(AutoMapping<Account> mapping)
		{
			mapping.Map(a => a.Name)
				.Length(MaxLen.Account_Name)
				.UniqueKey("Account_NameUser");

			mapping.Map(a => a.Url)
				.Length(MaxLen.Account_Url)
				.UniqueKey("Account_UrlUser");

			mapping.Map(a => a.EndDate).Nullable();

			mapping.Map(a => a.RedLimitCents).Nullable();
			mapping.Map(a => a.YellowLimitCents).Nullable();

			mapping.IgnoreProperty(a => a.RedLimit);
			mapping.IgnoreProperty(a => a.YellowLimit);

			mapping.References(a => a.User)
				.UniqueKey("Account_NameUser")
				.UniqueKey("Account_UrlUser");
		}
	}
}
