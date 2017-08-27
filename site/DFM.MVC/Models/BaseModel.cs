using System;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Services;
using DFM.Entities.Extensions;

namespace DFM.MVC.Models
{
    public class BaseModel
    {
        private static ServiceAccess access;

        private static ServiceAccess getOrCreateAccess()
        {
            return access ?? (access = new ServiceAccess());
        }

        protected static AdminService Admin { get { return getOrCreateAccess().Admin; } }
        protected static MoneyService Money { get { return getOrCreateAccess().Money; } }
        protected static ReportService Report { get { return getOrCreateAccess().Report; } }
        protected static RobotService Robot { get { return getOrCreateAccess().Robot; } }
        protected static SafeService Safe { get { return getOrCreateAccess().Safe; } }

        protected static Current Current { get { return getOrCreateAccess().Current; } }

        public DateTime Today
        {
            get
            {
                return Current.User == null
                    ? DateTime.UtcNow
                    : Current.User.Now().Date;
            }
        }
    }
}