using System;
using DFM.Entities;

namespace DFM.Extensions
{
    public static class SummaryExtension
    {
        public static Double Value(this Summary summary)
        {
            return Math.Round(summary.In - summary.Out, 2);
        }

    }
}
