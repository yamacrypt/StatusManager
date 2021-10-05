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
    public abstract class StatusModifier<T> :IStatusModifier
        where T:IStatusInt{
        IModifier<int> linker;
        public StatusModifier(InApplicableStatusInt status,Func<InApplicableStatusInt,IModifier<int>> linker){
            LinkObservable= (IStatusInt target) =>
            {
                status.Link(target);
            };
            UnLinkObservable= (IStatusInt target) =>
            {
                status.UnLink(target);
            };
            this.linker=linker(status);
        }
        public StatusModifier(IModifier<int> modifier){
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
            var dis=_apply(holder,typeof(T));
            res.Add(dis);
            if(LinkObservable!=null){
                LinkObservable(holder.getStatus<T>());
                res.Add(new Disposer(()=>UnLinkObservable(holder.getStatus(typeof(T)))));
                LinkObservable=null;
            }
            return new DisposableCollection(res);
        }

    }
    public interface IStatusModifier{
        IDisposable apply(IStatusHolder holder);
    }
 

}