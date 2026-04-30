using UnityEngine;

namespace TW.ReferenceObjects
{

	[CreateAssetMenu(fileName = "ReferenceTrigger", menuName = "Data/Reference/Trigger", order = 0)]
	public class ReferenceTrigger : ScriptableObject
	{
		public event System.Action OnTriggered;

		public void Trigger()
		{
			OnTriggered?.Invoke();
		}
	}
}
