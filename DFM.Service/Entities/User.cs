using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DFM.Service.Helpers;

namespace DFM.Service.Entities
{
    [DataContract]
    public class User : Core.Entities.User
    {
        public User(Core.Entities.User user)
        {
            ID = user.ID;
            Login = user.Login;
            Password = user.Password;
            Email = user.Email;
            base.AccountList = user.AccountList;
            base.CategoryList = user.CategoryList;
        }


        
        [DataMember]
		public new Int32 ID { get { return base.ID; } set { ID = value; } }
		[DataMember]
		public new String Login { get { return base.Login; } set { Login = value; } }
		[DataMember]
		public new String Password { get { return base.Password; } set { Password = value; } }
		[DataMember]
		public new String Email { get { return base.Email; } set { Email = value; } }
		[DataMember]
        public new IList<Account> AccountList { get { return base.AccountList.Cast(); } set { AccountList = value; } }
		[DataMember]
        public new IList<Category> CategoryList { get { return base.CategoryList.Cast(); } set { CategoryList = value; } }

    }
}
