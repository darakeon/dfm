using System;

namespace DFM.Generic.Pages
{
	public class OperatorBox
	{
		public OperatorBox(IPage item)
		{
			Item = item;
		}

		public IPage Item { get; }

		public static OperatorBox operator +(OperatorBox @this, Int32 other)
		{
			return new OperatorBox(@this.Item.Add(other));
		}

		public static OperatorBox operator ++(OperatorBox @this)
		{
			return @this + 1;
		}

		public static OperatorBox operator -(OperatorBox @this, Int32 other)
		{
			return @this + (-other);
		}

		public static OperatorBox operator --(OperatorBox @this)
		{
			return @this - 1;
		}

		public static Boolean operator <(OperatorBox @this, OperatorBox other)
		{
			return @this.Item.LessThan(other.Item);
		}

		public static Boolean operator >(OperatorBox @this, OperatorBox other)
		{
			return @this.Item.GreaterThan(other.Item);
		}

		public static Boolean operator <=(OperatorBox @this, OperatorBox other)
		{
			return !(@this > other);
		}

		public static Boolean operator >=(OperatorBox @this, OperatorBox other)
		{
			return !(@this < other);
		}

		#region Equality Members
		protected bool equals(OperatorBox other)
		{
			return Item.Equals(other.Item);
		}

		public override Boolean Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && equals((OperatorBox)obj);
		}

		public override int GetHashCode()
		{
			return Item.GetHashCode();
		}
		#endregion Equality Members
	}
}
