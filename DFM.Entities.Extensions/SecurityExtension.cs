using DFM.Generic;

namespace DFM.Entities.Extensions
{
    public static class SecurityExtension
    {
        public static void CreateToken(this Security security)
        {
            security.Token = Token.New();
        }

    }
}
