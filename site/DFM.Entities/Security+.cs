using System;
using token = DK.Generic.Extensions.Token;

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
