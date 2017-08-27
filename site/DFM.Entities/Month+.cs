using System;
using System.Collections.Generic;
using System.Linq;

namespace DFM.Entities
{
    public partial class Month
    {
        public virtual User User()
        {
            return Account().User;
        }

        public virtual Account Account()
        {
            return Year.Account;
        }

        public virtual IList<Move> MoveList()
        {
            var list = new List<Move>();

            list.AddRange(OutList);
            list.AddRange(InList);

            return list.OrderBy(m => m.ID).ToList();
        }



        public virtual void AddOut(Move move)
        {
            move.Out = this;
            OutList.Add(move);
        }

        public virtual void AddIn(Move move)
        {
            move.In = this;
            InList.Add(move);
        }



        public virtual void UpdateOut(Move move)
        {
            var oldMove = OutList
                .Single(m => m.ID == move.ID);

            OutList.Remove(oldMove);

            AddOut(move);
        }

        public virtual void UpdateIn(Move move)
        {
            var oldMove = InList
                .Single(m => m.ID == move.ID);

            InList.Remove(oldMove);

            AddIn(move);
        }



        public virtual Boolean OutContains(Move move)
        {
            return OutList
                .Any(m => m.ID == move.ID);
        }

        public virtual Boolean InContains(Move move)
        {
            return InList
                .Any(m => m.ID == move.ID);
        }

    }
}
