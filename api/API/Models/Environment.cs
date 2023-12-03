using System;
using DFM.Generic;

namespace DFM.API.Models
{
    public class Environment
    {
        public Environment(Theme theme, string language)
        {
            Theme = theme.ToString();
            Language = language;
        }

        public string Theme { get; }
        public string Language { get; }
    }
}
