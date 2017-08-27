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

        public static AdminService Admin { get { return getOrCreateAccess().Admin; } }
        public static MoneyService Money { get { return getOrCreateAccess().Money; } }
        public static ReportService Report { get { return getOrCreateAccess().Report; } }
        public static RobotService Robot { get { return getOrCreateAccess().Robot; } }
        public static SafeService Safe { get { return getOrCreateAccess().Safe; } }

        public static Current Current { get { return getOrCreateAccess().Current; } }

        public DateTime Today
        {
            get { return Current.User.Now().Date; }
        }

    }
}