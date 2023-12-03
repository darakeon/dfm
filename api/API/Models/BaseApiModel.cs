namespace DFM.API.Models
{
    public class BaseApiModel : BaseModel
    {
        public BaseApiModel()
        {
            Environment = new Environment(theme, language);
        }

        internal Environment Environment { get; }
    }
}
