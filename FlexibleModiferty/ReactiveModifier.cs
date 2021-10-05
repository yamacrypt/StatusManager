using MackySoft.Modiferty;
using Hexat;
using UnityEngine;
using System;

namespace StatusManager.FlexibleModiferty{
    abstract public class ReactiveOperatorModifierBase<TRHS,TResult> : IModifier<TResult>
    {
        protected int m_Priority;
        public int Priority {
			get => m_Priority;
			set => m_Priority = value;
		}
        protected IReadOnlyReactiveModifiable<TRHS> modifiable;
        protected ReactiveOperatorModifierBase(IReadOnlyReactiveModifiable<TRHS> modifiable)
        {
            this.modifiable=modifiable;
        }
        protected ReactiveOperatorModifierBase(TRHS value=default) : this(new ReactiveModifiable<TRHS>(value))
        {
        }

        abstract public TResult Evaluate(TResult value);
        
    }
    public abstract class ReactiveOperatorModifierBase<T> : ReactiveOperatorModifierBase<T, T>
    {
        protected ReactiveOperatorModifierBase(IReadOnlyReactiveModifiable<T> modifiable) : base(modifiable)
        {
        }
        protected ReactiveOperatorModifierBase(T value=default) : base(new ReactiveModifiable<T>(value))
        {
        }
    }
    public class ReactiveAdditiveModifier<TRHS, TResult> : ReactiveOperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=0;
        protected int m_Property=globalProperty;
        public ReactiveAdditiveModifier(IReadOnlyReactiveModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public ReactiveAdditiveModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (TResult)((dynamic)value+(dynamic)modifiable.Evaluate());
        }
    }
    public class ReactiveAdditiveModifier<T> : ReactiveAdditiveModifier<T, T>
    {
        public ReactiveAdditiveModifier(IReadOnlyReactiveModifiable<T> modifiable) : base(modifiable)
        {
        }

        public ReactiveAdditiveModifier(T value = default) : base(value)
        {
        }
    }
    public class ReactiveSubtractiveModifier<TRHS, TResult> : ReactiveOperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=0;
        protected int m_Property=globalProperty;
        public ReactiveSubtractiveModifier(IReadOnlyReactiveModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public ReactiveSubtractiveModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (TResult)((dynamic)value-(dynamic)modifiable.Evaluate());
        }
    }
    public class ReactiveSubtractiveModifier<T> : ReactiveSubtractiveModifier<T, T>
    {
        public ReactiveSubtractiveModifier(IReadOnlyReactiveModifiable<T> modifiable) : base(modifiable)
        {
        }

        public ReactiveSubtractiveModifier(T value = default) : base(value)
        {
        }
    }
    public class ReactiveMultiplyModifier<TRHS, TResult> : ReactiveOperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=1;
        protected int m_Property=globalProperty;
        public ReactiveMultiplyModifier(IReadOnlyReactiveModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public ReactiveMultiplyModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (TResult)((dynamic)value*(dynamic)modifiable.Evaluate());
        }
    }
    public class ReactiveMultiplyModifier<T> : ReactiveMultiplyModifier<T, T>
    {
        public ReactiveMultiplyModifier(IReadOnlyReactiveModifiable<T> modifiable) : base(modifiable)
        {
        }

        public ReactiveMultiplyModifier(T value = default) : base(value)
        {
        }
    }
    public class ReactiveDivisionModifier<TRHS, TResult> : ReactiveOperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=1;
        protected int m_Property=globalProperty;
        public ReactiveDivisionModifier(IReadOnlyReactiveModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public ReactiveDivisionModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (dynamic)value/(dynamic)modifiable.Evaluate();
        }
    }
    public class ReactiveDivisionModifier<T> :ReactiveDivisionModifier<T,T>
    {
        public ReactiveDivisionModifier(IReadOnlyReactiveModifiable<T> modifiable) : base(modifiable)
        {
        }

        public ReactiveDivisionModifier(T value = default) : base(value)
        {
        }
    }
    public class ReactiveSetModifier<TRHS, TResult> : ReactiveOperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=2;
        protected int m_Property=globalProperty;
        public ReactiveSetModifier(IReadOnlyReactiveModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public ReactiveSetModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (dynamic)modifiable.Evaluate();
        }
    }
    public class ReactiveSetModifier<T> :ReactiveSetModifier<T,T>
    {
        public ReactiveSetModifier(IReadOnlyReactiveModifiable<T> modifiable) : base(modifiable)
        {
        }

        public ReactiveSetModifier(T value = default) : base(value)
        {
        }
    }

    



}