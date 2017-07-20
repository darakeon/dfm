using System;
using DFM.Core.Email;
using DFM.Core.Enums;

namespace DFM.MVC.MultiLanguage.Helpers
{
    public class EmailFormats
    {
        public static Format GetForMove(MoveNature moveNature)
        {
            return Get(String.Format("MoveNotification{0}", moveNature));
        }


        public static Format GetForSecurity(SecurityAction securityAction)
        {
            return Get(securityAction.ToString());
        }


        public static Format Get(String format)
        {
            return new Format
                        {
                            Layout = PlainText.EmailLayout[format],
                            Subject = PlainText.Dictionary[format],
                        };
        }

    }
}