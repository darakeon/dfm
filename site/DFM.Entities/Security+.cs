using token = Ak.Generic.Extensions.Token;

namespace DFM.Entities
{
    public partial class Security
    {
        public virtual void CreateToken()
        {
            Token = token.New();
        }

    }
}
