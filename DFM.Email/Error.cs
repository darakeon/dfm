using System;

namespace DFM.Email
{
    public class Error
    {
        public static Boolean Report()
        {
            try
            {
                var body = String.Format("({0}) Look at Elmah, something is going wrong.", DateTime.Now);

                new Sender()
                    .ToDefault()
                    .Subject("Shit!!!")
                    .Body(body)
                    .Send();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
