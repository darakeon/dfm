using System.Web;
using DFM.Authentication;

namespace DFM.PageLog
{
    public class PageLogger
    {
        public static void Record(HttpContext context, ISafeService safeService)
        {
            var request = new Request(context, safeService);

            Logger.Record(request);
        }

    }
}
