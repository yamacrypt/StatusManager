using System;
using UniRx;

namespace StatusManager.FlexibleModiferty{
    public abstract class RangeModifier<T>:IModifier<T>{
        protected IModifier<T> modifier;
        protected Func<T> min;
        protected Func<T> max;
        public RangeModifier(IModifier<T> modifier,Func<T> min, Func<T> max)
        {
            this.modifier=modifier;
            this.min = min;
            this.max = max;
        }

        public int Priority => modifier.Priority;

        public abstract T Evaluate(T value);

    }
    public class RangeModifierInt : RangeModifier<int>
    {
        public RangeModifierInt(IModifier<int> modifier, Func<int> min, Func<int> max) : base(modifier, min, max)
        {
        }

        public override int Evaluate(int value)
        {
            return Math.Max(Math.Min(modifier.Evaluate(value),max()),min());
        }
    }
}