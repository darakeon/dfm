using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using DFM.Generic;

namespace DFM.Entities
{
    public partial class User
    {
		private void init()
		{
			Config = new Config();
			AccountList = new List<Account>();
			CategoryList = new List<Category>();
			ScheduleList = new List<Schedule>();
			SecurityList = new List<Security>();
		}
		public override String ToString()
		{
			return String.Format("[{0}] {1}", ID, Email);
		}
		
		
		public virtual Boolean IsAdm()
        {
            return Email == "{admin-email}";
        }

        public virtual DateTime Now()
        {
            return DateTimeGMT.Now(Config.TimeZone);
        }

        public virtual Boolean HasPendentActivation()
        {
            return SecurityList
                .Any(s => s.Action == SecurityAction.UserVerification
                     && s.Expire >= Now()
                     && s.Active);
        }

        public virtual IList<Category> VisibleCategoryList()
        {
            return CategoryList
                .Where(c => c.Active)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public virtual IList<Account> VisibleAccountList()
        {
            return AccountList
                .Where(c => c.IsOpen())
                .OrderBy(a => a.Name)
                .ToList();
        }

        public virtual Boolean WrongPassExceeded()
        {
            return WrongLogin >= Cfg.PasswordErrorLimit;
        }



    }
}
