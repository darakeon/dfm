using System;

namespace DFM.Generic
{
    public static class Token
    {
        public static String New()
        {
            return Guid.NewGuid()
                .ToString()
                .ToUpper()
                .Replace("-", "");
        }

    }
}
