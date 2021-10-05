using System;
using System.Linq;
using MackySoft.Modiferty;

namespace StatusManager.FlexibleModiferty {
	public static class IModifiableExtensions {


        public static bool ContainsModifier<T> (this IModifiable<T> source,IModifier<T> modifier) {
			return source.Modifiers.Contains(modifier);
		}
		public static void AddModifier<T> (this IModifiable<T> source,IModifier<T> modifier) {
			source.Modifiers.Add(modifier);
		}

		public static IDisposable AddModifierAsDisposable<T> (this IModifiable<T> source,IModifier<T> modifier) {
			return source.Modifiers.AddModifierAsDisposable(modifier);
		}

		/// <summary>
		/// Shortcut of <see cref="IModifiable{T}.Modifiers"/>.Remove(modifier);
		/// </summary>
		public static bool RemoveModifier<T> (this IModifiable<T> source,IModifier<T> modifier) {
			return source.Modifiers.Remove(modifier);
		}

		/// <summary>
		/// Shortcut of <see cref="IModifiable{T}.Modifiers"/>.Clear();
		/// </summary>
		public static void ClearModifiers<T> (this IModifiable<T> source) {
			source.Modifiers.Clear();
		}

	}

}