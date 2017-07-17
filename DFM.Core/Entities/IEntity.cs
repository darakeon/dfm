using System;

namespace DFM.Core.Entities
{
    public interface IEntity
    {
        Int32 ID { get; set; }

        //public virtual Int32 ID { get; set; }

        //public static Boolean operator ==(IEntity x, IEntity y)
        //{
        //    var isNullX = isNull(x);
        //    var isNullY = isNull(y);

        //    return isNullX ? isNullY
        //               : isNullY ? false : x.ID == y.ID;
        //}
        
        //// Made to operator == don't become recursive
        //private static Boolean isNull(IEntity entity)
        //{
        //    try { return entity.GetType().Name != String.Empty; }
        //    catch (NullReferenceException) { return true; }
        //}



        //public static bool operator !=(IEntity x, IEntity y)
        //{
        //    return !(x == y);
        //}

        //public virtual bool Equals(IEntity other)
        //{
        //    return this == other;
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj.GetType() != GetType())
        //        return false;

        //    return this == (IEntity)obj;
        //}

        //public override int GetHashCode()
        //{
        //    return ID;
        //}
    }
}
