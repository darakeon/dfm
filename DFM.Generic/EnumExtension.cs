using System;

namespace DFM.Generic
{
    public static class EnumX
    {
        public static T Parse<T>(String value)
        {
            return (T)Enum.Parse(typeof (T), value);
        }
    }
}
