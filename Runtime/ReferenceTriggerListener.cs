using UnityEngine;
using UnityEngine.Events;

namespace TW.ReferenceObjects
{
	public class ReferenceTriggerListener : MonoBehaviour
	{
		[SerializeField] private ReferenceTrigger _trigger;
		public UnityEvent OnTrigger = new UnityEvent();
		private void OnEnable()
		{
			if (_trigger != null)
			{
				_trigger.OnTriggered += OnTriggered;
			}
		}

		private void OnDisable()
		{
			if (_trigger != null)
			{
				_trigger.OnTriggered -= OnTriggered;
			}
		}

		private void OnTriggered()
		{
			OnTrigger.Invoke();
		}

		public void Trigger()
		{
			if (_trigger != null)
			{
				_trigger.Trigger();
			}
		}
	}

}