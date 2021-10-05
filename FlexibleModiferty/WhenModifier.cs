using System;
using MackySoft.Modiferty;
using UniRx;

namespace StatusManager.FlexibleModiferty{
    public class WhereModifier<T>:IModifier<T>{
        public  IWhereOperator whereOperator{get;protected set;}
        protected IModifier<T> modifier;

        public int Priority => modifier.Priority;

        public T Evaluate(T value){
            if(whereOperator.Satisfy())return modifier.Evaluate(value);
            else return value;
        }


        public WhereModifier(IModifier<T> modifier,IWhereOperator where)
        {
            this.modifier=modifier;
            this.whereOperator=where;

        }
    }
    public interface IWhereOperator{
        bool Satisfy();
    }
    public class WhereOperator : IWhereOperator
    {
        public readonly Func<bool> satisfy;
        public WhereOperator(Func<bool> satisfy)
        {
            this.satisfy = satisfy;
        }

        public bool Satisfy()
        {
            return satisfy();
        }
    }
    public class MortalOperator : IWhereOperator
    {
        public readonly float activeTime;
        public MortalOperator(float activeTime)
        {
            this.activeTime = activeTime;
            CountTimer();
        }
        void CountTimer(){
            Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1/100f)) //0秒後から1秒間隔で実行
            .Select(x => (int)(this.activeTime - x))                       //xは起動してからの秒数
            .TakeWhile(x => x > 0);
        }

        public bool Satisfy()
        {
            return activeTime>0f;
        }
    }
}