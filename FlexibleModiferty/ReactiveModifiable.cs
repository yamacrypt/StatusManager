using System;
using Hexat;
using MackySoft.Modiferty;
using UniRx;

namespace StatusManager.FlexibleModiferty{
    public interface IReadOnlyReactiveModifiable<T> : IReadOnlyModifiable<T>, IReadOnlyReactiveProperty<T> {
		new IReadOnlyReactiveModifierList<T> Modifiers { get; } 
	}

	public interface IReactiveModifiable<T> : IReadOnlyReactiveModifiable<T>, IModifiable<T>, IReactiveProperty<T> {
		new IReactiveModifierList<T> Modifiers { get; }
	}
    [Serializable]
	public class ReactiveModifiable<T> : ReactiveProperty<T>, IReactiveModifiable<T> {

		ReactiveModifierList<T> m_Modifiers;
		//ISecureValue<T> m_BaseValue= (ISecureValue<T>)SecureValue.New<T>();

		public T BaseValue {
			get => base.Value;
			set => base.Value = value;
		}

		new T Value {
			get => base.Value;
			set => base.Value = value;
		}
		
		public IReactiveModifierList<T> Modifiers => m_Modifiers ?? (m_Modifiers = new ReactiveModifierList<T>());

		public bool HasModifiers => (m_Modifiers != null) && (m_Modifiers.Count > 0);

		public ReactiveModifiable () : this(default) {
		}

		public ReactiveModifiable (T baseValue) : base(baseValue) {
		}

		public T Evaluate () {
			return (m_Modifiers != null) ? m_Modifiers.Evaluate(BaseValue) : BaseValue;
		}

        public IModifier<TResult> convert<TResult>(OperatorType type)
        {
            switch(type){
                case OperatorType.Additive:return new ReactiveAdditiveModifier<T,TResult>(this);
                case OperatorType.Subtractive:return new ReactiveSubtractiveModifier<T,TResult>(this);
                case OperatorType.Multiply:return new ReactiveMultiplyModifier<T,TResult>(this);
                case OperatorType.Division:return new ReactiveDivisionModifier<T,TResult>(this);
            }
            throw new NotImplementedException();
        }

        public IModifier<T> convert(OperatorType type)
        {
            return convert<T>(type);
        }

        #region Explicit implementation

        IReadOnlyReactiveModifierList<T> IReadOnlyReactiveModifiable<T>.Modifiers => Modifiers;

		IModifierList<T> IModifiable<T>.Modifiers => Modifiers;

		IReadOnlyModifierList<T> IReadOnlyModifiable<T>.Modifiers => Modifiers;



        public T baseValue =>BaseValue;



        #endregion

    }

	public static class ReactiveModifiableExtensions {

		/// <summary>
		/// Notify BaseValue and Modifiers changes.
		/// </summary>
		public static IObservable<IReadOnlyReactiveModifiable<T>> ObserveChanged<T> (this IReadOnlyReactiveModifiable<T> source) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			return Observable.Merge(
				source.AsUnitObservable(),
				source.Modifiers.ObserveChanged().AsUnitObservable()
			).Select(_ => source);
		}

	}
}