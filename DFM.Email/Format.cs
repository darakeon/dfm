using System;
using DFM.Entities.Enums;

namespace DFM.Email
{
    public class Format
    {
        public String Subject { get; set; }
        public String Layout { get; set; }

        public delegate Format GetterForMove(MoveNature moveNature);
        public delegate Format GetterForSecurity(SecurityAction securityAction);
    }
}
