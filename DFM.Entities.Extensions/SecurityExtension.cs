using System;
using DFM.Entities;

namespace DFM.Extensions
{
    public static class SecurityExtension
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
