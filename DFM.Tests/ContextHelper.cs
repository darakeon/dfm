using System;
using TechTalk.SpecFlow;

namespace DFM.Tests
{
    public class ContextHelper
    {
        protected static T Get<T>(String key)
        {
            return ScenarioContext.Current.ContainsKey(key)
                ? (T)ScenarioContext.Current[key]
                : default(T);
        }

        protected static void Set(String key, object value)
        {
            ScenarioContext.Current[key] = value;
        }
    }


}
