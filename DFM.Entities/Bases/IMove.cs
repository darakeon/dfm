namespace DFM.Entities.Bases
{
    public interface IMove<T>
    {
        T In { get; set; }
        T Out { get; set; }
    }
}
