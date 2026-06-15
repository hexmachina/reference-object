using System;
using Unity.Properties;
using UnityEngine;
[assembly: GeneratePropertyBagsForAssembly]


namespace TW.ReferenceObjects
{
	[GeneratePropertyBag]
	public abstract partial class ReferenceBase<T> : ScriptableObject
	{
		[SerializeField] protected T _defaultValue;

		public T defaultValue => _defaultValue;

		[NonSerialized] protected T _value;

		[NonSerialized] protected bool _isInitialized = false;

		public bool IsInitialized => _isInitialized;

		[field: SerializeField, CreateProperty, HideInInspector]
		public T Value
		{
			get
			{
				Initialize();
				return _value;
			}

			set
			{
				Initialize();
				if (!Equals(_value, value))
				{
					_value = value;
					ValueChanged(_value);
				}
			}
		}

		public event Action<T> OnValueChanged;

		protected virtual void Initialize()
		{
			if (_isInitialized)
			{
				return;
			}
			_isInitialized = true;
			_value = _defaultValue;
		}

		public virtual void RestoreDefault()
		{
			_value = _defaultValue;

		}

		protected virtual void ValueChanged(T val)
		{
			OnValueChanged?.Invoke(val);
		}

		public virtual void SetWithoutNotify(T val)
		{
			Initialize();
			_value = val;
		}

		public virtual void SetAndNotify(T val)
		{
			Initialize();
			_value = val;
			OnValueChanged?.Invoke(_value);
		}

		public virtual void Notify()
		{
			OnValueChanged?.Invoke(Value);
		}
	}
}
