using System;
using System.Linq;
using System.Collections.Generic;

namespace DFM.MVC.MultiLanguage.Helpers
{
    public class DicList<T> : List<T>
        where T : class, INameable
    {
        public T this[String name]
        {
            get
            {
                var item = this.FirstOrDefault(t => t.Name == name);

                if (item == null)
                    throw new DicException(String.Format("No {0} {1}", typeof(T), name));

                return item;
            }
        }
    }
}