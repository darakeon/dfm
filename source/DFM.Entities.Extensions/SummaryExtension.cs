using System;

namespace DFM.Entities.Extensions
{
    public static class SummaryExtension
    {
        public static Double Value(this Summary summary)
        {
            return Math.Round(summary.In - summary.Out, 2);
        }

    }
}
