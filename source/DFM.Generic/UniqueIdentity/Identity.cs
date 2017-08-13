using System;
using System.Collections.Generic;

namespace DFM.Generic.UniqueIdentity
{
    public class Identity<T>
    {
        private static String cookie
        {
            get { return MyCookie.Get(); }
        }

        private IDictionary<String, T> tickets =
            new Dictionary<String, T>();



        public Boolean Exists
        {
            get { return tickets.ContainsKey(cookie); }
        }

        public T ID
        {
            get { return tickets[cookie]; }
        }

        public void Kill()
        {
            tickets.Remove(cookie);
        }

        public void Add(T value)
        {
            tickets.Add(cookie, value);
        }
    }
}
