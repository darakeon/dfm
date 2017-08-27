using System;

namespace DFM.Entities
{
    public partial class Category
    {
		private void init()
		{
			Active = true;
		}

		public override String ToString()
		{
			return String.Format("[{0}] {1}", ID, Name);
		}
		
		public virtual Boolean AuthorizeCRUD(User user)
        {
            return User == user;
        }
    }
}
