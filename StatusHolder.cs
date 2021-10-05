using System;
using System.Collections.Generic;

namespace StatusManager{
    public abstract class StatusHolder:IStatusHolder{
        protected Dictionary<Type,IStatusInt> _statusDictionary;
        public IStatusInt getStatus(Type type){
            return (IStatusInt)_statusDictionary[type];
        }

        InApplicableStatusInt InApplicableStatusHolder.getStatus(Type type)
        {
            return (IStatusInt)_statusDictionary[type];
        }
        public IStatusInt getStatus<T>() where T : IStatusInt
        {
            var res=_statusDictionary[typeof(T)];
            return res;

        }

        InApplicableStatusInt InApplicableStatusHolder.getStatus<T>()
        {
            var res=_statusDictionary[typeof(T)];

            return (T)res;
        }

        public IReadOnlyDictionary<Type, IStatusInt> getAll()
        {
           return _statusDictionary;
        }

        public StatusHolder(IReadOnlyList<IStatusInt> statusList){
            _statusDictionary=new Dictionary<Type, IStatusInt>();
            foreach (IStatusInt item in statusList)
            {
                Type type=item.GetType();
                if(!_statusDictionary.ContainsKey(type))
                    _statusDictionary.Add(type,item);
                else
                    throw new Exception($"key {type} is already included.");
 
            }
            
           
        }
    }
    public interface IStatusHolder:InApplicableStatusHolder{
        new IStatusInt getStatus<T>()where T:IStatusInt;
        new IStatusInt getStatus(Type type);
       
    }
     public interface InApplicableStatusHolder{
        InApplicableStatusInt getStatus<T>()where T:InApplicableStatusInt;
        InApplicableStatusInt getStatus(Type type);
        IReadOnlyDictionary<Type, IStatusInt> getAll();
    }
}