using System;

namespace DFM.MVC.Helpers.Extensions
{
    public static class Int32Extension
    {
        public static Int32 ForceBetween(this Int32 number, Int32 min, Int32 max)
        {
            return number > max ? max
                       : number < min ? min
                       : number;
        }
    }
}