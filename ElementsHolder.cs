using System;
using System.Collections.Generic;

namespace StatusManager{
    public abstract class ElementsHolder:IElementsHolder{
         protected Dictionary<Type, IStatusHolder> _elementDictionary;
         protected Dictionary<Type, IStatusInt> _statusDictionary;

        public ElementsHolder(IReadOnlyList<IStatusHolder> elementsList){
            _elementDictionary=new Dictionary<Type, IStatusHolder>();
            _statusDictionary=new Dictionary<Type, IStatusInt>();
            foreach (IStatusHolder item in elementsList)
            {
                Type elementtype=item.GetType();
                if(!_elementDictionary.ContainsKey(elementtype))
                    _elementDictionary.Add(elementtype,item);
                else
                    throw new Exception($"key {elementtype} is already included.");
                foreach (var status in item.getAll().Values)
                {
                    Type statusType=status.GetType();
                    if(!_statusDictionary.ContainsKey(statusType))
                        _statusDictionary.Add(statusType,status);
                    else
                        throw new Exception($"key {statusType} is already included.");
                }
               
            }
            
           
        }
        public IStatusInt getStatus(Type type){
            return (IStatusInt)_statusDictionary[type];
        }

        public IReadOnlyDictionary<Type, IStatusHolder> elementDictionary => this._elementDictionary;

         public IStatusInt getStatus<T>()where T:IStatusInt
        {
            try{
            var res=_statusDictionary[typeof(T)];
            return (T)res;
            }
            catch(Exception e){
                throw new Exception($"{typeof(T)} not exists");
            }
        }

        InApplicableStatusInt InApplicableStatusHolder.getStatus<T>()
        {
            return _statusDictionary[typeof(T)];
        }

        InApplicableStatusInt InApplicableStatusHolder.getStatus(Type type)
        {
            return _statusDictionary[type];
        }

        public IReadOnlyDictionary<Type, IStatusInt> getAll()
        {
            return _statusDictionary;
        }


        IStatusHolder IElementsHolder.getElement<T>()
        {
            return _elementDictionary[typeof(T)];
        }
        public InApplicableStatusHolder getElement<T>() where T : InApplicableStatusInt
        {
            return _elementDictionary[typeof(T)];
        }

        public IStatusHolder getElement(Type type)
        {
            return _elementDictionary[type];
        }

        InApplicableStatusHolder InApplicableElementsHolder.getElement(Type type)
        {
            return _elementDictionary[type];            
        }

        public IReadOnlyDictionary<Type, IStatusHolder> getAllElements()
        {
           return _elementDictionary;
        }


    }
     public interface IElementsHolder:IStatusHolder,InApplicableElementsHolder{

        IReadOnlyDictionary<Type,IStatusHolder> elementDictionary{get;}
        new IStatusHolder getElement<T>()where T:IStatusHolder;
        new IStatusHolder getElement(Type type);
    }
    public interface InApplicableElementsHolder:InApplicableStatusHolder{
        InApplicableStatusHolder getElement<T>()where T:InApplicableStatusInt;
        InApplicableStatusHolder getElement(Type type);
        IReadOnlyDictionary<Type, IStatusHolder> getAllElements();
    }
}