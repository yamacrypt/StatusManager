using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZeroFormatter.Formatters;
using ZeroFormatter;
using StatusManager.FlexibleModiferty;
using StatusManager;

namespace StatusManager{
    public class DisposableCollection : IDisposable
    {
        IEnumerable<IDisposable> args;
        public DisposableCollection(IEnumerable<IDisposable> args ){
            this.args=args;
        }
        public void Dispose()
        {
            foreach(var ele in args)
                ele.Dispose();
        }
    }
    public abstract class StatusModifier :IStatusModifier{
        IModifier<int> linker;
        public Type targetStatusType{get;private set;}
        public StatusModifier(Type targetStatusType,InApplicableStatusInt status,Func<InApplicableStatusInt,IModifier<int>> linker){
            LinkObservable= (IStatusInt target) =>
            {
                status.Link(target);
            };
            UnLinkObservable= (IStatusInt target) =>
            {
                status.UnLink(target);
            };
            this.targetStatusType=targetStatusType;
            this.linker=linker(status);
        }
        public StatusModifier(Type targetStatusType,IModifier<int> modifier){
            this.targetStatusType=targetStatusType;
            this.linker=modifier;
        }
        Action<IStatusInt> LinkObservable;  
        Action<IStatusInt> UnLinkObservable;
        class Disposer : IDisposable
        {
            Action method;
            public Disposer(Action method)
            {
                this.method=method;
            }

            public void Dispose()
            {
                method();
            }
        }
        IDisposable _apply(IStatusHolder holder,Type statustype){
            return holder.getStatus(statustype).apply(linker);
        }
        public  virtual  IDisposable apply(IStatusHolder holder)
        {

            List<IDisposable> res=new List<IDisposable>();
            var dis=_apply(holder,targetStatusType);
            res.Add(dis);
            if(LinkObservable!=null){
                LinkObservable(holder.getStatus(targetStatusType));
                res.Add(new Disposer(()=>UnLinkObservable(holder.getStatus(targetStatusType))));
                LinkObservable=null;
            }
            return new DisposableCollection(res);
        }
    }

    public abstract class StatusModifier<T> :StatusModifier, IStatusModifier
        where T:IStatusInt{
        IModifier<int> linker;
        public StatusModifier(InApplicableStatusInt status,Func<InApplicableStatusInt,IModifier<int>> linker):base(typeof(T),status,linker){
        }
        public StatusModifier(IModifier<int> modifier):base(typeof(T),modifier){
        }
    }
    public interface IStatusModifier{
        IDisposable apply(IStatusHolder holder);
    }
 

}