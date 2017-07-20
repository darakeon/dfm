using System;
using System.Threading;

namespace DFM.BusinessLogic.Helpers
{
    public static class DoubleToMoneyExtension
    {
        public static String ToMoney(this Double number)
        {                                   //TO-DO: Use PlainText, when I find a way to find the files
            return number.ToString("#0.00", Thread.CurrentThread.CurrentUICulture.NumberFormat);
        }
    }
}