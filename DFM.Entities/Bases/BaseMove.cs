using System;
using System.Collections.Generic;
using System.Linq;
using DFM.Entities.Enums;
using Newtonsoft.Json;

namespace DFM.Entities.Bases
{
    public class BaseMove : IEntity
    {
        public BaseMove()
        {
            DetailList = new List<Detail>();
        }

        
        public virtual Int32 ID { get; set; }

        public virtual String Description { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual MoveNature Nature { get; set; }

        public virtual Category Category { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual IList<Detail> DetailList { get; set; }

        public Double Value()
        {
            return DetailList.Sum(d => d.Value * d.Amount);
        }


        public Boolean Show()
        {
            return Date <= DateTime.Now;
        }



        public override String ToString()
        {
            return Description;
        }


        public virtual User User() { throw new NotImplementedException(); }
        public virtual Account AccOut() { throw new NotImplementedException(); }
        public virtual Account AccIn() { throw new NotImplementedException(); }



        public TC ConvertToOtherChild<TC>()
            where TC : IEntity
        {
            var serial = JsonConvert.SerializeObject(this);

            var newObj = JsonConvert.DeserializeObject<TC>(serial);

            newObj.ID = 0;

            return newObj;
        }


    }
}
