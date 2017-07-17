using System;

namespace DFM.Core.Entities.Extensions
{
    public static class SummaryExtension
    {
        internal static Double Value(this Summary summary)
        {
            return Math.Round(summary.In - summary.Out, 2);
        }

    }
}
