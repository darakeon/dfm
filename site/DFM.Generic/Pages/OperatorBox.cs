using System;
using System.Collections.Generic;
using System.Reflection;

namespace DFM.Generic.Pages
{
	public class OperatorBox<T>
	{
		public OperatorBox(T item)
		{
			Item = item;
		}

		public T Item { get; }

		public static OperatorBox<T> operator +(OperatorBox<T> @this, Int32 other)
		{
			var result = invokeOperator<T>("+", @this.Item, other);
			return new OperatorBox<T>(result);
		}

		public static OperatorBox<T> operator ++(OperatorBox<T> @this)
		{
			var result = invokeOperator<T>("++", @this.Item);
			return new OperatorBox<T>(result);
		}

		public static OperatorBox<T> operator -(OperatorBox<T> @this, Int32 other)
		{
			var result = invokeOperator<T>("-", @this.Item, other);
			return new OperatorBox<T>(result);
		}

		public static OperatorBox<T> operator --(OperatorBox<T> @this)
		{
			var result = invokeOperator<T>("--", @this.Item);
			return new OperatorBox<T>(result);
		}

		public static Boolean operator <(OperatorBox<T> @this, OperatorBox<T> other)
		{
			return invokeOperator<Boolean>("<", @this.Item, other.Item);
		}

		public static Boolean operator >(OperatorBox<T> @this, OperatorBox<T> other)
		{
			return invokeOperator<Boolean>(">", @this.Item, other.Item);
		}

		public static Boolean operator <=(OperatorBox<T> @this, OperatorBox<T> other)
		{
			return invokeOperator<Boolean>("<=", @this.Item, other.Item);
		}

		public static Boolean operator >=(OperatorBox<T> @this, OperatorBox<T> other)
		{
			return invokeOperator<Boolean>(">=", @this.Item, other.Item);
		}

		public static Boolean operator ==(OperatorBox<T> @this, OperatorBox<T> other)
		{
			if (@this == null)
				return other == null;

			return invokeOperator<Boolean>("==", @this.Item, other);
		}

		public static Boolean operator !=(OperatorBox<T> @this, OperatorBox<T> other)
		{
			if (@this == null)
				return other != null;

			return invokeOperator<Boolean>("!=", @this.Item, other);
		}

		private static TR invokeOperator<TR>(String method, params object[] @params)
		{
			var reflectionMethod = reflectionMethods[method];
			return (TR)typeof(T).InvokeMember(reflectionMethod, BindingFlags.InvokeMethod, null, null, @params);
		}

		// ReSharper disable once StaticMemberInGenericType
		private static readonly IDictionary<String, String> reflectionMethods =
			new Dictionary<String, String>
			{
				{"+", "op_Addition"},
				{"++", "op_Increment"},
				{"-", "op_Subtraction"},
				{"--", "op_Decrement"},
				{"<", "op_LessThan"},
				{">", "op_GreaterThan"},
				{"<=", "op_LessThanOrEqual"},
				{">=", "op_GreaterThanOrEqual"},
				{"==", "op_Equality"},
				{"!=", "op_Inequality"},
			};


		#region Equality Members
		protected bool Equals(OperatorBox<T> other)
		{
			return EqualityComparer<T>.Default.Equals(Item, other.Item);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((OperatorBox<T>)obj);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<T>.Default.GetHashCode(Item);
		}
		#endregion Equality Members
	}
}