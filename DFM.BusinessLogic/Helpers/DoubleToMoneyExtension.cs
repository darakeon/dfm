using System;
using DFM.Multilanguage;

namespace DFM.BusinessLogic.Helpers
{
    public static class DoubleToMoneyExtension
    {
        public static String ToMoney(this Double number, String language)
        {
            return number.ToString("#0.00", PlainText.GetNumberFormat(language));
        }
    }
}