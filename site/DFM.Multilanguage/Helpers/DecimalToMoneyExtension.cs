using System;

namespace DFM.Multilanguage.Helpers
{
    public static class DecimalToMoneyExtension
    {
        public static String ToMoney(this Decimal number, String language)
        {
            return number.ToString("#,###0.00", PlainText.GetNumberFormat(language));
        }
    }
}