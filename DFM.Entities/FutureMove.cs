using DFM.Entities.Bases;

namespace DFM.Entities
{
    public class FutureMove : BaseMove, IMove<Account>
    {
        public virtual Account In { get; set; }
        public virtual Account Out { get; set; }


        public override User User()
        {
            return (In ?? Out).User;
        }

        public override Account AccOut()
        {
            return Out;
        }

        public override Account AccIn()
        {
            return In;
        }

    }
}
