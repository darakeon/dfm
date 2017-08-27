namespace DFM.Entities
{
    public partial class Detail
    {
        public virtual Detail Clone()
        {
            return new Detail
                       {
                           Description = Description,
                           Amount = Amount,
                           Value = Value,
                           Move = Move,
                       };
        }

        public virtual void SetMove(Move baseMove)
        {
            Move = baseMove;
        }




    }
}
