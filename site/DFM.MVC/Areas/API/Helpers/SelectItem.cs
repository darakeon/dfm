using System;

namespace DFM.MVC.Areas.API.Helpers
{
    public class SelectItem<TValue, TText>
    {
        public SelectItem(TValue value, TText text)
        {
            Value = value;
            Text = text;
        }

        public TValue Value { get; set; }
        public TText Text { get; set; }

    }
}