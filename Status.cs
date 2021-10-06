using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using StatusManager.FlexibleModiferty;

namespace StatusManager{
    public interface IReadOnlyStatusModifiable<T>{
        T baseValue{get;}
        T Evaluate();
    }
    public interface IStatusModifiable<T>:IReadOnlyStatusModifiable<T>{
        IDisposable Modify(IModifier<T> linker);
        IModifiable<T> convert();
    }
    public interface IStatusModifiableInt:IStatusModifiable<int>{}
    public interface IStatusInt:IReactiveStatus<int>,InApplicableStatusInt{}
    public interface InApplicableStatusInt:IReadOnlyStatusInt{
        bool Link(IReadOnlyReactiveStatus<int> linker);
        bool UnLink(IReadOnlyReactiveStatus<int> linker);
    }
    public interface IReadOnlyStatusInt:IReadOnlyReactiveStatus<int>{}
    public interface IStatusFloat:IReactiveStatus<float>,InApplicableStatusFloat{}
    public interface InApplicableStatusFloat:IReadOnlyStatusFloat{
        bool Link(IReadOnlyReactiveStatus<float> linker);
        bool UnLink(IReadOnlyReactiveStatus<float> linker);
    }
    public interface IReadOnlyStatusFloat:IReadOnlyReactiveStatus<float>{}
    public abstract class StatusInt : Status<int>,IStatusInt
    {
        protected StatusInt(int baseValue) : base(baseValue)
        {
        }
    }
    public abstract class StatusFloat : Status<float>,IStatusFloat
    {
        protected StatusFloat(float baseValue) : base(baseValue)
        {
        }
    }
    public abstract class Status<T>: ReactiveProperty<T>,IReactiveStatus<T>{
        //ISecureValue<int> min,max;
        public Status(T baseValue)
        {
        }
        protected abstract IStatusModifiable<T> statusModifiable{get;}
        //bool isChanged=true;

        public  T Evaluate()=>statusModifiable.Evaluate();
       
        public T baseValue=>statusModifiable.baseValue;


        T IReadOnlyReactiveProperty<T>.Value => statusModifiable.baseValue;


        public IDisposable apply(IModifier<T> modifier)
        {    
            var res=statusModifiable.Modify(modifier);
            return res;
        }

        public IModifier<T> AsAdditiveModifier()
        {
           return new AdditiveModifier<T>(statusModifiable.convert());
        }

        public IModifier<T> AsSubtractiveModifier()
        {
            return new SubtractiveModifier<T>(statusModifiable.convert());
        }

        public IModifier<T> AsMultiplyModifier()
        {
            return new MultiplyModifier<T>(statusModifiable.convert());
        }

        public IModifier<T> AsDivisionModifier()
        {
            return new DivisionModifier<T>(statusModifiable.convert());
        }
        HashSet<IReadOnlyReactiveStatus<T>> LinkedStatus=new HashSet<IReadOnlyReactiveStatus<T>>();
        public IObservable<IReadOnlyReactiveStatus<T>> ObserveChanged()
        {
            return Observable.Merge(
				this.AsUnitObservable(),LinkedStatus.Select(ele=>ele.AsUnitObservable()).ToArray()
			).Select(_ => this);
        }

        public bool Link(IReadOnlyReactiveStatus<T> linker)
        {
            if(LinkedStatus.Contains(linker))return false;
            LinkedStatus.Add(linker);
            return true;
        }
        public bool UnLink(IReadOnlyReactiveStatus<T> linker)
        {
            if(!LinkedStatus.Contains(linker))return false;
            LinkedStatus.Remove(linker);
            return true;
        }
    }
    public interface IReactiveStatus<T>:IStatus<T>,IReactiveProperty<T>,IReadOnlyReactiveStatus<T>{
        bool Link(IReadOnlyReactiveStatus<T> linker);
        bool UnLink(IReadOnlyReactiveStatus<T> linker);

    }
    public interface IReadOnlyReactiveStatus<T>:IReadOnlyStatus<T>,IReadOnlyReactiveProperty<T>{
        IObservable<IReadOnlyReactiveStatus<T>> ObserveChanged();
    }
    public interface IStatus<T>:IReadOnlyStatus<T>{
        IDisposable apply(IModifier<T> modifier);
    }
    //将来的にInAPplicahOlder廃止してこいつをもつholderをわたすことにする.
    //IStatusにはふつうにapplyをいれる。
    public interface IReadOnlyStatus<T>{
        T Evaluate();
        T baseValue{get;}
        IModifier<T> AsAdditiveModifier();
        IModifier<T> AsSubtractiveModifier();
        IModifier<T> AsMultiplyModifier();
        IModifier<T> AsDivisionModifier();
        
    }
   
    
    
}
