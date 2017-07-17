using System;

namespace DFM.MVC.Helpers.Extensions
{
    public static class IntExtension
    {
        public static Int64 ForceBetween(this Int64 number, Int64 min, Int64 max)
        {
            return number > max ? max
                       : number < min ? min
                       : number;
        }

        public static Int32 ForceBetween(this Int32 number, Int32 min, Int32 max)
        {
            return number > max ? max
                       : number < min ? min
                       : number;
        }

        public static Int16 ForceBetween(this Int16 number, Int16 min, Int16 max)
        {
            return number > max ? max
                       : number < min ? min
                       : number;
        }
    }
}