using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DFM.Core.Enums;
using DFM.Service.Helpers;

namespace DFM.Service.Entities
{
    [DataContract]
    public class Account : Core.Entities.Account
    {
        public Account(Core.Entities.Account account)
        {
            ID = account.ID;
            Name = account.Name;
            BeginDate = account.BeginDate;
            EndDate = account.EndDate;
            Nature = account.Nature;
            base.User = account.User;
            base.YearList = account.YearList;
        }



        [DataMember]
        public new Int32 ID { get { return base.ID; } private set { base.ID = value; } }
        [DataMember]
        public new String Name { get{ return base.Name; } set{ Name = value; } }
        [DataMember]
        public new DateTime BeginDate { get{ return base.BeginDate; } set{ BeginDate = value; } }
        [DataMember]
        public new DateTime? EndDate { get{ return base.EndDate; } set{ EndDate = value; } }
        [DataMember]
        public new AccountNature Nature { get{ return base.Nature; } set{ Nature = value; } }
        [DataMember]
        public new User User { get{ return base.User.Cast(); } set{ User = value; } }
        [DataMember]
        public new IList<Year> YearList { get { return base.YearList.Cast(); } set { YearList = value; } }
        [DataMember]
        public new Double MovesSum { get{ return base.MovesSum; } }
        [DataMember]
        public new Boolean Open { get{ return base.Open; } }
    }
}
