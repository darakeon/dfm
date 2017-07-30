using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace DFM.Generic.UniqueIdentity
{
    class CookieCollection : IDictionary<String, String>
    {
        public static Boolean Exists
        {
            get { return context != null; }
        }

        private static HttpContext context
        {
            get { return HttpContext.Current; }
        }

        private static HttpCookieCollection requestCookies
        {
            get { return context.Request.Cookies; }
        }

        private static HttpCookieCollection responseCookies
        {
            get { return context.Response.Cookies; }
        }


        public String this[String name]
        {
            get
            {
                var cookie = requestCookies[name]
                             ?? responseCookies[name];

                if (cookie == null)
                    return null;

                if (cookie.Value == null)
                    Remove(name);

                return cookie.Value;
            }
            set
            {
                Add(name, value);
            }
        }

        public void Add(String name, String value)
        {
            Remove(name);

            var cookie = new HttpCookie(name)
            {
                Value = value,
                Expires = DateTime.Now.AddDays(7)
            };

            requestCookies.Add(cookie);
            responseCookies.Add(cookie);

            requestCookies[name].Value = value;
            responseCookies[name].Value = value;
        }

        public bool Remove(String name)
        {
            if (requestCookies[name] != null)
                requestCookies[name].Expires = 
                    DateTime.Now.AddMilliseconds(-1);

            if (responseCookies[name] != null)
                responseCookies[name].Expires =
                    DateTime.Now.AddMilliseconds(-1);

            return true;
        }

        public Boolean ContainsKey(string name)
        {
            return this[name] != null;
        }




        #region Not Implemented
        public ICollection<String> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public bool TryGetValue(String key, out String value)
        {
            throw new NotImplementedException();
        }

        public ICollection<String> Values
        {
            get { throw new NotImplementedException(); }
        }

        public void Add(KeyValuePair<String, String> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<String, String> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<String, String>[] array, Int32 arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<String, String> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<String, String>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion



    }
}
