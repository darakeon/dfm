using System;
using DFM.Entities.Enums;

namespace DFM.Email
{
    public class Format
    {
        public String Subject { get; set; }
        public String Layout { get; set; }

        
        public Format(MoveNature moveNature)
        {
            throw new NotImplementedException();
        }

        //public static Format GetForMove(String section, String language, MoveNature moveNature)
        //{
        //    return Get(section, language, String.Format("MoveNotification{0}", moveNature));
        //}

        public Format(SecurityAction securityAction)
        {
            throw new NotImplementedException();
        }

        //public static Format GetForSecurity(String section, String language, SecurityAction securityAction)
        //{
        //    return Get(section, language, securityAction.ToString());
        //}

        //public static Format Get(String section, String language, String format)
        //{
        //    return new Format
        //    {
        //        Layout = PlainText.EmailLayout[language, format],
        //        Subject = PlainText.Dictionary[section, language, format],
        //    };
        //}

        







    }
}
