using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TW.ReferenceObjects
{
	public class ReferenceBoolListener : MonoBehaviour
	{
		public enum LogicalOperator
		{
			[InspectorName("All Are True")]
			And,
			[InspectorName("Any Is True")]
			Or
		}

		public enum RelationalOperator
		{
			[InspectorName("Equal To")]
			Equal,
			[InspectorName("Not Equal To")]
			NotEqual
		}

		[System.Serializable]
		public class ReferenceBoolContainer
		{
			public ReferenceBool reference;
			public RelationalOperator condition;
			public bool targetValue;
			public bool Value
			{
				get
				{
					if (!reference)
					{
						return false;
					}
					return condition == RelationalOperator.Equal ? reference.Value == targetValue : reference.Value != targetValue;
				}
			}
		}
		public bool stopListeningOnDisable = false;
		[SerializeField] LogicalOperator operatorType = LogicalOperator.And;
		[SerializeField] private List<ReferenceBoolContainer> references = new();

		public UnityEvent<bool> onReferenceChanged = new();
		public UnityEvent onTrue = new();
		public UnityEvent onFalse = new();
		private bool _listening;

		private void OnEnable()
		{
			CheckReference();
			if (_listening)
				return;

			for (int i = 0; i < references.Count; i++)
			{
				if (!references[i].reference)
					continue;
				references[i].reference.OnValueChanged += OnReferenceChanged;
			}
			_listening = true;

		}

		private void OnReferenceChanged(bool obj)
		{
			CheckReference();
		}

		public void CheckReference()
		{
			if (operatorType == LogicalOperator.And)
			{
				for (int i = 0; i < references.Count; i++)
				{
					if (!references[i].Value)
					{
						onFalse.Invoke();
						onReferenceChanged.Invoke(false);
						return;
					}
				}
				onTrue.Invoke();
				onReferenceChanged.Invoke(true);
			}
			else
			{
				for (int i = 0; i < references.Count; i++)
				{
					if (references[i].Value)
					{
						onTrue.Invoke();
						onReferenceChanged.Invoke(true);
						return;
					}
				}
				onFalse.Invoke();
				onReferenceChanged.Invoke(false);
			}

		}

		private void OnDisable()
		{
			if (stopListeningOnDisable)
				return;

			for (int i = 0; i < references.Count; i++)
			{
				if (!references[i].reference)
					continue;
				references[i].reference.OnValueChanged -= OnReferenceChanged;
			}
			_listening = false;
		}

		private void OnDestroy()
		{
			if (!stopListeningOnDisable)
				return;

			for (int i = 0; i < references.Count; i++)
			{
				if (!references[i].reference)
					continue;
				references[i].reference.OnValueChanged -= OnReferenceChanged;
			}
		}
	}
}
