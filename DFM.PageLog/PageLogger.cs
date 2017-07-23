using System.Web;

namespace DFM.PageLog
{
    public class PageLogger
    {
        public static void Record(HttpContext context)
        {
            var request = new Request(context);

            Logger.Record(request);
        }

    }
}
