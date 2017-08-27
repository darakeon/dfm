using System;

namespace DFM.Entities
{
    public partial class Config
    {
        public override String ToString()
        {
            return String.Format("[{0}]", ID);
        }

    }
}
