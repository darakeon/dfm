using Newtonsoft.Json;

namespace DFM.Generic
{
    public class ConvertToChild
    {
        /// <summary>
        /// Convert an object of some base class to another child that is not de original
        /// </summary>
        /// <typeparam name="TP">Type parent</typeparam>
        /// <typeparam name="TO">Type other child</typeparam>
        public static TO Convert<TP, TO>(TP obj)
            where TO : TP
        {
            var serial = JsonConvert.SerializeObject(obj);

            return JsonConvert.DeserializeObject<TO>(serial);
        }

    }
}
