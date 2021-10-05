
namespace StatusManager.FlexibleModiferty{
    public interface IModifier<T>:MackySoft.Modiferty.IModifier<T>{
    }
    abstract public class OperatorModifierBase<TRHS,TResult> : IModifier<TResult>
    {
        protected int m_Priority;
        public int Priority {
			get => m_Priority;
			set => m_Priority = value;
		}
        protected IReadOnlyModifiable<TRHS> modifiable;
        protected OperatorModifierBase(IReadOnlyModifiable<TRHS> modifiable)
        {
            this.modifiable=modifiable;
        }
        protected OperatorModifierBase(TRHS value=default) : this(new Modifiable<TRHS>(value))
        {
        }

        abstract public TResult Evaluate(TResult value);
        
    }
    public abstract class OperatorModifierBase<T> : OperatorModifierBase<T, T>
    {
        protected OperatorModifierBase(IReadOnlyModifiable<T> modifiable) : base(modifiable)
        {
        }
        protected OperatorModifierBase(T value=default) : base(new Modifiable<T>(value))
        {
        }
    }
    public class AdditiveModifier<TRHS, TResult> : OperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=2;
        protected int m_Property=globalProperty;
        public AdditiveModifier(IReadOnlyModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public AdditiveModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (TResult)((dynamic)value+(dynamic)modifiable.Evaluate());
        }
    }
    public class AdditiveModifier<T> : AdditiveModifier<T, T>
    {
        public AdditiveModifier(IReadOnlyModifiable<T> modifiable) : base(modifiable)
        {
        }

        public AdditiveModifier(T value = default) : base(value)
        {
        }
    }
    public class SubtractiveModifier<TRHS, TResult> : OperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=2;
        protected int m_Property=globalProperty;
        public SubtractiveModifier(IReadOnlyModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public SubtractiveModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (TResult)((dynamic)value-(dynamic)modifiable.Evaluate());
        }
    }
    public class SubtractiveModifier<T> : SubtractiveModifier<T, T>
    {
        public SubtractiveModifier(IReadOnlyModifiable<T> modifiable) : base(modifiable)
        {
        }

        public SubtractiveModifier(T value = default) : base(value)
        {
        }
    }
    public class MultiplyModifier<TRHS, TResult> : OperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=1;
        protected int m_Property=globalProperty;
        public MultiplyModifier(IReadOnlyModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public MultiplyModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (TResult)((dynamic)value*(dynamic)modifiable.Evaluate());
        }
    }
    public class MultiplyModifier<T> : MultiplyModifier<T, T>
    {
        public MultiplyModifier(IReadOnlyModifiable<T> modifiable) : base(modifiable)
        {
        }

        public MultiplyModifier(T value = default) : base(value)
        {
        }
    }
    public class DivisionModifier<TRHS, TResult> : OperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=1;
        protected int m_Property=globalProperty;
        public DivisionModifier(IReadOnlyModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public DivisionModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (dynamic)value/(dynamic)modifiable.Evaluate();
        }
    }
    public class DivisionModifier<T> :DivisionModifier<T,T>
    {
        public DivisionModifier(IReadOnlyModifiable<T> modifiable) : base(modifiable)
        {
        }

        public DivisionModifier(T value = default) : base(value)
        {
        }
    }
    public class SetModifier<TRHS, TResult> : OperatorModifierBase<TRHS, TResult>
    {
        public static int globalProperty=0;
        protected int m_Property=globalProperty;
        public SetModifier(IReadOnlyModifiable<TRHS> modifiable) : base(modifiable)
        {
        }

        public SetModifier(TRHS value = default) : base(value)
        {
        }

        public override TResult Evaluate(TResult value)
        {
            return (dynamic)modifiable.Evaluate();
        }
    }
    public class SetModifier<T> :SetModifier<T,T>
    {
        public SetModifier(IReadOnlyModifiable<T> modifiable) : base(modifiable)
        {
        }

        public SetModifier(T value = default) : base(value)
        {
        }
    }

    



}