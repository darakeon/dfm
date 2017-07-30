using System.Linq;
using DFM.Entities.Bases;

namespace DFM.Entities
{
    public class Move : BaseMove
    {
        public virtual Month In { get; set; }
        public virtual Month Out { get; set; }
        

        
        public override User User()
        {
            return (In ?? Out)
                .Year.Account.User;
        }

        public override Account AccOut()
        {
            return getAccount(Out);
        }

        public override Account AccIn()
        {
            return getAccount(In);
        }

        private static Account getAccount(Month month)
        {
            return month == null ? null : month.Year.Account;
        }



    }
}
