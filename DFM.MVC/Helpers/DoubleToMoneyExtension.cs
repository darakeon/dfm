using System;
using DFM.MVC.MultiLanguage;

namespace DFM.MVC.Helpers
{
    public static class DoubleToMoneyExtension
    {
        public static String ToMoney(this Double number)
        {
            return number.ToString("#0.00", PlainText.Culture.NumberFormat);
        }
    }
}