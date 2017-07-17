using Ak.DataAccess.NHibernate;
using DFM.Core.Entities;
using DFM.Core.Mappings;
using NHibernate;

namespace DFM.Core.Database
{
    public class NHManager
    {
        public static ISession Session { get; private set; }

        public static void Initialize()
        {
            var mapInfo = new AutoMappingInfo<UserMap, User>();

            SessionBuilder.Initialize(mapInfo);
        }

        public static void Open()
        {
            SessionBuilder.Open();

            Session = SessionBuilder.Session;
        }

        public static void Close()
        {
            SessionBuilder.Close();
        }

        public static void NhInitialize(object obj)
        {
            SessionBuilder.NhInitialize(obj);
        }

        public static void End()
        {
            SessionBuilder.End();
        }
    }
}