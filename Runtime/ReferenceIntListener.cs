using UnityEngine;
using UnityEngine.Events;

namespace TW.ReferenceObjects
{
	public class ReferenceIntListener : MonoBehaviour
	{
		[SerializeField] private ReferenceIntBase m_ReferenceInt;
		[SerializeField] private bool _notifyOnEnable = true;
		public UnityEvent<int> onValueChanged = new();
		public UnityEvent<bool> onNonDefaultChanged = new();
		public UnityEvent<string> onValueChangedString = new();

		private void OnEnable()
		{
			if (m_ReferenceInt)
			{
				m_ReferenceInt.OnValueChanged += OnValueChanged;
				if (_notifyOnEnable)
				{
					Notify();
				}
			}
		}

		public void Notify()
		{
			if (m_ReferenceInt)
			{
				OnValueChanged(m_ReferenceInt.Value);
			}
		}

		private void OnValueChanged(int obj)
		{
			onValueChanged.Invoke(obj);
			onValueChangedString.Invoke(obj.ToString());
			onNonDefaultChanged.Invoke(obj != m_ReferenceInt.defaultValue);
		}

		private void OnDisable()
		{
			if (m_ReferenceInt)
			{
				m_ReferenceInt.OnValueChanged += OnValueChanged;
			}
		}
	}

}
