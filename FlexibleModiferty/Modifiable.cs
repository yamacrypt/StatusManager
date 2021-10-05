using System;
using Hexat;
using MackySoft.Modiferty;
using UnityEngine;

namespace StatusManager.FlexibleModiferty{
    public enum OperatorType {
		Additive = 0,
		Subtractive = 1,
		Multiply = 2,
		Division = 3,
		Set = 4,
	}
    public interface  IReadOnlyModifiable<T>{
        IReadOnlyModifierList<T> Modifiers { get; }
        T Evaluate();
        T baseValue{get;}
        IModifier<TResult> convert<TResult>(OperatorType type);
        IModifier<T> convert(OperatorType type);


    }
    public interface IModifiable<T>:IReadOnlyModifiable<T>{
        new IModifierList<T> Modifiers { get; }

    }
    public class  Modifiable<T> : IModifiable<T>
    {
        [SerializeField]
		ISecureValue<T> m_BaseValue= (ISecureValue<T>)SecureValue.New<T>();
        [NonSerialized]
		ModifierList<T> m_Modifiers;
        public Modifiable(T value=default)
        {
            m_BaseValue.Value=value;
        }
        public IModifierList<T> Modifiers =>  m_Modifiers ?? (m_Modifiers = new ModifierList<T>());
        public bool HasModifiers => (m_Modifiers != null) && (m_Modifiers.Count > 0);
        public T baseValue => m_BaseValue.Value;

        IReadOnlyModifierList<T> IReadOnlyModifiable<T>.Modifiers =>  Modifiers;


        public IModifier<TResult> convert<TResult>(OperatorType type)
        {
            switch(type){
                case OperatorType.Additive:return new AdditiveModifier<T,TResult>(this);
                case OperatorType.Subtractive:return new SubtractiveModifier<T,TResult>(this);
                case OperatorType.Multiply:return new MultiplyModifier<T,TResult>(this);
                case OperatorType.Division:return new DivisionModifier<T,TResult>(this);
            }
            throw new NotImplementedException();
        }

        public IModifier<T> convert(OperatorType type)
        {
            return convert<T>(type);
        }

        public T Evaluate()
        {
            return (m_Modifiers != null) ? m_Modifiers.Evaluate(m_BaseValue.Value) : m_BaseValue.Value;
        }
    }
   

}