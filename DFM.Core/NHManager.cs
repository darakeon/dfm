using Ak.DataAccess.NHibernate;
using Ak.DataAccess.NHibernate.UserPassed;
using DFM.Entities;
using DFM.Core.Mappings;
using DFM.BusinessLogic;

namespace DFM.Core
{
    public class NHManager
    {
        public static void Start()
        {
            var mapInfo = new AutoMappingInfo<UserMap, User>();

            SessionBuilder.Start(mapInfo);
        }

        public static void Open()
        {
            SessionBuilder.Open();
        }

        public static void Error()
        {
            SessionBuilder.Error();
        }

        public static void Close()
        {
            SessionBuilder.Close();
        }

        public static void End()
        {
            SessionBuilder.End();
        }



    }
}
