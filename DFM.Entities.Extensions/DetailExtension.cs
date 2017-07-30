namespace DFM.Entities.Extensions
{
    public static class DetailExtension
    {
        public static Detail Clone(this Detail detail)
        {
            return new Detail
                       {
                           Description = detail.Description,
                           Amount = detail.Amount,
                           Value = detail.Value,
                           Move = detail.Move,
                       };
        }

        public static void SetMove(this Detail detail, Move baseMove)
        {
            detail.Move = (Move) baseMove;
        }




    }
}
