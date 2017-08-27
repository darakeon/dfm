using System;

namespace DFM.MVC.Helpers.Views
{
    //TODO: REMOVE THIS SHIT
    public static class ValueSign
    {
        public static Boolean IsNegative(this Double number)
        {
            return (Int32)(number * 100) < 0;
        }

    }
}