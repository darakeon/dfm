using System;
using System.Collections.Generic;

namespace DFM.Generic.UniqueIdentity
{
    public class Identity
    {
        public static String GetGeneratedKeyFor(String name)
        {
            if (!Exists(name))
                keys.Add(name, Token.New());

            return GetKeyFor(name);
        }

        public static String GetKeyFor(String name)
        {
            if (!Exists(name))
                throw new DFMException(String.Format("Key [{0}] doesn't exists", name));

            return keys[name];
        }

        public static void SetKeyFor(String name, String value)
        {
            KillKeyFor(name);

            keys.Add(name, value);
        }

        public static void KillKeyFor(String name)
        {
            if (Exists(name))
                keys.Remove(name);
        }

        public static Boolean Exists(String name)
        {
            return keys.ContainsKey(name);
        }



        private static readonly IDictionary<String, String> keys = 
            CookieCollection.Exists
                ? (IDictionary<String, String>) new CookieCollection()
                : new Dictionary<String, String>();



    }
}
