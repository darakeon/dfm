using System;
using System.Threading;

namespace DFM.Core.Helpers
{
    //TO-DO: Use PlainText, when I find a way to find the files

    public static class DoubleToMoneyExtension
    {
        public static String ToMoney(this Double number)
        {
            return number.ToString("#0.00", Thread.CurrentThread.CurrentUICulture.NumberFormat);
        }
    }
}