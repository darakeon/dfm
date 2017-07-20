using System;
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




        //Remove doesn't work because they aren't same object
        public void RemoveFromIn()
        {
            In.InList =
                In.InList
                    .Where(m => m.ID != ID)
                    .ToList();
        }

        //Remove doesn't work because they aren't same object
        public void RemoveFromOut()
        {
            Out.OutList =
                Out.OutList
                    .Where(m => m.ID != ID)
                    .ToList();
        }



    }
}
