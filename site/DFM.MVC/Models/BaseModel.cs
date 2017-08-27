using System;
using DFM.Authentication;
using DFM.BusinessLogic;
using DFM.BusinessLogic.Services;
using DFM.Entities.Extensions;
using DFM.Repositories;

namespace DFM.MVC.Models
{
    public class BaseModel
    {
        private static ServiceAccess access;

        private static ServiceAccess getOrCreateAccess()
        {
            return access
                ?? (access = new ServiceAccess(new Connector()));
        }

        internal static AdminService Admin { get { return getOrCreateAccess().Admin; } }
        internal static MoneyService Money { get { return getOrCreateAccess().Money; } }
        internal static ReportService Report { get { return getOrCreateAccess().Report; } }
        internal static RobotService Robot { get { return getOrCreateAccess().Robot; } }
        internal static SafeService Safe { get { return getOrCreateAccess().Safe; } }

        internal static Current Current { get { return getOrCreateAccess().Current; } }

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