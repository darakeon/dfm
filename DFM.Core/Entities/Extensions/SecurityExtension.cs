using System;

namespace DFM.Core.Entities.Extensions
{
    static class SecurityExtension
    {
        public static void CreateToken(this Security security)
        {
            security.Token = 
                Guid.NewGuid()
                    .ToString()
                    .ToUpper()
                    .Replace("-", "");
        }

    }
}
