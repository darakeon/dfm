using System;
using token = Ak.Generic.Extensions.Token;

namespace DFM.Entities
{
    public partial class Security
    {
		public override string ToString()
		{
			return String.Format("[{0}] {1}", ID, Token);
		}

        public virtual void CreateToken()
        {
            Token = token.New();
        }

    }
}
